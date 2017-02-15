using OraCodeChallenge.Models;
using OraCodeChallenge.Models.Entities;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
using System;
using System.Collections.Generic;

namespace OraCodeChallenge.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using OraCodeChallenge.Models.Entities;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Chat>("Chats");
    builder.EntitySet<ChatMessage>("ChatMessages"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ChatsController : System.Web.OData.ODataController
    {
        private OraContext db = new OraContext();

        // GET: /Chats
        [System.Web.OData.EnableQuery]
        [Queryable]
        public IQueryable<Chat> GetChats()
        {
            return db.Chats;
        }

        // GET: /Chats(5)
        [System.Web.OData.EnableQuery]
        public SingleResult<Chat> GetChat([System.Web.OData.FromODataUri] int key)
        {
            return SingleResult.Create(db.Chats.Where(chat => chat.ChatId == key));
        }

        // PUT: /Chats(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Chat> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Chat chat = db.Chats.Find(key);
            if (chat == null)
            {
                return NotFound();
            }

            patch.Put(chat);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(chat);
        }

        // POST: /Chats
        public IHttpActionResult Post(Chat chat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Chats.Add(chat);
            db.SaveChanges();

            return Created(chat);
        }

        // PATCH: /Chats(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([System.Web.OData.FromODataUri] int key, System.Web.OData.Delta<Chat> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Chat chat = db.Chats.Find(key);
            if (chat == null)
            {
                return NotFound();
            }

            patch.Patch(chat);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(chat);
        }

        private void Validate(object p)
        {
            throw new NotImplementedException();
        }

        // DELETE: /Chats(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Chat chat = db.Chats.Find(key);
            if (chat == null)
            {
                return NotFound();
            }

            db.Chats.Remove(chat);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: /Chats(5)/ChatMessages
        [Queryable]
        [System.Web.OData.EnableQuery]
        public IQueryable<ChatMessage> GetChatMessages([System.Web.OData.FromODataUri] int key)
        {
            return db.Chats.Where(m => m.ChatId == key).SelectMany(m => m.ChatMessages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChatExists(int key)
        {
            return db.Chats.Count(e => e.ChatId == key) > 0;
        }

    }
}