using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static TheSystem.Home;
using API;
using TheSystem.UserControls;

namespace TheSystem.UserControls
{
    /// <summary>
    /// Logika interakcji dla klasy StatusControl.xaml
    /// </summary>
    public partial class StatusControl : UserControl
    {
        public Home Home { get; set; }
        public Core Core { get; set; }

        public StatusControl(Home home, Core core)
        {
            Home = home;
            Core = core;

            InitializeComponent();
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            labelChars.Content = (128 - textBoxContent.Text.Length) + " left";

            if(e.Key == Key.Enter)
            {
                SetStatus();
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SetStatus();
        }

        private void SetStatus()
        {
            Status status = new Status { User_id = Core.User.Id, StatusContent = textBoxContent.Text };
            status.StatusSet += Status_StatusSet;

            Application.Current.Dispatcher.Invoke(delegate
            {
                thisControl.Children.Clear();
                SpinnerControl spin = new SpinnerControl();
                thisControl.Children.Add(spin);
            });

            status.Set(Core.Token);
        }

        private void Status_StatusSet(object sender, Status.StatusEventsArgs e)
        {
            if (e.ResponseCode == System.Net.HttpStatusCode.OK)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Core.Status = e.Status;
                    Home.CurrentAction = GlobalAction.users;
                });
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Home.CurrentAction = GlobalAction.users;
        }
    }
}
