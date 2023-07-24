using BitWebServiceArticulos;
using BitWebServiceClientes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TeleshoppingConsoleArticulo.Utils;
using static BitWebServiceArticulos.wsGenQuerySoapClient;
using static BitWebServiceClientes.WsGenDocsSoapClient;
using static BitWebServiceTazaDeCambio.wsGenQuerySoapClient;
using System.Data.SqlTypes;
using static TeleshoppingConsoleArticulo.Models.ListaArticulos;
using System.Xml.Linq;

namespace TeleshoppingConsoleArticulo.Models
{
    public class BitServiceSOAP : IObtenerArticulos
    {
        private readonly string _USERNAME;
        private readonly string _PASSWORD;
        private readonly Logger _LOGGER;
        public BitServiceSOAP()
        {
            _USERNAME = ConfigurationManager.AppSettings["usuarioBit"];
            _PASSWORD = ConfigurationManager.AppSettings["passwordBit"];
            _LOGGER = new Logger();
        }       
        public List<Articulo> GetArticulos()
        {
            string keyCodeBit = ConfigurationManager.AppSettings["keyCodeBit"];
            var fecha = DateTime.UtcNow.AddDays(-1).ToString("yyyyMMdd");
            Console.WriteLine(fecha);
            wsGenQuerySoapClient bitWebService =
                new(wsGenQuerySoapClient.EndpointConfiguration.wsGenQuerySoap);
            bitWebService.ClientCredentials.UserName.UserName = _USERNAME;
            bitWebService.ClientCredentials.UserName.Password = _PASSWORD;

            Task<string> request;
            if (ConfigurationManager.AppSettings["Modificado"] == "1")
            {
                request = bitWebService.GetDataAsync(keyCodeBit, $"<params><modificado>{fecha}</modificado></params>"); //{fecha}
                _LOGGER.Debug($"<params><modificado>{fecha}</modificado></params>");
            }
            else {
                request = bitWebService.GetDataAsync(keyCodeBit, $"<params><modificado></modificado></params>");
                _LOGGER.Debug($"<params><modificado></modificado></params>");
            }

            request.Wait();
            string getListaArticulos = request.Result;
            _LOGGER.Debug($"Los artìculos existentes en BIT son: {request.Result.ToString()}");
            XDocument xmlDocument = XDocument.Parse(getListaArticulos);
            XmlSerializer xmlSerializer = new(typeof(NewDataSet), new XmlRootAttribute("NewDataSet"));
            try
            {
                NewDataSet listaDeArticulos = (NewDataSet)xmlSerializer.Deserialize(xmlDocument.CreateReader());
                var articulos = listaDeArticulos.Table.ToList();

                List<Articulo> Articulos = listaDeArticulos.Table.Select(
                    articulo => new Articulo
                    {
                        Codigo = articulo.codigo,
                        Descripcion = articulo.descripcion,
                        Estado = articulo.estado,
                        CallBackS = articulo.CallBackS,
                        Color = articulo.color,
                        ColorDescripcion = articulo.colorDescr,
                        Comentarios = articulo.comentarios,
                        Entrante = articulo.ENTRANTE,
                        Especial = articulo.ESPECIAL,
                        Familia = articulo.familia,
                        Precio = articulo.PRECIO,
                        Regalo = articulo.REGALO,
                        Saliente = articulo.SALIENTE,
                        PrecioUSD = articulo.PRECIOUSD,
                        Stock = articulo.stock,
                        Talle = articulo.talle,
                        TalleDescripcion = articulo.talleDescr
                    }).ToList();
                Console.WriteLine(Articulos.ToString());
                _LOGGER.Debug($"Los artìculos deserializados a guardar son: {request.Result.ToString()}");
                return Articulos;
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"No se pudo deserializar los articulos: {request.Result.ToString()}");
                return null;
            }
        }
    }
}
