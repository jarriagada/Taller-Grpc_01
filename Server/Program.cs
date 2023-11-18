using System;
using System.IO;

namespace Server
{
    class Program
    {
        const int Port = 50008;

        static void Main(string[] args)
        {
            Grpc.Core.Server server = null;  // Declarar la variable fuera del bloque try
            try
            {
                server = new Grpc.Core.Server()
                {
                    Ports = { new Grpc.Core.ServerPort("127.0.0.1", Port, Grpc.Core.ServerCredentials.Insecure) }
                };
                server.Start();
                Console.WriteLine("El servidor se está ejecutando en el puerto " + Port);
                Console.ReadKey();
            }
            catch (IOException e)
            {
                // Manejo de la excepción IOException
                Console.WriteLine("Error de E/S: " + e.Message);
            }
            finally
            {
                // Acciones que siempre se ejecutarán
                Console.WriteLine("Cerrando el servidor...");
                if (server != null)
                {
                    // Comprueba si el servidor se ha iniciado correctamente antes de intentar detenerlo
                    server.ShutdownAsync().Wait();
                }
                Console.WriteLine("El servidor se ha cerrado correctamente");
            }
        }
    }
}