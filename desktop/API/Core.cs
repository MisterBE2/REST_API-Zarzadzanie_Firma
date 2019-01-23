using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Core
    {
        public static SiteMap siteMap = new SiteMap();
        public static User mainUser = new User();

        #region Token
        private string token;
        public string Token {
            get
            {
                return token;
            }
            set
            {
                token = value;
                OnTokenSet(new TokenSetEventArgs(token));
            }
        }

        public event EventHandler<TokenSetEventArgs> TokenSet;
        protected virtual void OnTokenSet(TokenSetEventArgs e)
        {
            TokenSet?.Invoke(this, e);
        }
        public class TokenSetEventArgs : EventArgs
        {
            public string Token { get; set; }

            public TokenSetEventArgs(string token)
            {
                this.Token = token;
            }
        }
        #endregion

        #region Users
        private List<User> users;
        public List<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnUsersSet(new UsersSetEventArgs(users));
            }
        }

        public event EventHandler<UsersSetEventArgs> UsersSet;
        protected virtual void OnUsersSet(UsersSetEventArgs e)
        {
            UsersSet?.Invoke(this, e);
        }
        public class UsersSetEventArgs : EventArgs
        {
            public List<User> Users { get; set; }

            public UsersSetEventArgs(List<User> users)
            {
                this.Users = users;
            }
        }
        #endregion

        #region main user status
        private Status status;
        public Status Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnMainUserStatusSet(new StatusSetEventArgs(status));
            }
        }

        public event EventHandler<StatusSetEventArgs> MainUserStatusSet;
        protected virtual void OnMainUserStatusSet(StatusSetEventArgs e)
        {
            MainUserStatusSet?.Invoke(this, e);
        }
        public class StatusSetEventArgs : EventArgs
        {
            public Status Status { get; set; }

            public StatusSetEventArgs(Status status)
            {
                this.Status = status;
            }
        }
        #endregion
    }
}
