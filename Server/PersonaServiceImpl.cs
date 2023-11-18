using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaxi;
using static Vaxi.PersonaService;

namespace Client
{
    public class PersonaServiceImpl: PersonaServiceBase
    {

        //Registrar metodos
        public override Task<PersonaResponse> RegistrarPersona(PersonaRequest request, ServerCallContext context)
        {
            string mensaje = "se inserto correctamente el usuario: " + request.Persona.Nombre + " - " + request.Persona.Email;

            PersonaResponse response = new PersonaResponse()
            {
                Resultado = mensaje
            };
                
            return Task.FromResult(response);
        }


        public override async Task RegistrarPersonasServidorMultiple(ServerMultiplePersonaRequest request, IServerStreamWriter<ServerMultiplePersonaResponse> responseStream, ServerCallContext context)
        {

            Console.WriteLine("el servidor recibio el request del cliente" + request.ToString());

            //tomar el mensaje y envolverlo en un objeto de tipo response, para devolverlo al cliente
            string mensaje = "se inserto correctamente el usuario: " + request.Persona.Nombre + " - " + request.Persona.Email;

            foreach (int i in Enumerable.Range(1,10))  
            {
                ServerMultiplePersonaResponse response = new ServerMultiplePersonaResponse() 
                { 
                    Resultado = mensaje 
                };

                await responseStream.WriteAsync(response);

            }

        }


    }
}


