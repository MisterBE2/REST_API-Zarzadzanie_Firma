using API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace TheSystem.UserControls.Message
{
    /// <summary>
    /// Logika interakcji dla klasy MessageControl.xaml
    /// </summary>
    public partial class MessageControl : UserControl
    {
        public List<API.Message> Messages = new List<API.Message>();
        public Core Core { get; set; }
        public Home Home { get; set; }
        public User User { get; set; }

        public MessageControl(Home home, Core core, User user)
        {
            Home = home;
            Core = core;
            User = user;

            InitializeComponent();

            GetMessages();

            DispatcherTimer messagesAutoUpdate = new DispatcherTimer();
            messagesAutoUpdate.Tick += ((sender, e) => { GetMessages(); });
            messagesAutoUpdate.Interval = new TimeSpan(0, 0, 5);
            messagesAutoUpdate.Start();
        }

        private void GetMessages()
        {
            API.Message m = new API.Message { User_from = User.Email };
            m.MessageGet += M_MessageGet;
            m.Get(0, 100, Core.Token);
        }

        private void M_MessageGet(object sender, API.Message.GetEventsArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (e.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    var messagesTheSame = Messages.All(e.Messages.Contains) && Messages.Count == e.Messages.Count;
                    if (!messagesTheSame)
                    {
                        Messages = e.Messages;
                        content.Children.Clear();

                        foreach (API.Message message in e.Messages)
                        {
                            MessageSnippet messageSnippet;

                            if (message.User_from == Core.User.Email)
                                messageSnippet = new MessageSnippet(MessageSnippet.MessageType.sender, message);
                            else
                                messageSnippet = new MessageSnippet(MessageSnippet.MessageType.receiver, message);

                            content.Children.Add(messageSnippet);
                        }
                    }
                }
                else
                    Home.SetVidowStatus(e.Message);
            });
        }

        private void Send()
        {
            if (textBoxMessage.Text.Length > 0)
            {
                API.Message m = new API.Message { User_from = Core.User.Email, User_to = User.Email, MessageContent = textBoxMessage.Text };
                m.MessageSend += M_MessageSend;
                m.Send(Core.Token);

                textBoxMessage.Text = "";
            }
        }

        private void M_MessageSend(object sender, API.Message.StandardEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (e.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    GetMessages();
                    Home.scroll.ScrollToEnd();
                }
                else
                    Home.SetVidowStatus(e.Message);
            });
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            Send();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Home.CurrentAction = Home.GlobalAction.users;
        }

        private void Container_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Home.CurrentAction = Home.GlobalAction.users;
            }
            else if (e.Key == Key.Enter)
            {
                Send();
            }
        }
    }
}
