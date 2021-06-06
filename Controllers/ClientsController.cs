using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LanguageWebAPI.Models;

namespace LanguageWebAPI.Controllers
{
    public class ClientsController : ApiController
    {
        Context context = new Context();

        [Route("api/Client/GetClientList")]
        public List<ClientsList> GetClientList()
        {
            return context.ClientsList.ToList();
        }

        [Route("api/Client/GetGendersNames")]
        public List<string> GetGendersNames()
        {
            return context.Gender
                .Select(r => r.Name)
                .ToList();
        }

        [Route("api/Client/{clientID:int}")]
        [HttpGet]
        public Client GetClient(int clientID)
        {
            return context.Client
                .FirstOrDefault(r => r.ID == clientID);
        }

        [Route("api/Client/GetVisits/{clientID:int}")]
        [HttpGet]
        public List<ClientService> GetClientVisits(int clientID)
        {
            return context.ClientService
                .Where(r => r.IDClient == clientID)
                .ToList();
        }

        [Route("api/Client/GetTags/{clientID:int}")]
        [HttpGet]
        public List<Tag> GetClientTags(int clientID)
        {
            var idTags = context.TagClient
                .Where(r => r.IDClient == clientID)
                .Select(r => r.IDTag)
                .ToHashSet();

            return context.Tag
                .Where(r => idTags.Contains(r.ID))
                .ToList();
        }

        [Route("api/Client/Add")]
        [HttpPost]
        public IHttpActionResult AddClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Client.Add(client);
            context.SaveChanges();

            return Ok(client);
        }

        [Route("api/Client/Update/{clientID:int}")]
        public IHttpActionResult UpdateClient(int clientID, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (clientID != client.ID)
            {
                return BadRequest();
            }

            context.Entry(client).State = System.Data.Entity.EntityState.Modified;

            context.SaveChanges();

            return Ok(client);
        }

        [Route("api/Client/Delete/{clientID:int}")]
        public IHttpActionResult DeleteClient(int clientID)
        {
            var client = context.Client
                .FirstOrDefault(r => r.ID == clientID);

            if(client is null)
            {
                return NotFound();
            }

            context.Client.Remove(client);
            context.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}