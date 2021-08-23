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
        // GET: Ticket/ListTickets
        public ActionResult ListTickets()
        {
            string url = "TicketData/ListTickets";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TicketDto> tickets = response.Content.ReadAsAsync<IEnumerable<TicketDto>>().Result;
            return View(tickets);
        }
        // GET: Ticket/Error
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
       
        // GET: Ticket/NewTicket
        public ActionResult NewTicket()
        {
            return View();
        }

        // POST: Ticket/AddTicket
        [HttpPost]
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

        // GET: Ticket/EditTicket/5
        public ActionResult EditTicket(int id)
        {
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TicketDto selectedticket = response.Content.ReadAsAsync<TicketDto>().Result;
            return View(selectedticket);
        }

        // POST: Ticket/UpdateTicket/5
        [HttpPost]
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

        // GET: Ticket/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TicketDto selectedticket = response.Content.ReadAsAsync<TicketDto>().Result;
            return View(selectedticket);
        }

        // POST: Ticket/DeleteTicket/5
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
