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
    public class LotteryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ///<summary>Method to list lotteries in the database</summary>
        ///<returns>
        ///Status Code 200 : Request successful
        ///Status Code 402 : Not Found
        ///</returns>
        ///<example>GET: api/LotteryData/ListLotteries</example>
        [HttpGet]
        public IHttpActionResult ListLotteries()
        {
            List<Lottery> lotteries = db.Lotteries.ToList();
            List<LotteryDto> lotteryDtos = new List<LotteryDto>();
            lotteries.ForEach(l => lotteryDtos.Add(new LotteryDto()
            {
                id = l.id,
                name=l.name,
                startdate = l.startdate,
                enddate = l.enddate,
                total_prize = l.total_prize
            }));
            return Ok(lotteryDtos);
        }
        /// <summary>
        /// Fetch the data of selected lottery from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Status Code 200 : OK: Request Successful
        /// Staus Code 404: Not Found
        /// </returns>
        /// <example>GET: api/LotteryData/FindLottery/5</example>
        [ResponseType(typeof(Lottery))]
        [HttpGet]
        public IHttpActionResult FindLottery(int id)
        {
            Lottery lottery = db.Lotteries.Find(id);
            LotteryDto lotteryDto = new LotteryDto()
            {
                id = lottery.id,
                name = lottery.name,
                startdate = lottery.startdate,
                enddate = lottery.enddate,
                total_prize = lottery.total_prize
            };
            if (lottery == null)
            {
                return NotFound();
            }

            return Ok(lotteryDto);
        }
        /// <summary>
        /// Updates the lottery data for the selected lottery
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lottery"></param>
        /// <returns>
        /// 200 OK : Suucess
        /// 404 NOT FOUND
        /// 402: Forbidden
        /// </returns>
        // POST: api/LotteryData/UpdateLottery/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateLottery(int id, Lottery lottery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lottery.id)
            {
                return BadRequest();
            }

            db.Entry(lottery).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LotteryExists(id))
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
        /// Add new lottery to the database
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns>
        /// 200 OK : Successful
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        // POST: api/LotteryData/AddLottery
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(Lottery))]
        public IHttpActionResult AddLottery(Lottery lottery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lotteries.Add(lottery);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lottery.id }, lottery);
        }
        /// <summary>
        /// Delete the selected lottery data from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 200 OK : Successful
        /// 404 NOT FOUND
        /// 402 Forbidden
        /// </returns>
        /// <example>DELETE: api/Lotteries/DeleteLottery/5 </example>
        [ResponseType(typeof(Lottery))]
        public IHttpActionResult DeleteLottery(int id)
        {
            Lottery lottery = db.Lotteries.Find(id);
            if (lottery == null)
            {
                return NotFound();
            }

            db.Lotteries.Remove(lottery);
            db.SaveChanges();

            return Ok(lottery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LotteryExists(int id)
        {
            return db.Lotteries.Count(e => e.id == id) > 0;
        }
    }
}