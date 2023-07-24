using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeleshoppingConsoleArticulo.Utils;

namespace TeleshoppingConsoleArticulo.Models
{
    public class BaseDeDatos : ITratamientoArticulos
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

        public void TratarArticulo(List<Articulo> articulos)
        {
            try
            {
                if(articulos != null)
                {
                    using (this.connection)
                    {
                        this.connection.Open();
                        SqlCommand cmd = this.connection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "TratamientoArticulos";
                        cmd.Parameters.Add("@Codigo", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Estado", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Descripcion", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Familia", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Stock", System.Data.SqlDbType.Int);
                        cmd.Parameters.Add("@Talle", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Color", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@Precio", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@CallBackS", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@Especial", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@Regalo", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@Entrante", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@Saliente", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@PrecioUSD", System.Data.SqlDbType.Float);
                        articulos.ForEach(articulo =>
                        {
                            _LOGGER.Info(JsonSerializer.Serialize(articulo));
                            cmd.Parameters["@Codigo"].Value = articulo.Codigo;
                            cmd.Parameters["@Estado"].Value = articulo.Estado;
                            cmd.Parameters["@Descripcion"].Value = articulo.Descripcion;
                            cmd.Parameters["@Familia"].Value = articulo.Familia;
                            cmd.Parameters["@Stock"].Value = articulo.Stock;
                            cmd.Parameters["@Talle"].Value = articulo.Talle;
                            cmd.Parameters["@Color"].Value = articulo.Color;
                            cmd.Parameters["@Precio"].Value = articulo.Precio;
                            cmd.Parameters["@CallBackS"].Value = articulo.CallBackS;
                            cmd.Parameters["@Especial"].Value = articulo.Especial;
                            cmd.Parameters["@Regalo"].Value = articulo.Regalo;
                            cmd.Parameters["@Entrante"].Value = articulo.Entrante;
                            cmd.Parameters["@Saliente"].Value = articulo.Saliente;
                            cmd.Parameters["@PrecioUSD"].Value = articulo.PrecioUSD;
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                var resultado = reader.GetString(0);
                                if (resultado != "OK")
                                {
                                    _LOGGER.Error(resultado);
                                }
                                else
                                {
                                    _LOGGER.Info(resultado);
                                }
                            }
                            reader.Close();
                        });
                    }
                }
                else
                {
                    _LOGGER.Info("No hay articulos que tratar");
                }
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, "Hubo un error en tratar los articulos");
            }
        }
    }
}
