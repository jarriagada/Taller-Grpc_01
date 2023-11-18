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

            var client = new PersonaService.PersonaServiceClient(canal);

            //instancio Persona, creo un array de personas y les seteo el Email
            Persona[] PersonaCollection =
            {

                new Persona() {Email = "jlas1680@gmail_01.com"},
                new Persona() {Email = "jlas1680@gmail_02.com"},
                new Persona() {Email = "jlas1680@gmail_03.com"},
                new Persona() {Email = "jlas1680@gmail_04.com"},
                new Persona() {Email = "jlas1680@gmail_05.com"},
                new Persona() {Email = "jlas1680@gmail_06.com"},
                new Persona() {Email = "jlas1680@gmail_07.com"},
                new Persona() {Email = "jlas1680@gmail_08.com"},
                new Persona() {Email = "jlas1680@gmail_09.com"},
                new Persona() {Email = "jlas1680@gmail_10.com"}

            };

            //instancio la interfaz
            var stream = client.RegistrarPersonaBidireccional();

            //se necesita un objeto para enviar las personas
            foreach (var persona in PersonaCollection)
            {
                //Console.WriteLine("enviando al servidor : " + persona.Email);
                //await stream.RequestStream.WriteAsync(new BidireccionalPersonaRequest
                //{
                //    Persona = persona
                //});
                var request = new BidireccionalPersonaRequest()
                {
                    Persona = persona

                };

                await stream.RequestStream.WriteAsync(request);
            }

            await stream.RequestStream.CompleteAsync();

            var responseCollection = Task.Run(async () =>
            {

                while (await stream.ResponseStream.MoveNext()) 
                {
                    Console.WriteLine("el cliente esta recibiendo del servidor : {0} {1}", stream.ResponseStream.Current.Resultado, Environment.NewLine);
                }

            });
           
            await responseCollection;

            canal.ShutdownAsync().Wait();
            Console.ReadKey();

        }
    }
}