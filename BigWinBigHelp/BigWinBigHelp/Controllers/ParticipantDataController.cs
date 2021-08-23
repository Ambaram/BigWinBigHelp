using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BigWinBigHelp.Models;

namespace BigWinBigHelp.Controllers
{
    public class ParticipantDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ParticipantData/ListParticipants
        [HttpGet]
        public IHttpActionResult ListParticipants()
        {
            List<Participant> participants = db.Participants.ToList();
            List<participantdto> participantdtos = new List<participantdto>();
            participants.ForEach(p => participantdtos.Add(new participantdto()
            {
                id = p.id,
                name = p.name
            }));
            return Ok(participantdtos);
        }

        // GET: api/ParticipantData/FindParticipant/5
        [HttpGet]
        [ResponseType(typeof(Participant))]
        public IHttpActionResult FindParticipant(int id)
        {
            Participant participant = db.Participants.Find(id);
            participantdto participantdto = new participantdto()
            {
                id = participant.id,
                name = participant.name
            };
            if (participant == null)
            {
                return NotFound();
            }

            return Ok(participantdto);
        }

        // Post: api/ParticipantData/UpdateParticipant/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateParticipant(int id, Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participant.id)
            {
                return BadRequest();
            }

            db.Entry(participant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ParticipantData/AddParticipant
        [HttpPost]
        [ResponseType(typeof(Participant))]
        public IHttpActionResult PostParticipant(Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Participants.Add(participant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = participant.id }, participant);
        }

        // DELETE: api/ParticipantData/DeleteParticipant/5
        [ResponseType(typeof(Participant))]
        public IHttpActionResult DeleteParticipant(int id)
        {
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return NotFound();
            }

            db.Participants.Remove(participant);
            db.SaveChanges();

            return Ok(participant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParticipantExists(int id)
        {
            return db.Participants.Count(e => e.id == id) > 0;
        }
    }
}