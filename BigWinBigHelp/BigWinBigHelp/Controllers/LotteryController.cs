using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigWinBigHelp.Models;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace BigWinBigHelp.Controllers
{
    public class LotteryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static LotteryController()
        {
            // client handler to listen to http request to commnunicate with the api
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }
        /// <summary>
        /// Returns a list of lotteries available in the system
        /// </summary>
        /// <returns>IEnumerable object containing the lotteries in  the system</returns>
        /// <example>GET: Lottery/ListLotteries</example>
        [HttpGet]
        public ActionResult ListLotteries()
        {
            string url = "LotteryData/ListLotteries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<LotteryDto> lotteries = response.Content.ReadAsAsync<IEnumerable<LotteryDto>>().Result; 
            return View(lotteries);
        }
        /// <summary>
        /// Error view for lottery entity
        /// </summary>
        /// <example>GET: Lottery/Error </example>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// Displays a view to add a new lottery to the system
        /// </summary>
        /// <returns>New Lottery View</returns>
        /// <example>GET: Lottery/NewLottery </example>
        [HttpGet]
        [Authorize]
        public ActionResult NewLottery()
        {
            return View();
        }

        /// <summary>
        /// Adds new lottery to the system
        /// </summary>
        /// <param name="lottery"></param>
        // POST: Lottery/AddLottery
        [HttpPost]
        [Authorize]
        public ActionResult AddLottery(Lottery lottery)
        {
            string url = "TicketData/AddLottery";
            string jsonpayload = jss.Serialize(lottery);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListLotteries");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Displays view to edit the selected lottery
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EditLottery View</returns>
        /// <example>GET: Lottery/EditLottery/5 </example>
        [HttpGet]
        [Authorize]
        public ActionResult EditLottery(int id)
        {
            string url = "LotteryData/FindLottery/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LotteryDto selectedlottery = response.Content.ReadAsAsync<LotteryDto>().Result;
            return View(selectedlottery);
        }

        /// <summary>
        /// Updates the selected lottery information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lottery"></param>
        /// <returns></returns>
        /// <example>POST: Lottery/Edit/5 </example>
        [HttpPost]
        [Authorize]
        public ActionResult UpdateLottery(int id, Lottery lottery)
        {
            string url = "LotteryData/UpdateLottery/" + id;
            string jsonpayload = jss.Serialize(lottery);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListLotteries");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays view to confirm if the selected lottery to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <example>GET: Lottery/DeleteConfirm/5</example>
        [HttpGet]
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "LotteryData/FindLottery/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LotteryDto selectedlottery = response.Content.ReadAsAsync<LotteryDto>().Result;
            return View(selectedlottery);
        }

        /// <summary>
        /// Deletes the selected lottery from the system
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lottery"></param>
        /// <example>POST: Lottery/DeleteLottery/5</example>
        [HttpPost]
        [Authorize]
        public ActionResult DeleteLottery(int id, Lottery lottery)
        {
            string url = "LotteryData/DeleteLottery/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListLotteries");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
