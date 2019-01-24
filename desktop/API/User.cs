using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace API
{
    public class User
    {
        #region prop
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int Permission { get; set; }
        public string Status { get; set; }
        public string Newemail { get; set; }
        #endregion
        #region constructor
        public User() { }
        public User(string id, string firstname, string lastname, string email, string position, string created, string updated, string permission, string newemail, string status)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Position = position;
            Newemail = newemail;
            Status = status;

            if (Int32.TryParse(id, out int tmpInt))
            {
                Id = tmpInt;
            }

            if (Int32.TryParse(permission, out tmpInt))
            {
                Permission = tmpInt;
            }

            if (DateTime.TryParse(created, out DateTime tmpDate))
            {
                Created = tmpDate;
            }

            if (DateTime.TryParse(updated, out tmpDate))
            {
                Updated = tmpDate;
            }
        }
        public User(int id, string firstname, string lastname, string email, string position, DateTime created, DateTime updated, int permission, string newemail, string status)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Position = position;
            Created = created;
            Updated = updated;
            Permission = permission;
            Newemail = newemail;
            Status = status;
        }

        #endregion
        #region helpers
        private User JsonToUser(JToken o)
        {
            return new User(
                           (string)o["id"],
                           (string)o["firstname"],
                           (string)o["lastname"],
                           (string)o["email"],
                           (string)o["position"],
                           (string)o["created"],
                           (string)o["updated"],
                           (string)o["permission"],
                           "",
                           (string)o["status"]
                       );
        }
        private User JsonToUser(JObject o)
        {
            return new User(
                           (string)o["id"],
                           (string)o["firstname"],
                           (string)o["lastname"],
                           (string)o["email"],
                           (string)o["position"],
                           (string)o["created"],
                           (string)o["updated"],
                           (string)o["permission"],
                           "",
                           (string)o["status"]
                       );
        }
        #endregion
        #region API calls
        public void Token(string password)
        {
            var client = new RestClient(Core.siteMap.userDir[SiteMap.UserMethod.token]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            request.AddParameter("email", Email);
            request.AddParameter("password", password);

            client.ExecuteAsync(request, response => {

                StandardEventArgs args = StandardEventArgsDeserialiser(response);
                OnTokenResult(args);
            });
        }
        public void Validate(string token)
        {
            var client = new RestClient(Core.siteMap.userDir[SiteMap.UserMethod.validate]);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };

            request.AddParameter("body", token.Length >0 ? token : token);

            client.ExecuteAsync(request, response =>
            {
                ValidateEventsArgs args = new ValidateEventsArgs();
                args.ResponseCode = response.StatusCode;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject o = JObject.Parse(response.Content);
                    args.Message = (string)o["message"];
                    args.User = JsonToUser(o["body"]);
                    args.Body = token.Length > 0 ? token : token;

                    Id = args.User.Id;
                    Firstname = args.User.Firstname;
                    Lastname = args.User.Lastname;
                    Email = args.User.Email;
                    Position = args.User.Position;
                    Created = args.User.Created;
                    Updated = args.User.Updated;
                    Permission = args.User.Permission;
                }
                else
                {
                    StandardEventArgs standArgs = StandardEventArgsDeserialiser(response);
                    args.Body = standArgs.Body;
                    args.Message = standArgs.Message;
                    args.User = null;
                }

                OnValidateResult(args);
            });
        }
        public void Get(string email, string token)
        {
            var client = new RestClient(Core.siteMap.userDir[SiteMap.UserMethod.get]);

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.AddParameter("body", token);
            if (email.Length > 0)
            {
                request.AddParameter("email", email);
            }

            client.ExecuteAsync(request, response =>
            {
                GetEventsArgs args = new GetEventsArgs();
                args.ResponseCode = response.StatusCode;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject o = JObject.Parse(response.Content);
                    args.Message = (string)o["message"];

                    JArray tempUsers = (o.SelectToken("body") as JArray);
                    args.Users = new List<User>();

                    foreach (JObject tempUser in tempUsers)
                    {
                        //.WriteLine(JsonConvert.SerializeObject(tempUser, Formatting.Indented));
                       args.Users.Add(JsonToUser(tempUser));
                    }
                }
                else
                {
                    StandardEventArgs standArgs = StandardEventArgsDeserialiser(response);
                    args.Body = standArgs.Body;
                    args.Message = standArgs.Message;
                    args.Users = null;
                }

                OnGetResult(args);
            });
        }
        public void Delete(string token)
        {
            var client = new RestClient(Core.siteMap.userDir[SiteMap.UserMethod.delete]);

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.DELETE;
            request.AddParameter("body", token);
            request.AddParameter("email", Email);

            client.ExecuteAsync(request, response =>
            {
                OnDeleteResult(StandardEventArgsDeserialiser(response));
            });
        }
        public void Update(string password, string token)
        {
            var client = new RestClient(Core.siteMap.userDir[SiteMap.UserMethod.update]);

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.POST;
            request.AddParameter("body", token);
            request.AddParameter("firstname", Firstname);
            request.AddParameter("lastname", Lastname);
            request.AddParameter("email", Email);
            request.AddParameter("password", password);
            request.AddParameter("newemail", Newemail == null ? Email : Newemail);
            request.AddParameter("position", Position);

            //Console.WriteLine(JsonConvert.SerializeObject(request, Formatting.Indented));

            client.ExecuteAsync(request, response =>
            {
                OnUpdateResult(StandardEventArgsDeserialiser(response));
            });
        }
        public void Create(string password, string token)
        {
            var client = new RestClient(Core.siteMap.userDir[SiteMap.UserMethod.create]);

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.POST;

            request.AddParameter("body", token);
            request.AddParameter("firstname", Firstname);
            request.AddParameter("lastname", Lastname);
            request.AddParameter("email", Email);
            request.AddParameter("password", password);
            request.AddParameter("position", Position);

            client.ExecuteAsync(request, response =>
            {
                OnCreateResult(StandardEventArgsDeserialiser(response));
            });
        }
        #endregion
        #region events
        public event EventHandler<StandardEventArgs> TokenResult;
        public event EventHandler<StandardEventArgs> CreateResult;
        public event EventHandler<StandardEventArgs> DeleteResult;
        public event EventHandler<StandardEventArgs> UpdateResult;
        public event EventHandler<ValidateEventsArgs> ValidateResult;
        public event EventHandler<GetEventsArgs> GetResult;
        #endregion
        #region on events
        protected virtual void OnTokenResult(StandardEventArgs e)
        {
            TokenResult?.Invoke(this, e);
        }
        protected virtual void OnCreateResult(StandardEventArgs e)
        {
            CreateResult?.Invoke(this, e);
        }
        protected virtual void OnDeleteResult(StandardEventArgs e)
        {
            DeleteResult?.Invoke(this, e);
        }
        protected virtual void OnUpdateResult(StandardEventArgs e)
        {
            UpdateResult?.Invoke(this, e);
        }
        protected virtual void OnValidateResult(ValidateEventsArgs e)
        {
            ValidateResult?.Invoke(this, e);
        }
        protected virtual void OnGetResult(GetEventsArgs e)
        {
            GetResult?.Invoke(this, e);
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

        public class ValidateEventsArgs : EventArgs
        {
            public HttpStatusCode ResponseCode { get; set; }
            public string Message { get; set; }
            public string Body { get; set; }
            public User User { get; set; }
        }
        public class GetEventsArgs : EventArgs
        {
            public HttpStatusCode ResponseCode { get; set; }
            public string Message { get; set; }
            public string Body { get; set; }
            public List<User> Users { get; set; }
        }
        #endregion
    }
}
