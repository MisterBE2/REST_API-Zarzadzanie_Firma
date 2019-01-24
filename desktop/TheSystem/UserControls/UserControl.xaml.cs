using API;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TheSystem.UserControls
{
    /// <summary>
    /// Logika interakcji dla klasy User.xaml
    /// </summary>
    public partial class UserSnippet : UserControl
    {
        public User User { get; set; }
        public Core Core { get; set; }
        public Home Home { get; set; }

        public UserSnippet(User user, Core core, Home home)
        {
            User = user;
            Core = core;
            Home = home;

            InitializeComponent();

            if(User.Permission > 0) labelADM.Visibility = Visibility.Collapsed;

            if (Core.User.Permission > 0)
            {
                buttonDelete.Visibility = Visibility.Collapsed;
                buttonEdit.Visibility = Visibility.Collapsed;
            }

            labelUser.Content = User.Firstname + " " + User.Lastname;
            textBlockPosition.Text = User.Position;
            textBlockStatus.Text = User.Status;

            if (User.Status == null)
                labelStatus.Visibility = Visibility.Collapsed;

            DispatcherTimer statusAutoUpdate = new DispatcherTimer();
            statusAutoUpdate.Tick += statusAutoUpdate_Tick;
            statusAutoUpdate.Interval = new TimeSpan(0, 0, 2);
            statusAutoUpdate.Start();

        }

        private void statusAutoUpdate_Tick(object sender, EventArgs e)
        {
            Status s = new Status();
            s.StatusGet += S_StatusGet;
            s.Get(User.Email, Core.Token);
        }

        private void S_StatusGet(object sender, Status.StatusEventsArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if(e.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    if(User.Status != e.Status.StatusContent)
                    {
                        textBlockStatus.Text = User.Status = e.Status.StatusContent;

                        if (User.Status == null)
                            labelStatus.Visibility = Visibility.Collapsed;
                        else
                            labelStatus.Visibility = Visibility.Visible;
                    }
                }
            });
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Home.selectedUser = User;
            Home.CurrentAction = Home.GlobalAction.editUser;
        }

        private void ButtonPM_Click(object sender, RoutedEventArgs e)
        {
            Home.selectedUser = User;
            Home.CurrentAction = Home.GlobalAction.message;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Home.selectedUser = User;
            Home.CurrentAction = Home.GlobalAction.delete;
        }
    }
}
