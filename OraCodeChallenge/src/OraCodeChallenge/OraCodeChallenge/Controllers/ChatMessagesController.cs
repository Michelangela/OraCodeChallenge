using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using OraCodeChallenge.Models;
using OraCodeChallenge.Models.Entities;

namespace OraCodeChallenge.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using OraCodeChallenge.Models.Entities;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ChatMessage>("ChatMessages");
    builder.EntitySet<Chat>("Chats"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ChatMessagesController : ODataController
    {
        private OraContext db = new OraContext();

        // GET: /ChatMessages
        [EnableQuery]
        public IQueryable<ChatMessage> GetChatMessages()
        {
            return db.ChatMessages;
        }

        // GET: /ChatMessages(5)
        [EnableQuery]
        public SingleResult<ChatMessage> GetChatMessage([FromODataUri] int key)
        {
            return SingleResult.Create(db.ChatMessages.Where(chatMessage => chatMessage.ChatMessageId == key));
        }

        // PUT: /ChatMessages(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ChatMessage> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChatMessage chatMessage = db.ChatMessages.Find(key);
            if (chatMessage == null)
            {
                return NotFound();
            }

            patch.Put(chatMessage);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(chatMessage);
        }

        // POST: /ChatMessages
        public IHttpActionResult Post(ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ChatMessages.Add(chatMessage);
            db.SaveChanges();

            return Created(chatMessage);
        }

        // PATCH: /ChatMessages(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ChatMessage> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChatMessage chatMessage = db.ChatMessages.Find(key);
            if (chatMessage == null)
            {
                return NotFound();
            }

            patch.Patch(chatMessage);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(chatMessage);
        }

        // DELETE: /ChatMessages(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ChatMessage chatMessage = db.ChatMessages.Find(key);
            if (chatMessage == null)
            {
                return NotFound();
            }

            db.ChatMessages.Remove(chatMessage);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: /ChatMessages(5)/Chat
        [EnableQuery]
        public SingleResult<Chat> GetChat([FromODataUri] int key)
        {
            return SingleResult.Create(db.ChatMessages.Where(m => m.ChatMessageId == key).Select(m => m.Chat));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChatMessageExists(int key)
        {
            return db.ChatMessages.Count(e => e.ChatMessageId == key) > 0;
        }
    }
}
