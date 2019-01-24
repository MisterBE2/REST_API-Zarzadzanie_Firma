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
using API;

namespace TheSystem.UserControls.Message
{
    /// <summary>
    /// Logika interakcji dla klasy MessageSnippet.xaml
    /// </summary>
    public partial class MessageSnippet : UserControl
    {
        public enum MessageType {sender, receiver}

        private MessageType type;
        public MessageType Type {
            get
            {
                return type;
            }
            set
            {
                type = value;

                if(value == MessageType.receiver)
                {
                    mainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(198,198,198));
                }
                else
                {
                    mainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(27, 118, 251));
                    Canvas.SetRight(mainBorder, 0);
                }
            }
        }

        public MessageSnippet(MessageType type, API.Message message)
        {
            InitializeComponent();

            Type = type;

            labelDate.Content = message.Sended.ToString();
            labelUser.Content = message.User_from;
            textBlockMessage.Text = message.MessageContent;
        }
    }
}
