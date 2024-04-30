using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Utils
{
    public class ObtenerDatosDelJson
    {
        private static readonly string PathJson = ConfigurationManager.AppSettings["PathJson"];

        public static void SerializarJson(DatosUtiles datosUtiles)
        {
            string datosUtilesJson = JsonConvert.SerializeObject(datosUtiles);
            File.WriteAllText(PathJson, datosUtilesJson);
        }

        public static double ObtenerLaUltimaTaza()
        {
            string datosUtilesFromFile;
            using(var reader = new StreamReader(PathJson))
            {
                datosUtilesFromFile = reader.ReadToEnd();
            }
            var datosUtiles = JsonConvert.DeserializeObject<DatosUtiles>(datosUtilesFromFile);
            return datosUtiles.UltimaTaza;
        }
    }
}
