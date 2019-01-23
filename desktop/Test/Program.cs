using API;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Test
{
    class Program
    {
        public static string Token { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Start");


            API.Main.Core.TokenSet += Core_TokenSet;
            API.Main.Core.MainUserStatusSet += Core_MainUserStatusSet;
            Core.mainUser.Email = "admin";

            
            Core.mainUser.TokenResult += User_tokenResult;
            Core.mainUser.ValidateResult += User_validateResult;
            Core.mainUser.GetResult += User_getResult;

            Print("Token");
            Core.mainUser.Token("admin");
            Console.ReadKey();
        }

        private static void Core_MainUserStatusSet(object sender, Core.StatusSetEventArgs e)
        {
            Console.WriteLine("User updated status!");
            Print(e.Status.StatusContent);
        }

        private static void Core_TokenSet(object sender, Core.TokenSetEventArgs e)
        {
            //Core.mainUser.Validate();
            //Core.mainUser.Get();

            //Console.WriteLine("Creating user");
            //User temp = new User
            //{
            //    Firstname = "Grażyna",
            //    Lastname = "Pączek",
            //    Email = "graz.poaczek@firma.com",
            //    Position = "Accountant",
            //};

            //temp.CreateResult += Temp_CreateResult;
            //temp.Create("12345");

            //Message mes = new Message
            //{
            //    MessageContent = "Testowa wiadmomość do pani sprzątaczki",
            //    User_to = "bogumila.alpaczak@gmail.com"
            //};

            //mes.MessageSend += Mes_MessageSend;
            //mes.Send();

            //Message mes = new Message
            //{
            //    User_from = "bogumila.alpaczak@gmail.com"
            //};

            //mes.MessageGet += Mes_MessageGet;
            //mes.Get(0, 100);

            //Status st = new Status();
            //st.StatusContent = "Status z api!";
            //st.User_id = Core.mainUser.Id;

            //st.StatusSet += St_StatusSet;
            //st.StatusGet += St_StatusGet;

            //st.Set();
            //st.Get();
        }

        private static void St_StatusGet(object sender, Status.GetEventsArgs e)
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

            if(e.ResponseCode == HttpStatusCode.OK)
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
            u.Update("");
        }

        private static void U_UpdateResult(object sender, User.StandardEventArgs e)
        {
            Console.WriteLine("Upate done");
            Print(e.ResponseCode.ToString());
            Print(e.Message);
            Print(e.Body);

            User u = (User)sender;

            u.DeleteResult += U_DeleteResult;
            u.Delete();
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
                string json = JsonConvert.SerializeObject(e.User, Formatting.Indented);
                Print(json);
            }
            else
            {
                Print(e.Body);
            }
        }

        private static void User_tokenResult(object sender, User.StandardEventArgs e)
        {
            //Console.WriteLine("Token retrieved");
            //Print(e.ResponseCode.ToString());
            //Print(e.Message);
            //Print(e.Body);

            if (e.ResponseCode == HttpStatusCode.OK)
            {
                API.Main.Core.Token = e.Body;
            }
        }

        public static void Print(string mes)
        {
            Console.WriteLine("=> " + mes);
        }
    }
}
