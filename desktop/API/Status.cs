using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Status
    {
        public string status { get; set; }
        public DateTime updated { get; set; }
        public int user_id { get; set; }

        public Status() { }

        public Status(string status, DateTime updated, int user_id)
        {
            this.status = status;
            this.updated = updated;
            this.user_id = user_id;
        }

        public Status(string status, string updated, string user_id)
        {
            this.status = status;

            DateTime tmpDate;
            if (DateTime.TryParse(updated, out tmpDate))
                this.updated = tmpDate;

            int tmpInt;
            if (Int32.TryParse(user_id, out tmpInt))
                this.user_id = tmpInt;
        }

        public Status get(string token)
        {
            return null;
        }
    }
}
