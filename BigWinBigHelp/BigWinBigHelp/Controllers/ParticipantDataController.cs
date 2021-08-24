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
        /// <summary>
        /// List the participants present in the database
        /// </summary>
        /// <returns>
        /// 200 OK
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>GET: api/ParticipantData/ListParticipants</example>
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

        /// <summary>
        /// Fetches specific participant from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 200 OK
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>GET: api/ParticipantData/FindParticipant/5 </example>
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

        /// <summary>
        /// Updates the data of selected participant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="participant"></param>
        /// <returns>
        /// 200 OK
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>Post: api/ParticipantData/UpdateParticipant/5 </example>
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Add new participant to the database
        /// </summary>
        /// <param name="participant"></param>
        /// <returns>
        /// 200 OK
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>POST: api/ParticipantData/AddParticipant </example>
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Delete specific participant from  the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 200 OK
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>DELETE: api/ParticipantData/DeleteParticipant/5 </example>
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