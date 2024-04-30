using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeleshoppingConsole.Utils;

namespace TeleshoppingConsole.Models
{
    public class BaseDeDatos : ITraeVentaDetalle, ITraerVenta, ITraerMetodoPago, ITraerClientes, IAgregarCodigoBitClientes, ITraeCostoEnvio, IEnviarTipoDeCambio
    {
        private readonly SqlConnection connection;
        private readonly Logger _LOGGER;
        public BaseDeDatos()
        {
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = ConfigurationManager.AppSettings["BDIP"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["BDShema"];
            builder.UserID = ConfigurationManager.AppSettings["BDUser"];
            builder.Password = ConfigurationManager.AppSettings["BDUsrPass"];
            connection = new SqlConnection(builder.ConnectionString);
            _LOGGER = new Logger();
        }

        public List<TraeVentaDet> TraeVentaDetalles(string idCall)
        {
            List<TraeVentaDet> detalles = new();
            this.connection.Open();
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "TraeVentasDetBIT";
            cmd.Parameters.Add("@IdCall", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@IdCall"].Value = idCall;
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    detalles.Add(new TraeVentaDet()
                    {
                        NumPedido = Decimal.Parse(reader["NumPedido"].ToString()),
                        Renglon = int.Parse(reader["Renglon"].ToString()),
                        CodigoPro = reader["CodigoProd"].ToString(),
                        Producto = reader["Producto"].ToString(),
                        Cantidad = int.Parse(reader["Cantidad"].ToString()),
                        Precio = Double.Parse(reader["Precio"].ToString()),
                        linea = reader["linea"].ToString(),
                        Talla = reader["Talla"].ToString(),
                        Color = reader["Color"].ToString(),
                        ListaPrecio = reader["ListaPrecio"].ToString(),
                        TipoMoneda = reader["MonedaArt"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"No se pudo procesar los detalle de la venta {idCall}");
                return null;
            }
            finally
            {
                this.connection.Close();
            }
            return detalles;
        }

        public List<TraeVenta> TraeVentas(DateTime desde, DateTime hasta)
        {
            List<TraeVenta> ventas = new();
            using (this.connection)
            {
                this.connection.Open();
                SqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "TraeVentasBitFinal";
                cmd.Parameters.Add("@FechaIni", System.Data.SqlDbType.DateTime);
                cmd.Parameters.Add("@FechaFin", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@FechaIni"].Value = desde;
                cmd.Parameters["@FechaFin"].Value = hasta;
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        ventas.Add(new TraeVenta()
                        {
                            NombreC = reader["NombreC"].ToString(),
                            IDBit = reader["IDBit"].ToString(),
                            IDCliente = reader["IDCliente"].ToString(),
                            NumPedido = Decimal.Parse(reader["NumPedido"].ToString()),
                            TelCasa = reader["TelCasa"].ToString(),
                            TelCel = reader["TelCel"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Departamento = reader["Departamento"].ToString(),
                            Ciudad = reader["Ciudad"].ToString(),
                            Observaciones = reader["Observaciones"].ToString(),
                            IdCall = reader["IdCall"].ToString(),
                            Referencia = reader["Referencia"].ToString(),
                            Usuario = reader["Usuario"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    _LOGGER.Error(ex, $"No se pudo procesar las ventas desde {desde} hasta {hasta}");
                    return null;
                }
            }
            return ventas;
        }

        public List<TraeVentasPago> TraeMetodoPago(decimal numPedido)
        {
            this.connection.Open();
            List<TraeVentasPago> traeVentasPagos = new();
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "TraeVentasPagos2";
            cmd.Parameters.Add("@NrPedi0003", System.Data.SqlDbType.Decimal);
            cmd.Parameters["@NrPedi0003"].Value = numPedido;
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    traeVentasPagos.Add(new TraeVentasPago()
                    {
                        MontoTotal = Double.Parse(reader["MontoTotal"].ToString()),
                        Autoriza = reader["Autoriza"].ToString(),
                        Cuotas = reader["Cuotas"].ToString(),
                        Banco = reader["Banco"].ToString(),
                        MonedaFactura = reader["MonedaFactura"].ToString(),
                        MonedaPag = reader["MonedaPag"].ToString(),
                        TDC = reader["TDC"].ToString(),
                        TipoCambio = Double.Parse(reader["TipoCambio"].ToString()),
                        FechaEntrega = !string.IsNullOrEmpty(reader["FechaEntrega"].ToString()) ? DateTime.Parse(reader["FechaEntrega"].ToString()).ToString("dd/MM/yyyy") : "",
                        
                    });
                }
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"No se pudo procesar el metodo de pago de la venta con el numero de pedido {numPedido}");
                return null;
            }
            finally
            {
                this.connection.Close();
            }
                return traeVentasPagos;
            
        }

        public List<Cliente> TraerClientes(DateTime desde, DateTime hasta)
        {
            using (this.connection)
            {
                this.connection.Open();
                List<Cliente> clientes = new();
                SqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "TraerClientes";
                cmd.Parameters.Add("@FechaInicio", System.Data.SqlDbType.DateTime);
                cmd.Parameters.Add("@FechaFinal", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@FechaInicio"].Value = desde;
                cmd.Parameters["@FechaFinal"].Value = hasta;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        try
                        {
                            clientes.Add(new Cliente()
                            {
                                IdCliente = reader["IdCliente"].ToString(),
                                Doc = reader["Doc"].ToString(),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                FechaNac = !string.IsNullOrEmpty(reader["FechaNac"].ToString()) ? DateTime.Parse(reader["FechaNac"].ToString()).ToString("yyyyMMdd") : "19000101",
                                Telefono = reader["Telefono"].ToString(),
                                Celular = reader["Celular"].ToString(),
                                DireccionData = reader["Direccion"].ToString(),
                                NroPuerta = reader["NroPuerta"].ToString(),
                                BisPiso = reader["BisPiso"].ToString(),
                                Apto = reader["Apto"].ToString(),
                                Entre1 = reader["Entre1"].ToString(),
                                Entre2 = reader["Entre2"].ToString(),
                                Ciudad = reader["Ciudad"].ToString(),
                                Barrio = reader["Barrio"].ToString(),
                                Mail = reader["Mail"].ToString(),
                                Justificacion = reader["Justificacion"].ToString(),
                                RazonSocial = reader["RazonSocial"].ToString(),
                                Campana = reader["Campana"].ToString(),
                                Agente = reader["Agente"].ToString(),
                                NroLlamada = reader["NroLlamada"].ToString(),
                                Resultado = reader["Resultado"].ToString()
                            });
                        }
                        catch (Exception ex)
                        {
                            _LOGGER.Error(ex, $"No se pudo mapear el cliente con el ID {reader["IdCliente"]}");
                        }
                    }
                    return clientes;
                }
                catch (Exception ex)
                {
                    _LOGGER.Error(ex, "No se pudo traer a los clientes en BD");
                    return null;
                }
            }
        }

        public bool AgregarCodigoBitClientes(string IdCliente, string CodigoBit)
        {
            this.connection.Open();
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "AgregarIDBitCliente";
            cmd.Parameters.Add("@IdBit", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@IdCliente", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@IdBit"].Value = CodigoBit;
            cmd.Parameters["@IdCliente"].Value = IdCliente;
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    if (reader[0].ToString() != "OK")
                    {
                        _LOGGER.Error($"No se pudo actualizar el código bit al cliente {IdCliente} con código Bit {CodigoBit}");
                        return false;
                    }
                    return true;
                 }
                 return false;
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"No se pudo actualizar el código bit al cliente {IdCliente} con código Bit {CodigoBit} por error de BD");
                this.connection.Close();
                return false;
            }
            finally
            {
                this.connection.Close();
            }
        }

        public Cliente TraerClientePorId(string IDCliente)
        {
            this.connection.Open();
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "TraerCliente";
            cmd.Parameters.Add("@IdCliente", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@IdCliente"].Value = IDCliente;
            Cliente cliente;
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    try
                    {
                        cliente = new Cliente()
                        {
                            IdCliente = reader["IdCliente"].ToString(),
                            Doc = reader["Doc"].ToString(),
                            Nombres = reader["Nombres"].ToString(),
                            Apellidos = reader["Apellidos"].ToString(),
                            FechaNac = !string.IsNullOrEmpty(reader["FechaNac"].ToString()) ? DateTime.Parse(reader["FechaNac"].ToString()).ToString("yyyyMMdd") : "19000101",
                            Telefono = reader["Telefono"].ToString(),
                            Celular = reader["Celular"].ToString(),
                            DireccionData = reader["Direccion"].ToString(),
                            NroPuerta = reader["NroPuerta"].ToString(),
                            BisPiso = reader["BisPiso"].ToString(),
                            Apto = reader["Apto"].ToString(),
                            Entre1 = reader["Entre1"].ToString(),
                            Entre2 = reader["Entre2"].ToString(),
                            Ciudad = reader["Ciudad"].ToString(),
                            Barrio = reader["Barrio"].ToString(),
                            Mail = reader["Mail"].ToString(),
                            Justificacion = reader["Justificacion"].ToString(),
                            RazonSocial = reader["RazonSocial"].ToString(),
                            Campana = reader["Campana"].ToString(),
                            Agente = reader["Agente"].ToString(),
                            NroLlamada = reader["NroLlamada"].ToString(),
                            Resultado = reader["Resultado"].ToString()
                        };
                        return cliente;
                    }
                    catch (Exception ex)
                    {
                        _LOGGER.Error(ex, $"No se pudo mapear el cliente con el ID {IDCliente}");
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"No se pudo traer al cliente con ID: {IDCliente} en BD");
                return null;
            }
            finally
            {
                this.connection.Close();
            }
        }

        public void InsertarLogDeRequestReponseBit(string request, string response)
        {
            this.connection.Open();
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertarLogDeRequestReponseBit";
            cmd.Parameters.Add("@Request", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@Response", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@Request"].Value = request;
            cmd.Parameters["@Response"].Value = response;
            cmd.ExecuteNonQuery();
            this.connection.Close();
        }

        public void InsertarReintentoTimedOutBit(string IdCliente, string IdCall, string NrPedido)
        {
            this.connection.Open();
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertarTimedOutWS";
            cmd.Parameters.Add("@IdCliente", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@IdCall", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@NrPedido", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@IdCliente"].Value = IdCliente;
            cmd.Parameters["@IdCall"].Value = IdCall;
            cmd.Parameters["@NrPedido"].Value = NrPedido;
            //cmd.ExecuteNonQuery();
            this.connection.Close();
        }

        public List<TraeVenta> TraeVentasIntento()
        {
            List<TraeVenta> ventas = new();
            using (this.connection)
            {
                this.connection.Open();
                SqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "TraeVentasIntento";
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        ventas.Add(new TraeVenta()
                        {
                            NombreC = reader["NombreC"].ToString(),
                            IDBit = reader["IDBit"].ToString(),
                            IDCliente = reader["IDCliente"].ToString(),
                            NumPedido = Decimal.Parse(reader["NumPedido"].ToString()),
                            TelCasa = reader["TelCasa"].ToString(),
                            TelCel = reader["TelCel"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Departamento = reader["Departamento"].ToString(),
                            Ciudad = reader["Ciudad"].ToString(),
                            Observaciones = reader["Observaciones"].ToString(),
                            IdCall = reader["IdCall"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    _LOGGER.Error(ex, $"No se pudo procesar las ventas intento");
                    return null;
                }
            }
            return ventas;
        }

        public CostoEnvio TraeCosteEnvioByNroPedido(decimal numPedido)
        {
            this.connection.Open();
            CostoEnvio traeCostoEnvio;
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "TraeCosteEnvioByNroPedido";
            cmd.Parameters.Add("@NrPedido", System.Data.SqlDbType.Decimal);
            cmd.Parameters["@NrPedido"].Value = numPedido;
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                  if (reader.Read())
                {
                    try
                    {
                        traeCostoEnvio = new CostoEnvio()
                        {
                            Precio = reader["Precio"].ToString()
                        };
                        return traeCostoEnvio;
                    }
                    catch (Exception ex)
                    {
                        _LOGGER.Error(ex, $"No se pudo mapear el costo del envio");
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"No se pudo traer costo de envio desde BD");
                return null;
            }
            finally
            {
                this.connection.Close();
            }
        }
        //implementar
        public void GuardarVentasBit(string nroPedido, int nroDoc, string nroOrden) {

            this.connection.Open();
            SqlCommand cmd = this.connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertarVentasBit";
            cmd.Parameters.Add("@NroPedido", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@NroDoc", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@NroOrden", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@NroPedido"].Value = nroPedido;
            cmd.Parameters["@NroDoc"].Value = nroDoc.ToString();
            cmd.Parameters["@NroOrden"].Value = nroOrden;
            cmd.ExecuteNonQuery();
            this.connection.Close();
        }

        public string GuardarTipoDeCambio(double tipodecambio)
        {

            
            try
            {
                this.connection.Open();
                SqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_UpdateMoneda";
                cmd.Parameters.Add("@moneda", System.Data.SqlDbType.Decimal);
                cmd.Parameters["@moneda"].Value = tipodecambio;
                cmd.ExecuteNonQuery();
                this.connection.Close();
                return "ok";
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"Error en la funcion GuardarTipoDeCambio");
                return "failed";
            }
            finally
            {
                this.connection.Close();
            }

            
        }
    }
}
