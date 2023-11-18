using System;
using System.IO;
using Vaxi;
using System.Threading;
using Grpc.Core;
using System.Linq;

namespace Client
{

    class Program
    {
        const string serverPoint = "127.0.0.1:50008";

        static async Task Main(string[] args)
        {
            //abro canal
            Grpc.Core.Channel canal = new Grpc.Core.Channel(serverPoint, Grpc.Core.ChannelCredentials.Insecure);

            await canal.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("el cliente se conecto al server GRPC");
                }
            });

            var persona = new Persona()
            {
                Nombre= "jarriagada",
                Apellido= "Arr",
                Email= "jlas1680@gmail.com"
            };

            var client = new PersonaService.PersonaServiceClient(canal);
            //1. crear nuevo request
            var request = new ClientMultiplePersonaRequest() 
            {
                
                Persona = persona
            };

            var stream = client.RegistrarPersonaClientMultiple();

            foreach (int i in Enumerable.Range(1, 10)) 
            {
                await stream.RequestStream.WriteAsync(request);
            }

            await stream.RequestStream.CompleteAsync();
            //flag que indica termino del proceso, respuesta
            var response = await stream.ResponseAsync;

            //imprimir la respuesta
            Console.WriteLine($"Response: {response.Resultado}");

            canal.ShutdownAsync().Wait();
            Console.ReadKey();

        }
    }
}