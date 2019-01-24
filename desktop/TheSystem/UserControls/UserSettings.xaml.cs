using API;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static TheSystem.Home;


namespace TheSystem.UserControls
{
    /// <summary>
    /// Logika interakcji dla klasy UserSettings.xaml
    /// </summary>
    public partial class UserSettings : UserControl
    {
        public Home Home { get; set; }
        public Core Core { get; set; }
        public User User { get; set; }
        public User TempUser { get; set; }

        public enum ControlDestination { userSettings, createUser, editUser };

        private ControlDestination destination;
        public ControlDestination Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;

                switch (destination)
                {
                    case ControlDestination.userSettings:
                        labelheader.Content = "Settings";
                        break;
                    case ControlDestination.createUser:
                        labelheader.Content = "Create User";
                        break;
                    case ControlDestination.editUser:
                        labelheader.Content = "Edit User: " + User.Firstname + " " + User.Lastname;
                        break;
                    default:
                        break;
                }
            }
        }

        public UserSettings(Home home, Core core, User user, ControlDestination destination)
        {
            Home = home;
            Core = core;
            User = user;

            InitializeComponent();

            Destination = destination;

            if (Core.User.Permission > 0)
            {
                textBoxEmail.IsEnabled = false;
                textBoxPosition.IsEnabled = false;
            }

            if (User != null)
                parseUser();
            else
                clearInputs();
        }

        private void parseUser()
        {
            textBoxEmail.Text = User.Email;
            textBoxName.Text = User.Firstname;
            textBoxSurname.Text = User.Lastname;
            textBoxPosition.Text = User.Position;
        }

        private void clearInputs()
        {
            textBoxEmail.Text = "";
            textBoxName.Text = "";
            textBoxSurname.Text = "";
            textBoxPosition.Text = "";
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Home.CurrentAction = GlobalAction.users;
        }

        private void Save()
        {
            TempUser = new User
            {
                Email = User == null ? textBoxEmail.Text : User.Email,
                Firstname = textBoxName.Text,
                Lastname = textBoxSurname.Text,
                Position = textBoxPosition.Text,
                Newemail = textBoxEmail.Text
            };

            TempUser.CreateResult += User_CreateResult;
            TempUser.UpdateResult += User_UpdateResult;

            if (Destination == ControlDestination.createUser)
                TempUser.Create(textBoxPassword.Password, Core.Token);
            else
                TempUser.Update(textBoxPassword.Password, Core.Token);
        }

        private void User_CreateResult(object sender, User.StandardEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (e.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    Home.CurrentAction = GlobalAction.users;
                }
                else
                {
                    Home.SetVidowStatus(e.Message);
                }
            });
        }

        private void User_UpdateResult(object sender, User.StandardEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (e.ResponseCode == System.Net.HttpStatusCode.OK)
                {

                    if (Destination == ControlDestination.userSettings)
                    {
                        TheSystem.Properties.Settings.Default.Token = e.Body;
                        TheSystem.Properties.Settings.Default.Save();

                        App.Restart();
                    }
                    else if (Destination == ControlDestination.editUser)
                        Home.CurrentAction = GlobalAction.users;

                }
                else
                {
                    Home.SetVidowStatus(e.Message);
                }
            });
        }

        private void Core_TokenSet(object sender, Core.TokenSetEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save();
            }
        }
    }
}
