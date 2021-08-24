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
            // HTTP Client handler to listen to HTTP request
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }
        /// <summary>
        /// List the participants available in the database
        /// </summary>
        /// <returns>IEnuerable object containig the participant data</returns>
        /// <example>GET: Participant/ListParticipants </example>
        [HttpGet]
        public ActionResult ListParticipant()
        {
            string url = "ParticipantData/ListParticipants";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<participantdto> participants = response.Content.ReadAsAsync<IEnumerable<participantdto>>().Result;
            return View(participants);
        }

        /// <summary>
        /// Error view for participant entity
        /// </summary>
        /// <returns>Error View</returns>
        /// <example>Participant/Error</example>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Show a view to add new participant to the database
        /// </summary>
        /// <returns>New Participant view</returns>
        // GET: Participant/NewParticipant
        [HttpGet]
        public ActionResult NewParticipant()
        {
            return View();
        }

        /// <summary>
        /// Add new participant to the database
        /// </summary>
        /// <param name="participant"></param>
        // POST: Participant/AddParticipant
        [HttpPost]
        [Authorize]
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
        /// <summary>
        /// Edit the selected participant data into the databse
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Edit Participant view</returns>
        /// <example>GET: Participant/EditParticipant/5 </example>
        [HttpGet]
        public ActionResult EditParticipant(int id)
        {
            string url = "ParticipantData/FindParticipant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            participantdto selectedparticipant = response.Content.ReadAsAsync<participantdto>().Result;
            return View(selectedparticipant);
        }
        /// <summary>
        /// Update the selected participant data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="participant"></param>
        /// POST: Participant/UpdateParticipant/5
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// View to ask user to delete selected participant
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete Confirm View</returns>
        /// <example>GET: Participant/DeleteConfirm/5 </example>
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ParticipantData/FindParticipant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            participantdto selectedparticipant = response.Content.ReadAsAsync<participantdto>().Result;
            return View(selectedparticipant);
        }
        /// <summary>
        /// Deletes the selected participant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="participant"></param>
        /// <example>POST: Participant/DeleteParticipant/5 </example>
        [HttpPost]
        [Authorize]
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
