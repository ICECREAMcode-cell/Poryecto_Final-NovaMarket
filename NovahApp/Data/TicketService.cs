using System;
using System.IO;
using System.Collections.Generic;
using NovahApp.Data;

namespace NovahApp.Services
{
    public class TicketService
    {
        public static void GenerarFactura(string nombreCliente, List<CarritoItem> items, double total)
        {
            // Creamos la ruta del archivo con el nombre del cliente y la fecha
            string carpetaDestino = @"D:\Zvscodeprojects\Facturas de compra";
            string nombreArchivo = $"Factura_{nombreCliente}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string ruta = Path.Combine(carpetaDestino, nombreArchivo);

            using (StreamWriter writer = new StreamWriter(ruta))
            {
                writer.WriteLine("========================================");
                writer.WriteLine("           NOVAMARKET - UPDS            ");
                writer.WriteLine("========================================");
                writer.WriteLine($"Fecha: {DateTime.Now}");
                writer.WriteLine($"Cliente: {nombreCliente}");
                writer.WriteLine("----------------------------------------");
                writer.WriteLine("Cant.   Producto                Subtotal");
                
                foreach (var item in items)
                {
                    writer.WriteLine($"{item.Cantidad,-7} {item.Nombre,-23} {item.PrecioVenta,8:N2}");
                }

                writer.WriteLine("----------------------------------------");
                writer.WriteLine($"TOTAL A PAGAR:           Bs. {total,8:N2}");
                writer.WriteLine("========================================");
                writer.WriteLine("    ¡Gracias por su compra en Nova!     ");
                writer.WriteLine("========================================");
            }

            // Abrir la factura automáticamente para mostrarla al docente
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(ruta) { UseShellExecute = true });
        }
    }
}