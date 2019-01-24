using API;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Test
{
    class Program
    {
        static Core Core = new Core();

        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            Core.TokenSet += Core_TokenSet;
            Core.MainUserStatusSet += Core_UserStatusSet;

            Core.User.TokenResult += User_tokenResult;
            Core.User.ValidateResult += User_validateResult;
            Core.User.GetResult += User_getResult;
            Core.User.Email = "admin";

            Core.User.Token("admin");

            Console.ReadKey();
        }

        private static void Core_UserStatusSet(object sender, Core.StatusSetEventArgs e)
        {
            Console.WriteLine("User updated status!");
            Print(e.Status.StatusContent);
        }

        private static void Core_TokenSet(object sender, Core.TokenSetEventArgs e)
        {
            //Console.WriteLine("Token set!");
            //Print(e.Token);

            //Core.User.Validate(e.Token);
            Core.User.Get("", e.Token);

            //Console.WriteLine("Creating user");
            //User temp = new User
            //{
            //    Firstname = "Grażyna",
            //    Lastname = "Pączek",
            //    Email = "graz.poaczek@firma.com",
            //    Position = "Accountant",
            //};

            //temp.CreateResult += Temp_CreateResult;
            //temp.Create("12345", e.Token);

            //Message mes = new Message
            //{
            //    MessageContent = "Testowa wiadmomość do pani sprzątaczki",
            //    User_to = "bogumila.alpaczak@gmail.com"
            //};

            //mes.MessageSend += Mes_MessageSend;
            //mes.Send(e.Token);

            //Message mes = new Message
            //{
            //    User_from = "bogumila.alpaczak@gmail.com"
            //};

            //mes.MessageGet += Mes_MessageGet;
            //mes.Get(0, 100, e.Token);

            //Status st = new Status();
            //st.StatusContent = "Status z api!";
            //st.User_id = Core.User.Id;
            //st.StatusSet += St_StatusSet;
            //st.Set(e.Token);

            //Status st = new Status();
            //st.StatusContent = "Status z api!";
            //st.User_id = Core.User.Id;
            //st.StatusGet += St_StatusGet;
            //st.Get(e.Token);
        }

        private static void St_StatusGet(object sender, Status.StatusEventsArgs e)
        {
            Console.WriteLine("Receiving status done");
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            Print(e.Body);

            if (e.ResponseCode == HttpStatusCode.OK)
            {
                Print(e.Status.StatusContent);
            }
        }

        private static void St_StatusSet(object sender, Status.StandardEventArgs e)
        {
            Console.WriteLine("Changing status done");
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            Print(e.Body);
        }

        private static void Mes_MessageSend(object sender, Message.StandardEventArgs e)
        {
            Console.WriteLine("Sending message end");
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            Print(e.Body);
        }

        private static void Mes_MessageGet(object sender, Message.GetEventsArgs e)
        {
            Console.WriteLine("Receiving message end");
            Print(e.ResponseCode.ToString());
            Print(e.Body);

            if (e.ResponseCode == HttpStatusCode.OK)
            {
                string json = JsonConvert.SerializeObject(e.Messages, Formatting.Indented);
                Print(json);
            }
        }

        private static void Temp_CreateResult(object sender, User.StandardEventArgs e)
        {
            Console.WriteLine("Creation done");
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            Print(e.Body);

            User u = (User)sender;
            u.Firstname = "Jadwiga";

            u.UpdateResult += U_UpdateResult;
            u.Update("", Core.Token);
        }

        private static void U_UpdateResult(object sender, User.StandardEventArgs e)
        {
            Console.WriteLine("Upate done");
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            Print(e.Body);

            User u = (User)sender;

            u.DeleteResult += U_DeleteResult;
            u.Delete(Core.Token);
        }

        private static void U_DeleteResult(object sender, User.StandardEventArgs e)
        {
            Console.WriteLine("Delete done");
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            Print(e.Body);

        }

        private static void User_getResult(object sender, User.GetEventsArgs e)
        {
            Print(e.ResponseCode.ToString());
            Print(e.Message);

            if (e.ResponseCode == HttpStatusCode.OK)
            {
                foreach (User user in e.Users)
                {
                    string json = JsonConvert.SerializeObject(user, Formatting.Indented);
                    Print(json);
                }
            }
            else
            {
                Print(e.Body);
            }
        }

        private static void User_validateResult(object sender, User.ValidateEventsArgs e)
        {
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            if (e.User != null)
            {
                Print(JsonConvert.SerializeObject(e.User, Formatting.Indented));

                Print(JsonConvert.SerializeObject(Core.User, Formatting.Indented));

                Status st = new Status();
                st.StatusGet += St_StatusGet1;
                st.User_id = e.User.Id;
                st.User_id = Core.User.Id;
                st.Get(Core.Token);
            }
            else
            {
                Print(e.Body);
            }
        }

        private static void St_StatusGet1(object sender, Status.StatusEventsArgs e)
        {
            Core.Status = e.Status;
        }

        private static void User_tokenResult(object sender, User.StandardEventArgs e)
        {
            //Console.WriteLine("Token retrieved");
            //Print(e.ResponseCode.ToString());
            //Print(e.Message);
            //Print(e.Body);

            if (e.ResponseCode == HttpStatusCode.OK)
            {
                Core.Token = e.Body;
            }
        }

        public static void Print(string mes)
        {
            Console.WriteLine("=> " + mes);
        }
    }
}
