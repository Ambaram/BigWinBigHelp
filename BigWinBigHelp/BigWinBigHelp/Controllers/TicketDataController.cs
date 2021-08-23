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
    public class TicketDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TicketData/ListTickets
        public IHttpActionResult ListTickets()
        {
            List<Ticket> Tickets = db.Tickets.ToList();
            List<TicketDto> ticketDtos = new List<TicketDto>();
            Tickets.ForEach(t => ticketDtos.Add(new TicketDto()
            {
                id = t.id,
                ticket_name = t.ticket_name,
                ticket_price = t.ticket_price,
                multiplier = t.multiplier
            }));
            return Ok(ticketDtos) ;
        }
        // GET: api/TicketData/5
        [HttpGet]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult FindTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            TicketDto ticketDto = new TicketDto()
            {
                id = ticket.id,
                ticket_name = ticket.ticket_name,
                ticket_price = ticket.ticket_price,
                multiplier = ticket.multiplier
            };
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticketDto);
        }

        // POST: api/TicketData/UpdateTicket/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateTicket(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.id)
            {
                return BadRequest();
            }

            db.Entry(ticket).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/TicketData/AddTicket
        [HttpPost]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult AddTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tickets.Add(ticket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ticket.id }, ticket);
        }

        // DELETE: api/TicketData/DeleteTicket/5
        [HttpPost]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult DeleteTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            db.Tickets.Remove(ticket);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.id == id) > 0;
        }
    }
}