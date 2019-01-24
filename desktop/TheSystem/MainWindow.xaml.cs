using API;
using System.Net;
using System.Windows;

namespace TheSystem
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core Core = new Core();

        public MainWindow()
        {
            InitializeComponent();
            Core.User.TokenResult += MainUser_TokenResult;
            Core.User.ValidateResult += MainUser_ValidateResult;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            Core.User.Email = inputLogin.Text;
            Core.User.Token(inputPassword.Password);
        }

        private void MainUser_TokenResult(object sender, User.StandardEventArgs e)
        {
            if (e.ResponseCode == HttpStatusCode.OK)
            {
                TheSystem.Properties.Settings.Default.Token = e.Body;
                TheSystem.Properties.Settings.Default.Save();
                Core.Token = e.Body;
                Core.User.Validate(e.Body);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    laberError.Content = e.Message;
                });
            }
        }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            if (TheSystem.Properties.Settings.Default.Token != null)
                if (TheSystem.Properties.Settings.Default.Token.Length > 0)
                {
                    Core.Token = TheSystem.Properties.Settings.Default.Token;
                    Core.User.Validate(Core.Token);
                }
        }

        private void MainUser_ValidateResult(object sender, User.ValidateEventsArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                //MessageBoxResult result = MessageBox.Show((JsonConvert.SerializeObject(e.Body, Formatting.Indented)), "", MessageBoxButton.OK, MessageBoxImage.None);
                //MessageBoxResult result = MessageBox.Show(e.Message + " " + e.Body + " " + Core.Token, "", MessageBoxButton.OK, MessageBoxImage.None);
            });

            if (e.ResponseCode == HttpStatusCode.OK)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Home home = new Home(Core);
                    home.Show();
                    Close();
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    TheSystem.Properties.Settings.Default.Token = null;
                    TheSystem.Properties.Settings.Default.Save();

                    App.Restart();
                });
            }
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Core.User.Email = inputLogin.Text;
                Core.User.Token(inputPassword.Password);
            }
        }
    }
}
