using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace API
{
    public class Message
    {
        public string MessageContent { get; set; }
        public string User_from { get; set; }
        public string User_to { get; set; }
        public DateTime Sended { get; set; }

        public Message() {}
        public Message(string message, string user_from, string user_to, DateTime sended)
        {
            MessageContent = message;
            User_from = user_from;
            User_to = user_to;
            Sended = sended;
        }
        public Message(string message, string user_from, string user_to, string sended)
        {
            MessageContent = message;
            User_from = user_from;
            User_to = user_to;

            if (DateTime.TryParse(sended, out DateTime tmpDate))
                Sended = tmpDate;
        }

        #region helpers
        private Message JsonToMessage(JToken o, Dictionary<int, string> users)
        {
            return new Message((string)o["message"], users[(int)o["from_user_id"]], users[(int)o["to_user_id"]], (string)o["date"]);
        }
        private Message JsonToMessage(JObject o, Dictionary<int, string> users)
        {
            return new Message((string)o["message"], users[(int)o["from_user_id"]], users[(int)o["to_user_id"]], (string)o["date"]);
        }
        #endregion
        #region API calls
        public void Send(string token)
        {
            var client = new RestClient(Core.siteMap.messageDir[SiteMap.MessageMethod.send]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            request.AddParameter("email", User_to);
            request.AddParameter("message", MessageContent);
            request.AddParameter("body", token);

            client.ExecuteAsync(request, response => { OnMessageSend(StandardEventArgsDeserialiser(response)); });
        }
        public void Get(int page, int ammount, string token)
        {
            var client = new RestClient(Core.siteMap.messageDir[SiteMap.MessageMethod.get]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            request.AddParameter("email", User_from);
            request.AddParameter("page", page);
            request.AddParameter("ammount", ammount);
            request.AddParameter("body", token);

            client.ExecuteAsync(request, response =>
            {
                GetEventsArgs args = new GetEventsArgs();
                args.ResponseCode = response.StatusCode;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject o = JObject.Parse(response.Content);
                    args.Message = (string)o["message"];

                    Dictionary<int, string> users = new Dictionary<int, string>();
                    users.Add((int)o["body"]["userFrom"]["id"], (string)o["body"]["userFrom"]["email"]);
                    users.Add((int)o["body"]["userTo"]["id"], (string)o["body"]["userTo"]["email"]);

                    JArray tm = (o["body"].SelectToken("messages") as JArray);
                    List <Message> mesList= new List<Message>();

                    foreach (JObject tmpMes in tm)
                    {
                        mesList.Add(JsonToMessage(tmpMes, users));
                    }

                    args.Messages = mesList;
                }
                else
                {
                    StandardEventArgs standArgs = StandardEventArgsDeserialiser(response);
                    args.Body = standArgs.Body;
                    args.Message = standArgs.Message;
                    args.Messages = null;
                }

                OnMessageGet(args);
            });
        }
        #endregion
        #region events
        public event EventHandler<StandardEventArgs> MessageSend;
        public event EventHandler<GetEventsArgs> MessageGet;
        #endregion
        #region on events
        protected virtual void OnMessageSend(StandardEventArgs e)
        {
            MessageSend?.Invoke(this, e);
        }
        protected virtual void OnMessageGet(GetEventsArgs e)
        {
            MessageGet?.Invoke(this, e);
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
            public List<Message> Messages { get; set; }
        }
        #endregion
    }
}
