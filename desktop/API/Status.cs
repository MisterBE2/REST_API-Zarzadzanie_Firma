using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;

namespace API
{
    public class Status
    {
        public string StatusContent { get; set; }
        public DateTime Updated { get; set; }
        public int User_id { get; set; }

        public Status() { }
        public Status(string status, DateTime updated, int user_id)
        {
            StatusContent = status;
            Updated = updated;
            User_id = user_id;
        }
        public Status(string status, string updated, string user_id)
        {
            StatusContent = status;

            if (DateTime.TryParse(updated, out DateTime tmpDate))
                Updated = tmpDate;

            if (Int32.TryParse(user_id, out int tmpInt))
                User_id = tmpInt;
        }

        #region helpers
        #endregion
        #region API calls
        public void Set(string token)
        {
            var client = new RestClient(Core.siteMap.statusDir[SiteMap.StatusMethod.set]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            request.AddParameter("status", StatusContent);
            request.AddParameter("body", token);

            client.ExecuteAsync(request, response => {
                StatusEventsArgs args = new StatusEventsArgs
                {
                    ResponseCode = response.StatusCode,
                    Status = this
                };

                StandardEventArgs standArgs = StandardEventArgsDeserialiser(response);
                args.Body = standArgs.Body;
                args.Message = standArgs.Message;

                OnStatusSet(args);
            });
        }
        public void Get(string email, string token)
        {
            var client = new RestClient(Core.siteMap.statusDir[SiteMap.StatusMethod.get]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            request.AddParameter("body", token);
            request.AddParameter("email", email);


            client.ExecuteAsync(request, response =>
            {
                StatusEventsArgs args = new StatusEventsArgs();
                args.ResponseCode = response.StatusCode;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject o = JObject.Parse(response.Content);
                    args.Message = (string)o["message"];
                    args.Status = new Status((string)o["body"]["status"], (string)o["body"]["updated"], (string)o["body"]["user_id"]);
                    StatusContent = args.Status.StatusContent;
                    User_id = args.Status.User_id;
                    Updated = args.Status.Updated;
                }
                else
                {
                    StandardEventArgs standArgs = StandardEventArgsDeserialiser(response);
                    args.Body = standArgs.Body;
                    args.Message = standArgs.Message;
                    args.Status = null;
                }

                OnStatusGet(args);
            });
        }
        public void Get(string token)
        {
            Get("", token);
        }
        #endregion
        #region events
        public event EventHandler<StatusEventsArgs> StatusSet;
        public event EventHandler<StatusEventsArgs> StatusGet;
        #endregion
        #region on events
        protected virtual void OnStatusSet(StatusEventsArgs e)
        {
            StatusSet?.Invoke(this, e);
        }
        protected virtual void OnStatusGet(StatusEventsArgs e)
        {
            StatusGet?.Invoke(this, e);
        }
        #endregion
        #region events args
        public class StandardEventArgs : EventArgs
        {
            public string Message { get; set; }
            public string Body { get; set; }
            public HttpStatusCode ResponseCode { get; set; }
        }
        public StandardEventArgs StandardEventArgsDeserialiser(IRestResponse response)
        {
            //Console.WriteLine(response.Content);
            JObject o = JObject.Parse(response.Content);
            StandardEventArgs args = new StandardEventArgs
            {
                ResponseCode = response.StatusCode,
                Message = (string)o["message"]
            };

            if (response.StatusCode == HttpStatusCode.OK)
            {
                args.Body = (string)o["body"];
            }
            else
            {
                args.Body = Newtonsoft.Json.JsonConvert.SerializeObject(o["body"]);
            }

            return args;
        }
        public class StatusEventsArgs : EventArgs
        {
            public HttpStatusCode ResponseCode { get; set; }
            public string Message { get; set; }
            public string Body { get; set; }
            public Status Status { get; set; }
        }
        #endregion
    }
}
