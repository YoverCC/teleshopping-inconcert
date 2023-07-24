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
using TeleshoppingConsole.Utils;
using static BitWebServiceArticulos.wsGenQuerySoapClient;
using static BitWebServiceClientes.WsGenDocsSoapClient;
using static BitWebServiceTazaDeCambio.wsGenQuerySoapClient;
using System.Data.SqlTypes;
using System.Xml.Linq;
using Microsoft.IdentityModel.Tokens.Saml;
using static TeleshoppingConsole.Models.DOCUMENTO;

namespace TeleshoppingConsole.Models
{
    public class BitServiceSOAP : IEnviarVenta, IEnviarClientes, ITraerTazaActual
    {
        private readonly string _USERNAME;
        private readonly string _PASSWORD;
        private readonly Logger _LOGGER;
        private readonly IAgregarCodigoBitClientes agregarCodigoBitClientes;
        private BaseDeDatos baseDeDatos;

        public BitServiceSOAP()
        {
            _USERNAME = ConfigurationManager.AppSettings["usuarioBit"];
            _PASSWORD = ConfigurationManager.AppSettings["passwordBit"];
            _LOGGER = new Logger();
            agregarCodigoBitClientes = new BaseDeDatos();
        }

        public string EnviarCliente(Cliente cliente)
        {
            //string keyCodeBitClientes = ConfigurationManager.AppSettings["keyCodeBitClientes"];
            wsGenQuerySoapClient bitWebService =
                new(wsGenQuerySoapClient.EndpointConfiguration.wsGenQuerySoap);
            bitWebService.ClientCredentials.UserName.UserName = _USERNAME;
            bitWebService.ClientCredentials.UserName.Password = _PASSWORD;
            var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(Cliente));
            var settings = new XmlWriterSettings();
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            var writer = XmlWriter.Create(stringWriter, settings);
            serializer.Serialize(writer, cliente, emptyNamespaces);
            baseDeDatos = new BaseDeDatos();
            try
            {
                var request = bitWebService.GetDataAsync("INCONCERT_CUSTOMER_UPD",
                stringWriter.ToString());
                _LOGGER.Debug($"Se envio el cliente {stringWriter}");
                request.Wait();
                _LOGGER.Info(request.Result);
                try
                {
                    XmlSerializer serializerCliente = new(typeof(ClienteNewDataSet));
                    byte[] byteArray = Encoding.UTF8.GetBytes(request.Result);
                    var response = stringWriter.ToString();
                    baseDeDatos.InsertarLogDeRequestReponseBit(stringWriter.ToString(), request.Result);
                    MemoryStream stream = new(byteArray);
                    var responseWS = (ClienteNewDataSet)serializerCliente.Deserialize(stream);
                    return responseWS.Table.ClienteCreado.ToString();
                }
                catch (Exception ex)
                {
                    _LOGGER.Error(ex, $"No se pudo deserializar la respuesta del cliente: {request.Result}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, ex.Message);
                return null;
            }
        }

        public void EnviarClientes(List<Cliente> clientes)
        {
            clientes.ForEach(cliente =>
            {
                var IdBit = EnviarCliente(cliente);
                if (IdBit != null)
                {
                    if(agregarCodigoBitClientes.AgregarCodigoBitClientes(cliente.IdCliente, IdBit.ToString()))
                    {
                        _LOGGER.Info($"Se pudo actualizar el código bit al cliente {cliente.IdCliente} con código Bit {IdBit}");
                    };
                }
            });
        }

        public void EnviarVentas(DateTime desde, DateTime hasta)
        {
            baseDeDatos = new BaseDeDatos();
            WsGenDocsSoapClient bitWebService =
                new(WsGenDocsSoapClient.EndpointConfiguration.WsGenDocsSoap);
            bitWebService.ClientCredentials.UserName.UserName = _USERNAME;
            bitWebService.ClientCredentials.UserName.Password = _PASSWORD;
            IParsearVenta parsearVenta = new ParsearVentaBit();
            List<Description> ventas = parsearVenta.ParsearVenta(DateTime.Parse(desde.ToString()), DateTime.Parse(hasta.ToString()));
            var serializer = new XmlSerializer(typeof(Description));
            var settings = new XmlWriterSettings();
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            //var writer = XmlWriter.Create(stringWriter, settings);
            ventas.ForEach(venta =>
            { 
                venta = procesarUnaVenta(ventas, venta.NrPedido);
                try
                {
                    var stringWriter = new StringWriter();
                    serializer.Serialize(stringWriter, venta, emptyNamespaces);
                    _LOGGER.Debug($"La venta que se mando fue: {stringWriter}");
                    var request = bitWebService.GenerateAsync(stringWriter.ToString());
                    request.Wait();
                    baseDeDatos.InsertarLogDeRequestReponseBit(stringWriter.ToString(), request.Result);
                    // añadir validaciòn para guardar solo ventas generadas

                    string getDocVenta = request.Result;
                    XDocument xmlDocument = XDocument.Parse(getDocVenta);
                    var serializerResponse = new XmlSerializer(typeof(DOCUMENTO), new XmlRootAttribute("DOCUMENTO"));
                    var stringWriterResponse = new StringWriter();
                    DOCUMENTO documento = (DOCUMENTO)serializerResponse.Deserialize(xmlDocument.CreateReader());
                   
                    if (documento.ERROR == null)
                    {
                        baseDeDatos.GuardarVentasBit(venta.NrPedido, documento.NRODOC, documento.NROORDEN);
                        Console.WriteLine(documento.NROORDEN);
                    }
                    else { 
                        _LOGGER.Info(request.Result.ToString());
                        Console.WriteLine($"Hubo un error al enviar la venta debido a: {documento.ERROR.DESC}");
                    }

                    _LOGGER.Info(request.Result.ToString());

                }
                catch (TimeoutException)
                {
                    baseDeDatos.InsertarReintentoTimedOutBit(venta.IdCliente, venta.IdCall, venta.NrPedido);
                }
            });

        }

        public Description procesarUnaVenta(List<Description> ventas, string nrPedido) {

            var venta = new Description();

            venta = ventas.Find(venta => venta.NrPedido == nrPedido);

            return venta;

        }

        public double ObtenerLaTazaActualDeDolaresXPesos()
        {
            string keyCodeBitTaza = "INCONCERT_TIPOCAMBIO";
            var fecha = DateTime.UtcNow.ToString("yyyyMMdd");
            BitWebServiceTazaDeCambio.wsGenQuerySoapClient bitWebService = 
                new(BitWebServiceTazaDeCambio.wsGenQuerySoapClient.EndpointConfiguration.wsGenQuerySoap);
            bitWebService.ClientCredentials.UserName.UserName = _USERNAME;
            bitWebService.ClientCredentials.UserName.Password = _PASSWORD;
            var request = bitWebService.GetDataAsync(keyCodeBitTaza, $"<params><Fecha>{fecha}</Fecha></params>");
            request.Wait();
            XmlSerializer serializer = new(typeof(TazaCambioNewDataSet));
            byte[] byteArray = Encoding.UTF8.GetBytes(request.Result);
            MemoryStream stream = new(byteArray);
            try
            {
                var taza = (TazaCambioNewDataSet) serializer.Deserialize(stream);
                return taza.Table.TipoCambio;
            }
            catch (Exception ex)
            {
                _LOGGER.Error(ex, $"No se pudo deserializar la repuesta de la taza de cambio: {request.Result}");
                return 0;
            }
        }
    }
}
