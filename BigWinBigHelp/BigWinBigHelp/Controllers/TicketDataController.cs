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
        /// <summary>
        /// Fetches the list of availabe tickets from the database
        /// </summary>
        /// <returns>
        /// 200 OK : Success
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>GET: api/TicketData/ListTickets </example>
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

        /// <summary>
        /// Finds the data of selected ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 200 OK : Successful
        /// 404 NOT Found
        /// 402 Forbidden
        /// </returns>
        /// <example>GET: api/TicketData/FindTicket/5 </example>
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

        /// <summary>
        /// Updates the ticket data of the selected ticket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <returns>
        /// 200 OK : Success
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>POST: api/TicketData/UpdateTicket/5 </example>
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Add new ticket to the database
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns>
        /// 200 OK
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>POST: api/TicketData/AddTicket </example>
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Deletes a ticket data  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 200 OK
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>DELETE: api/TicketData/DeleteTicket/5 </example>
        [HttpPost]
        [Authorize]
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