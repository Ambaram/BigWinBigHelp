using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BigWinBigHelp.Models
{
    public class Lottery
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string total_prize { get; set; }
    }
    public class LotteryDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string total_prize { get; set; }
    }
}