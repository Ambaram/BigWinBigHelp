using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using BigWinBigHelp.Models;

namespace BigWinBigHelp.Controllers
{
    public class ParticipantController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ParticipantController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }
        // GET: Participant/ListParticipants
        [HttpGet]
        public ActionResult ListParticipant()
        {
            string url = "ParticipantData/ListParticipants";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<participantdto> participants = response.Content.ReadAsAsync<IEnumerable<participantdto>>().Result;
            return View(participants);
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        // GET: Participant/Create
        [HttpGet]
        public ActionResult NewParticipant()
        {
            return View();
        }

        // POST: Participant/Create
        [HttpPost]
        public ActionResult AddParticipant(Participant participant)
        {
            string url = "ParticipantData/AddParticipant";
            string jsonpayload = jss.Serialize(participant);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListParticipant");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Participant/EditParticipant/5
        [HttpGet]
        public ActionResult EditParticipant(int id)
        {
            string url = "ParticipantData/FindParticipant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            participantdto selectedparticipant = response.Content.ReadAsAsync<participantdto>().Result;
            return View(selectedparticipant);
        }

        // POST: Participant/UpdateParticipant/5
        [HttpPost]
        public ActionResult UpdateParticipant(int id, Participant participant)
        {
            string url = "ParticipantData/UpdateParticipant/" + id;
            string jsonpayload = jss.Serialize(participant);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListParticipants");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Participant/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ParticipantData/FindParticipant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            participantdto selectedparticipant = response.Content.ReadAsAsync<participantdto>().Result;
            return View(selectedparticipant);
        }

        // POST: Participant/DeleteParticipant/5
        [HttpPost]
        public ActionResult DeleteParticipant(int id, Participant participant)
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
