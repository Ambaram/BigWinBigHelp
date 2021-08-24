using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using BigWinBigHelp.Models;

namespace BigWinBigHelp.Controllers
{
    public class TicketController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        // HTTP Cleint Handler to listen to HTTP request 
        static TicketController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }
        /// <summary>
        /// Fetch a list of tickets available in the database
        /// </summary>
        /// <example>GET : Ticket/ListTickets</example>
        /// <returns>an IEnumerated object</returns>
        [HttpGet]
        public ActionResult ListTickets()
        {
            string url = "TicketData/ListTickets";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TicketDto> tickets = response.Content.ReadAsAsync<IEnumerable<TicketDto>>().Result;
            return View(tickets);
        }
        /// <summary>
        /// An error page will be displayed if any issue happens
        /// </summary>
        /// <returns>Error View</returns>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
       
        /// <summary>
        /// A new ticket method to add new ticket into the database
        /// </summary>
        /// <example>GET : Ticket/NewTicket</example>
        /// <returns>New Ticket page</returns>
        public ActionResult NewTicket()
        {
            return View();
        }
        /// <summary>
        /// Adds a new ticket to the database
        /// </summary>
        /// <param name="ticket"></param>
        /// <example>POST : Ticket/AddTicket</example>
        [HttpPost]
        [Authorize]
        public ActionResult AddTicket(Ticket ticket)
        {
            string url = "TicketData/AddTicket";
            string jsonpayload = jss.Serialize(ticket);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListTickets");
            }
            else 
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays a view to edit the data of selected ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <example>GET: Ticket/EditTicket/5</example>
        public ActionResult EditTicket(int id)
        {
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TicketDto selectedticket = response.Content.ReadAsAsync<TicketDto>().Result;
            return View(selectedticket);
        }
        /// <summary>
        /// Updates the ticket data for the selected data with the new entries passed in the ticket object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <example> POST: Ticket/UpdateTicket/5 </example>
        [HttpPost]
        [Authorize]
        public ActionResult UpdateTicket(int id, Ticket ticket)
        {
            string url = "TicketData/UpdateTicket/" + id;
            string jsonpayload = jss.Serialize(ticket);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListTickets");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Displays a view to delete the selected ticket data
        /// </summary>
        /// <param name="id"></param>
        /// <example>GET: Ticket/Delete/5</example>
        [HttpGet]
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TicketDto selectedticket = response.Content.ReadAsAsync<TicketDto>().Result;
            return View(selectedticket);
        }
        /// <summary>
        /// Deletes the ticket data of the selected ticket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <example>POST: Ticket/DeleteTicket/5</example>
        [HttpPost]
        public ActionResult DeleteTicket(int id, Ticket ticket)
        {
            string url = "TicketData/DeleteTicket/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListTickets");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
