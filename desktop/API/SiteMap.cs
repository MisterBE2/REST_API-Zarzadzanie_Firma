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

        public enum userMethod {token, validate, create, update, delete, get};
        public Dictionary<userMethod, string> userDir = new Dictionary<userMethod, string>();

        public enum messageMethod {send, get};
        public Dictionary<messageMethod, string> messageDir = new Dictionary<messageMethod, string>();

        public enum statusMethod { set, get };
        public Dictionary<statusMethod, string> statusDir = new Dictionary<statusMethod, string>();

        public SiteMap()
        {
            userDir.Add(userMethod.token, root+ "user/token.php");
            userDir.Add(userMethod.validate, root + "user/validate.php");
            userDir.Add(userMethod.create, root + "user/create.php");
            userDir.Add(userMethod.update, root + "user/update.php");
            userDir.Add(userMethod.delete, root + "user/delete.php");
            userDir.Add(userMethod.get, root + "user/get.php");

            messageDir.Add(messageMethod.send, root + "message/send.php");
            messageDir.Add(messageMethod.get, root + "message/get.php");

            statusDir.Add(statusMethod.get, root+"status/get.php");
            statusDir.Add(statusMethod.set, root+ "status/set.php");
        }
    }
}
