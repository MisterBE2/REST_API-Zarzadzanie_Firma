using API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TheSystem.UserControls;
using TheSystem.UserControls.Message;

namespace TheSystem
{
    /// <summary>
    /// Logika interakcji dla klasy Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        Core Core = new Core();
        public List<User> Users = new List<User>();
        public enum GlobalAction { users, settings, status, delete, createUser, editUser, message }
        public StatusControl StatusControl { get; set; }
        public UserSettings UserSettings { get; set; }
        public User selectedUser { get; set; }
        private DispatcherTimer autoScroll = new DispatcherTimer();

        private GlobalAction currentAction = GlobalAction.users;
        public GlobalAction CurrentAction
        {
            get
            {
                return currentAction;
            }
            set
            {
                currentAction = value;
                SpinnerControl sp = new SpinnerControl();

                switch (value)
                {
                    case GlobalAction.users:
                        usersPanel.Children.Clear();
                        usersPanel.Children.Add(sp);
                        Core.User.Get("", Core.Token);
                        break;
                    case GlobalAction.settings:
                        usersPanel.Children.Clear();
                        UserSettings = new UserSettings(this, Core, Core.User, UserSettings.ControlDestination.userSettings);
                        usersPanel.Children.Add(UserSettings);
                        break;
                    case GlobalAction.status:
                        usersPanel.Children.Clear();
                        StatusControl = new StatusControl(this, Core);
                        usersPanel.Children.Add(StatusControl);
                        break;
                    case GlobalAction.delete:
                        MessageBoxResult result = MessageBox.Show(("Are you sure you want to delete user: " + selectedUser.Firstname + " " + selectedUser.Lastname), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            usersPanel.Children.Add(sp);
                            selectedUser.DeleteResult += SelectedUser_DeleteResult;
                            selectedUser.Delete(Core.Token);
                        }
                        break;
                    case GlobalAction.createUser:
                        usersPanel.Children.Clear();
                        UserSettings = new UserSettings(this, Core, null, UserSettings.ControlDestination.createUser);
                        usersPanel.Children.Add(UserSettings);
                        break;
                    case GlobalAction.editUser:
                        usersPanel.Children.Clear();
                        UserSettings = new UserSettings(this, Core, selectedUser, UserSettings.ControlDestination.editUser);
                        usersPanel.Children.Add(UserSettings);
                        break;
                    case GlobalAction.message:
                        usersPanel.Children.Clear();
                        MessageControl m = new MessageControl(this, Core, selectedUser);
                        usersPanel.Children.Add(m);
                        scroll.ScrollToEnd();
                        break;
                    default:
                        break;
                }
            }
        }

        private void SelectedUser_DeleteResult(object sender, User.StandardEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                SetVidowStatus(e.Message);
                if (e.ResponseCode == HttpStatusCode.OK)
                {
                    CurrentAction = GlobalAction.users;
                }
            });
        }

        public Home(Core core)
        {
            Core = core;

            InitializeComponent();

            Core.MainUserStatusSet += Core_MainUserStatusSet;
            Core.User.GetResult += User_GetResult;

            SetMainUserInfo();

            Status st = new Status();
            st.StatusGet += St_StatusGet;
            st.User_id = Core.User.Id;
            st.Get(Core.Token);

            CurrentAction = GlobalAction.users;

            DispatcherTimer usersAutoUpdate = new DispatcherTimer();
            usersAutoUpdate.Tick += usersAutoUpdate_Tick;
            usersAutoUpdate.Interval = new TimeSpan(0, 0, 5);
            usersAutoUpdate.Start();

            DispatcherTimer statusAutoUpdate = new DispatcherTimer();
            statusAutoUpdate.Tick += statusAutoUpdate_Tick;
            statusAutoUpdate.Interval = new TimeSpan(0, 0, 2);
            statusAutoUpdate.Start();
        }

        private void statusAutoUpdate_Tick(object sender, EventArgs e)
        {
            Status s = new Status();
            s.StatusGet += S_StatusGet;
            s.Get(Core.User.Email, Core.Token);
        }

        private void S_StatusGet(object sender, Status.StatusEventsArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (e.ResponseCode == HttpStatusCode.OK)
                {
                    if (Core.Status.StatusContent != e.Status.StatusContent)
                    {
                        Core.Status = e.Status;
                    }
                }
            });
        }

        public void SetMainUserInfo()
        {
            labelUserName.Content = Core.User.Firstname + " " + Core.User.Lastname;
            labelEmail.Content = Core.User.Email;
            labelJoined.Content = Core.User.Created.ToShortDateString() + "r.";
            labelPosition.Content = Core.User.Position;
            labelStatus.Content = Core.Status.StatusContent;

            if (Core.User.Permission > 0)
            {
                labelAdministrator.Visibility = Visibility.Collapsed;
                buttonCreateUser.Visibility = Visibility.Collapsed;
            }
        }

        private void usersAutoUpdate_Tick(object sender, EventArgs e)
        {
            if (currentAction == GlobalAction.users)
            {
                Core.User.Get("", Core.Token);
            }
        }

        private void User_GetResult(object sender, User.GetEventsArgs e)
        {
            if (e.ResponseCode == HttpStatusCode.OK)
            {
                if (Application.Current.Dispatcher != null)
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        SetVidowStatus(e.Message);
                        var usersTheSame = Users.All(e.Users.Contains) && Users.Count == e.Users.Count;
                        if (currentAction == GlobalAction.users && !usersTheSame)
                        {
                            Users = e.Users;
                            usersPanel.Children.Clear();

                            WrapPanel wp = new WrapPanel();
                            usersPanel.Children.Add(wp);

                            foreach (User user in e.Users)
                            {
                                UserSnippet userSnippet = new UserSnippet(user, Core, this);
                                wp.Children.Add(userSnippet);
                            }
                        }
                    });
                }
            }
        }

        private void St_StatusGet(object sender, Status.StatusEventsArgs e)
        {
            if (e.ResponseCode == HttpStatusCode.OK)
            {
                Core.Status = e.Status;
            }
            //else
            //{
            //    MessageBoxResult result = MessageBox.Show(e.Body + " " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Core.Token = null;
            TheSystem.Properties.Settings.Default.Token = null;
            TheSystem.Properties.Settings.Default.Save();

            MainWindow main = new MainWindow();
            main.Show();

            Close();
        }

        private void Core_MainUserStatusSet(object sender, Core.StatusSetEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                labelStatus.Content = e.Status.StatusContent;
                labelStatus.Visibility = Visibility.Visible;
            });
        }

        private void ButtonAccountSettings_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAction == GlobalAction.users)
            {
                CurrentAction = GlobalAction.settings;
            }
        }

        private void ButtonChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAction == GlobalAction.users)
            {
                CurrentAction = GlobalAction.status;
            }
        }

        private void ButtonNewUser_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAction == GlobalAction.users)
            {
                CurrentAction = GlobalAction.createUser;
            }
        }

        public void SetVidowStatus(string status)
        {
            labelVindowStatus.Content = status;
        }
    }
}
