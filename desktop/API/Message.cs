using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Message
    {
        public string message { get; set; }
        public string user_from { get; set; }
        public string user_to { get; set; }
        public DateTime sended { get; set; }

        public Message() { }
        public Message(string message, string user_from, string user_to, DateTime sended)
        {
            this.message = message;
            this.user_from = user_from;
            this.user_to = user_to;
            this.sended = sended;
        }
        public Message(string message, string user_from, string user_to, string sended)
        {
            this.message = message;
            this.user_from = user_from;
            this.user_to = user_to;

            DateTime tmpDate;
            if (DateTime.TryParse(sended, out tmpDate))
                this.sended = tmpDate;
        }
    }
}
