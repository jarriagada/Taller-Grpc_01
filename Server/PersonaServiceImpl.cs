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
        public override Task<PersonaResponse> RegistrarPersona(PersonaRequest request, ServerCallContext context)
        {
            string mensaje = "se inserto correctamente el usuario: " + request.Persona.Nombre + " - " + request.Persona.Email;

            PersonaResponse response = new PersonaResponse()
            {
                Resultado = mensaje
            };
                
            return Task.FromResult(response);
        }

    }
}


