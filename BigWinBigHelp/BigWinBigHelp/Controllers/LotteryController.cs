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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }
        // GET: Lottery/ListLotteries
        public ActionResult ListLotteries()
        {
            string url = "LotteryData/ListLotteries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<LotteryDto> lotteries = response.Content.ReadAsAsync<IEnumerable<LotteryDto>>().Result; 
            return View(lotteries);
        }

        // GET: Lottery/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Lottery/NewLottery
        public ActionResult NewLottery()
        {
            return View();
        }

        // POST: Lottery/AddLottery
        [HttpPost]
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

        // GET: Lottery/EditLottery/5
        public ActionResult EditLottery(int id)
        {
            string url = "LotteryData/FindLottery/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LotteryDto selectedlottery = response.Content.ReadAsAsync<LotteryDto>().Result;
            return View(selectedlottery);
        }

        // POST: Lottery/Edit/5
        [HttpPost]
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

        // GET: Lottery/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "LotteryData/FindLottery/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LotteryDto selectedlottery = response.Content.ReadAsAsync<LotteryDto>().Result;
            return View(selectedlottery);
        }

        // POST: Lottery/DeleteLottery/5
        [HttpPost]
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
