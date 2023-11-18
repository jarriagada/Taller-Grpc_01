using System;
using System.IO;
using Vaxi;

namespace Client
{

    class Program
    {
        const string serverPoint = "127.0.0.1:50008";

        static void Main(string[] args)
        {
            //abro canal
            Grpc.Core.Channel canal = new Grpc.Core.Channel(serverPoint, Grpc.Core.ChannelCredentials.Insecure);

            canal.ConnectAsync().ContinueWith(task =>
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

            var request = new PersonaRequest()
            {
                Persona= persona,
            };

            //declaracion del nuevo cliente
            var client = new PersonaService.PersonaServiceClient(canal);
            //utilizando client, utilizo el metodo RegistrarPersona y le paso el metodo request
            //que retorna el response al servidor
            var response = client.RegistrarPersona(request);
            Console.WriteLine($"Response: {response.Resultado}"); 


            canal.ShutdownAsync().Wait();
            Console.ReadKey();

        }
    }
}