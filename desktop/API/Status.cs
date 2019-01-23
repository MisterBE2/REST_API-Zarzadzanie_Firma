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
        public void Set()
        {
            var client = new RestClient(Core.siteMap.statusDir[SiteMap.StatusMethod.set]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            request.AddParameter("status", StatusContent);
            request.AddParameter("body", Main.Core.Token);

            client.ExecuteAsync(request, response => { OnStatusSet(StandardEventArgsDeserialiser(response)); });
        }
        public void Get()
        {
            var client = new RestClient(Core.siteMap.statusDir[SiteMap.StatusMethod.get]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            request.AddParameter("body", Main.Core.Token);

            client.ExecuteAsync(request, response =>
            {
                GetEventsArgs args = new GetEventsArgs();
                args.ResponseCode = response.StatusCode;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject o = JObject.Parse(response.Content);
                    args.Message = (string)o["message"];
                    args.Status = new Status((string)o["body"]["status"], (string)o["body"]["updated"], (string)o["body"]["user_id"]);
                }
                else
                {
                    StandardEventArgs standArgs = StandardEventArgsDeserialiser(response);
                    args.Body = standArgs.Body;
                    args.Message = standArgs.Message;
                    args.Status = null;
                }

                if (this.User_id == Core.mainUser.Id)
                    Main.Core.Status = args.Status;

                OnStatusGet(args);
            });
        }
        #endregion
        #region events
        public event EventHandler<StandardEventArgs> StatusSet;
        public event EventHandler<GetEventsArgs> StatusGet;
        #endregion
        #region on events
        protected virtual void OnStatusSet(StandardEventArgs e)
        {
            StatusSet?.Invoke(this, e);
        }
        protected virtual void OnStatusGet(GetEventsArgs e)
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
        public class GetEventsArgs : EventArgs
        {
            public HttpStatusCode ResponseCode { get; set; }
            public string Message { get; set; }
            public string Body { get; set; }
            public Status Status { get; set; }
        }
        #endregion

    }
}
