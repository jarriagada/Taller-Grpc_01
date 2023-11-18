using System;
using System.IO;
using Vaxi;
using System.Threading;
using Grpc.Core;

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

            //var request = new PersonaRequest()
            //{
            //    Persona= persona,
            //};


            var request = new ServerMultiplePersonaRequest()
            {
                Persona = persona,
            };

            //declaracion del nuevo cliente
            var client = new PersonaService.PersonaServiceClient(canal);
            //utilizando client, utilizo el metodo RegistrarPersona y le paso el metodo request
            //que retorna el response al servidor

            //var response = client.RegistrarPersona(request);
            var response = client.RegistrarPersonasServidorMultiple(request);
            //Console.WriteLine($"Response: {response.ResponseStream}"); 

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine($"{response.ResponseStream.Current.Resultado}");
                await Task.Delay(1000);
            }

            canal.ShutdownAsync().Wait();
            Console.ReadKey();

        }
    }
}