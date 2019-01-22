using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace API
{
    public class User
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string position { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public int permission { get; set; }
        public Status status { get; set; }
        public string newemail { get; set; }

        public User() { }
        public User(int id, string firstname, string lastname, string email, string position, DateTime created, DateTime updated, int permission, Status status, string newemail)
        {
            this.id = id;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.position = position;
            this.created = created;
            this.updated = updated;
            this.permission = permission;
            this.status = status;
            this.newemail = newemail;
        }

        public User(string id, string firstname, string lastname, string email, string position, string created, string updated, int permission, string status, string newemail)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.position = position;
            this.permission = permission;
            this.newemail = newemail;

            int tmpInt;
            if (Int32.TryParse(id, out tmpInt))
                this.id = tmpInt;

            DateTime tmpDate;
            if (DateTime.TryParse(created, out tmpDate))
                this.created = tmpDate;

            if (DateTime.TryParse(updated, out tmpDate))
                this.updated = tmpDate;

            this.status = new Status(status, null, null);      
        }

        public string getToken(string password)
        {
            var client = new RestClient(Core.siteMap.userDir[SiteMap.userMethod.token]);

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.AddParameter("email", this.email);
            request.AddParameter("password", password);

            var response = client.Execute(request);

            return response.Content;
        }
    }
}
