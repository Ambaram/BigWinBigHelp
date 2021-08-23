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

        // GET: api/LotteryData/ListLotteries
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

        // GET: api/LotteryData/FindLottery/5
        [ResponseType(typeof(Lottery))]
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

        // POST: api/LotteryData/UpdateLottery/5
        [ResponseType(typeof(void))]
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

        // POST: api/LotteryData/AddLottery
        [HttpPost]
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

        // DELETE: api/Lotteries/DeleteLottery/5
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