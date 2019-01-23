using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class SiteMap
    {
        public string root = "http://localhost/php/Projekty/EiT/Programowanie%20Aplikacyjne/Projekt%202/REST_API-Zarzadzanie_Firma/api/api/";

        public enum UserMethod {token, validate, create, update, delete, get};
        public Dictionary<UserMethod, string> userDir = new Dictionary<UserMethod, string>();

        public enum MessageMethod {send, get};
        public Dictionary<MessageMethod, string> messageDir = new Dictionary<MessageMethod, string>();

        public enum StatusMethod { set, get };
        public Dictionary<StatusMethod, string> statusDir = new Dictionary<StatusMethod, string>();

        public SiteMap()
        {
            userDir.Add(UserMethod.token, root+ "user/token.php");
            userDir.Add(UserMethod.validate, root + "user/validate.php");
            userDir.Add(UserMethod.create, root + "user/create.php");
            userDir.Add(UserMethod.update, root + "user/update.php");
            userDir.Add(UserMethod.delete, root + "user/delete.php");
            userDir.Add(UserMethod.get, root + "user/get.php");

            messageDir.Add(MessageMethod.send, root + "message/send.php");
            messageDir.Add(MessageMethod.get, root + "message/get.php");

            statusDir.Add(StatusMethod.get, root+"status/get.php");
            statusDir.Add(StatusMethod.set, root+ "status/set.php");
        }
    }
}
