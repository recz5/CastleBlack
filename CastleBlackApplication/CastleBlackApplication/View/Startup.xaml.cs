using System;
using System.Collections.Generic;
using System.Data.Objects;
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
using System.Windows.Shapes;

namespace CastleBlackApplication.View
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        CastleBlackEntities cbEntities = new CastleBlackEntities();
        public string user = Environment.UserName;
        public Startup()
        {
            InitializeComponent();
        }

        private void WelcomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            welcome_Textblock.Text = "Loading...";

            var qry = from qas in cbEntities.QAs
                      where qas.UserName == user
                      select new { qas.UserName, qas.First_Name, qas.Last_Name };

            if (!qry.Any())
            {
                new_Button.IsEnabled = false;
                welcome_Textblock.Text = string.Format("Hi {0}! Welcome To Castle Black!\r\rPlease wait while I set you up to the system...\r", user);
                
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));

                cbEntities.addUser(user, "castleblack", user, user, responseMessage);

                if (responseMessage.Value.ToString() == "Success")
                {
                    welcome_Textblock.Text = welcome_Textblock.Text + string.Format("Your all set.\rYour user name is {0}\r\rPlease select one of the following.\r\rNew if this a new case\r and\r Existing to continue work on an existing case.", user);

                    new_Button.IsEnabled = true;
                }
            }
            else 
            {
                welcome_Textblock.Text = string.Format("Hi {0}! Welcome Back!\r\rPlease select one of the following.\r\rNew if this a new case\r and\r Existing to continue work on an existing case.", user);

                new_Button.IsEnabled = true;
            }
            
        }

        private void new_Button_Click(object sender, RoutedEventArgs e)
        {
            NewCaseWindow main = new NewCaseWindow();
            main.Show();
            this.Close();
        }

        private void existing_Button_Click(object sender, RoutedEventArgs e)
        {
            NewCaseWindow main = new NewCaseWindow();
            main.Show();
            this.Close();
        }
    }
}
