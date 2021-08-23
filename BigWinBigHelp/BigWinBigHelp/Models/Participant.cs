using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BigWinBigHelp.Models
{
    public class Participant
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }

    public class participantdto
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}