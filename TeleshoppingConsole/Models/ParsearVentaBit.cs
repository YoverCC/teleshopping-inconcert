using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleshoppingConsole.Utils;

namespace TeleshoppingConsole.Models
{
    public class ParsearVentaBit : IParsearVenta
    {
        private readonly ITraerVenta traerVenta;
        private readonly ITraeVentaDetalle traeVentaDetalle;
        private readonly ITraerMetodoPago traerMetodoPago;
        private readonly IEnviarClientes enviarCliente;
        private readonly IAgregarCodigoBitClientes agregarCodigoBitCliente;
        private readonly ITraerClientes traerCliente;
        private readonly ITraerTazaActual traerTazaActual;
        private static IDictionary<string, string> CodigosInConcertXBit;
        private readonly ITraeCostoEnvio traeCostoEnvio;
        private BaseDeDatos baseDeDatos;
        private readonly Logger _LOGGER;

        public ParsearVentaBit()
        {
            _LOGGER = new Logger();
            traerVenta = new BaseDeDatos();
            traeVentaDetalle = new BaseDeDatos();
            traerMetodoPago = new BaseDeDatos();
            enviarCliente = new BitServiceSOAP();
            agregarCodigoBitCliente = new BaseDeDatos();
            traerCliente = new BaseDeDatos();
            traeCostoEnvio = new BaseDeDatos();
            traerTazaActual = new TraerTazaActualUtils();
            CodigosInConcertXBit = new Dictionary<string, string>
            {
                { "PRECIO", "P" },
                { "CALL BACK S", "CB" },
                { "ESPECIAL", "E" },
                { "REGALO", "R" },
                { "ENTRANTE", "EN" },
                { "SALIENTE", "S" }
            };
        }

        public List<Description> ParsearVenta(DateTime desde, DateTime hasta)
        {
            baseDeDatos = new BaseDeDatos();
            List<Description> ventasDocumento = new();
            List<TraeVenta> ventas = traerVenta.TraeVentas(desde, hasta);
            List<TraeVenta> ventasIntento = baseDeDatos.TraeVentasIntento();
            if (ventas != null)
            {
                if (ventasIntento != null && ventasIntento.Count > 0)
                {
                    ventas.AddRange(ventasIntento);
                }
                //var tazaActual = traerTazaActual.ObtenerLaTazaActualDeDolaresXPesos();
                var tazaActual = 40.7;
                ventas.ForEach(venta =>
                {
                    try
                    {
                        Description documento = new();
                        documento.NrPedido = venta.NumPedido.ToString();
                        documento.IdCliente = venta.IDCliente.ToString();
                        documento.IdCall = venta.IdCall.ToString();
                        List<TraeVentasPago> traeVentasPago = traerMetodoPago.TraeMetodoPago(venta.NumPedido);
                        var IDBit = "";
                        var siProcesoIDBit = true;
                        if (String.IsNullOrEmpty(venta.IDBit))
                        {
                            var cliente = traerCliente.TraerClientePorId(venta.IDCliente);
                            if (cliente != null)
                            {
                                IDBit = enviarCliente.EnviarCliente(cliente);
                            }
                            if (!String.IsNullOrEmpty(IDBit))
                            {
                                agregarCodigoBitCliente.AgregarCodigoBitClientes(venta.IDCliente, IDBit);
                            }
                            else
                            {
                                siProcesoIDBit = false;
                            }
                        }
                        else
                        {
                            IDBit = venta.IDBit;
                        }
                        if (siProcesoIDBit)
                        {
                            Cabezal cabezal = new()
                            {
                                Cliente = IDBit == null ? "" : IDBit,
                                Numero = "",
                                Modelo = traeVentasPago[0].MonedaPag == "1" ? "PedInConcert$" : "PedInConcertU$S", //PedInConcert$-Pesos - PedInConcertU$S-Dolares
                                Fecha = DateTime.UtcNow.ToString("dd/MM/yyyy"),
                                ClienteNombre = venta.NombreC,
                                ListaPrecio =
                                traeVentasPago[0].MonedaPag == "1" ? "P" : "1",
                                ClienteRazon = venta.NombreC,
                                ClienteDepartamento = String.IsNullOrEmpty(venta.Departamento) ? "N/A" : venta.Departamento,
                                ClienteCiudad = String.IsNullOrEmpty(venta.Ciudad) ? "N/A" : venta.Ciudad,
                                ClienteDirection = String.IsNullOrEmpty(venta.Direccion) ? "N/A" : venta.Direccion,
                                Contacto = String.IsNullOrEmpty(venta.TelCel) ? "N/A" : venta.TelCasa,
                                Comentario = String.IsNullOrEmpty(venta.Observaciones) ? "N/A" : venta.Observaciones,
                                FechaReq = traeVentasPago[0].FechaEntrega,
                                Mensaje = "0",
                                Referencia = String.IsNullOrEmpty(venta.Referencia) ? "" : venta.Referencia,
                                Usuario = String.IsNullOrEmpty(venta.Usuario) ? "" : venta.Usuario

                            };
                            documento.Cabezal = cabezal;
                            List<TraeVentaDet> detalles = traeVentaDetalle.TraeVentaDetalles(venta.IdCall);
                            CostoEnvio costoEnvio = traeCostoEnvio.TraeCosteEnvioByNroPedido(venta.NumPedido);
                            documento.Detalles = new();
                            int agregoEnvio = 0;
                            detalles.ForEach(detalle =>
                            {
                                tazaActual = traeVentasPago[0].TipoCambio;
                                documento.Detalles.Add(new Detalle()
                                {
                                    Articulo = detalle.CodigoPro,
                                    Talle = detalle.Talla, //SUBART1
                                Color = detalle.Color, //SUBART2                          
                                Cantidad = detalle.Cantidad.ToString(),
                                    Precio = traeVentasPago[0].MonedaPag == "1" && detalle.TipoMoneda == "1" ?
                                    (Math.Round(detalle.Precio * 100) / 100) : traeVentasPago[0].MonedaPag == "1" && detalle.TipoMoneda == "2" ?
                                    (Math.Round(detalle.Precio * tazaActual * 100) / 100) :
                                    (traeVentasPago[0].MonedaPag == "2" && detalle.TipoMoneda == "1" ?
                                    (Math.Round(detalle.Precio / tazaActual * 100) / 100) : detalle.Precio)
                                });

                                if (agregoEnvio == 0)
                                {
                                // SE ARMA EL NUEVO ARTICULO PARA EL COSTO DE ENVIO                       
                                documento.Detalles.Add(new Detalle()
                                    {
                                        Articulo = "ENVIO",
                                        Color = "0",
                                        Talle = "0",
                                        Cantidad = "0",
                                        //Precio = double.Parse(costoEnvio.Precio) != 0 ? double.Parse(costoEnvio.Precio) : 0,
                                        Precio = traeVentasPago[0].MonedaPag == "1" ? double.Parse(costoEnvio.Precio) : (Math.Round(double.Parse(costoEnvio.Precio) / tazaActual * 100) / 100),
                                });
                                    agregoEnvio++;
                                }

                            });
                            List<Pago> pagos = new();
                            traeVentasPago.ForEach(pago =>
                            {
                                double value = pago.MontoTotal;
                                double num = (Math.Round(value * 100) / 100);
                                var digiTarj = new string(pago.TDC.Reverse().Take(4).Reverse().ToArray());
                                Console.WriteLine(num);


                                pagos.Add(new Pago()
                                {

                                    Entidad = pago.Banco,
                                    Cuotas = !String.IsNullOrEmpty(pago.Cuotas) ? pago.Cuotas : "1",
                                    AutorizNum = !String.IsNullOrEmpty(pago.Autoriza) ? pago.Autoriza : "1",
                                    Tarjeta = !String.IsNullOrEmpty(digiTarj) ? digiTarj : "",
                                    //Monto = traeVentasPago[0].MonedaPag == "1" && pago.MonedaPag == "1" ?
                                    //(Math.Round(num)) : traeVentasPago[0].MonedaPag == "2" && pago.MonedaPag == "1" ?
                                    //(Math.Round(num)) : num,
                                    Monto = traeVentasPago[0].MonedaPag == "1" && pago.MonedaPag == "1" ?
                                    num : traeVentasPago[0].MonedaPag == "1" && pago.MonedaPag == "2" ?
                                    (Math.Round(num * tazaActual * 100) / 100) :
                                    (traeVentasPago[0].MonedaPag == "2" && pago.MonedaPag == "1" ?
                                    (Math.Round((num / tazaActual) * 100) / 100) : num)
                                });
                            });
                            documento.Pago = pagos;
                            ventasDocumento.Add(documento);
                        }
                        else
                        {
                            _LOGGER.Debug($"La venta con problema en generar cliente es: {venta.NumPedido}, con numero de cliente: {venta.IDCliente}, con ID de interaccion: {venta.IdCall}");
                            // No hago nada si hubo un error y no se tiene IDBit
                        }
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        _LOGGER.Debug($"La venta con error en proceso es: {venta.NumPedido}, con numero de cliente: {venta.IDCliente}, con ID de interaccion: {venta.IdCall}");
                        // No hago nada si hubo un error y no se tiene IDBit
                    }
                });
            }
            return ventasDocumento;
        }

        private static string ParsearCodigoListaDePrecio(string codigoInConcert)
        {
            CodigosInConcertXBit.TryGetValue(codigoInConcert, out string result);
            return result;
        }
    }
}
