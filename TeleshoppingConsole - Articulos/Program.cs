using Serilog;
using Serilog.Enrichers;
using System;
using System.Collections.Generic;
using System.Configuration;
using TeleshoppingConsoleArticulo.Models;
using TeleshoppingConsoleArticulo.Utils;

namespace TeleshoppingConsoleArticulo
{
    internal class Program
    {
        private static readonly string dirLogs = ConfigurationManager.AppSettings["DirLogs"];
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.With(new ThreadIdEnricher())
                .WriteTo
                .File(
                    $@"{dirLogs}\TeleshoppingConsoleArticulo-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7
                 )
                .CreateLogger();
            var hasta = DateTime.UtcNow;
            var desde = hasta.AddMinutes(-30);
            try
            {               
               IObtenerArticulos obtenerArticulos = new BitServiceSOAP();
               List<Articulo> listaArticulos = obtenerArticulos.GetArticulos();
               ITratamientoArticulos tratamientoArticulos = new BaseDeDatos();
               tratamientoArticulos.TratarArticulo(listaArticulos);

                Console.WriteLine("Fin de consumo de artìculos");
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }
    }
}
