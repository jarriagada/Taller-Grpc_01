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
            Grpc.Core.Channel canal = new Grpc.Core.Channel(serverPoint, Grpc.Core.ChannelCredentials.Insecure);

            canal.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("el cliente se conecto al server GRPC");
                }
            });

            var client = new Operaciones.OperacionesClient(canal);

        }
    }
}