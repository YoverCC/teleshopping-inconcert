using Serilog;
using Serilog.Enrichers;
using System;
using System.Collections.Generic;
using System.Configuration;
using TeleshoppingConsole.Models;
using TeleshoppingConsole.Utils;

namespace TeleshoppingConsole
{
    internal class Program
    {
        private static readonly string dirLogs = ConfigurationManager.AppSettings["DirLogs"];
       private static readonly string minutes = ConfigurationManager.AppSettings["Minutes"];

        public static double Minutes => double.Parse(minutes);

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.With(new ThreadIdEnricher())
                .WriteTo
                .File(
                    $@"{dirLogs}\TeleshoppingConsole-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7
                 )
                .CreateLogger();
            var hasta = DateTime.UtcNow;
            var desde = hasta.AddMinutes(- Minutes);
            Console.WriteLine(hasta);
            Console.WriteLine(desde);


            //desde = DateTime.Parse("2023-06-02 12:00:00.000");
            //hasta = DateTime.Parse("2023-06-03 00:00:00.000");
            // 01 venta probada de "2023-06-02 12:00:00.000" a "2023-06-03 00:00:00.000"
            try
            {
                ITraerTazaActual traerTazaActual = new BitServiceSOAP();
                ITraerClientes traerClientes = new BaseDeDatos();
                //var clientes = traerClientes.TraerClientes(desde, hasta); //DateTime.Parse("2023-05-25 00:00:00.000"), DateTime.Parse("2023-05-27 00:00:00.000")
                //IEnviarClientes enviarClientes = new BitServiceSOAP();
                //enviarClientes.EnviarClientes(clientes);
                IEnviarVenta enviarVenta = new BitServiceSOAP();
                enviarVenta.EnviarVentas(desde, hasta); //DateTime.Parse("2023-05-19 18:25:48.000"), DateTime.Parse("2023-05-25 19:12:27.000")

                Console.WriteLine("Fin de Consumo Ws Clientes, Taza Cambio, Envio de Ventas");
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }
    }
}
