using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BigWinBigHelp.Models
{
    public class Ticket
    {
        public int id { get; set; }
        public string ticket_name { get; set; }
        public string ticket_price { get; set; }
        public string multiplier { get; set; }
    }

    public class TicketDto
    {
        public int id { get; set; }
        public string ticket_name { get; set; }
        public string ticket_price { get; set; }
        public string multiplier { get; set; }
    }
}