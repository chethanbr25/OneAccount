using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AccountSkate
{
    /// <summary>
    /// Interaction logic for LoginUser.xaml
    /// </summary>
    public partial class LoginUser : UserControl
    {
        Propertiess dd;
        public LoginUser(Propertiess prop)
        {
            InitializeComponent();
            dd = prop;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();

           // (this.Parent as Grid).Children.Remove(this);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenAccount spOpenAccount = new OpenAccount();
                if (spOpenAccount.functionLogin(txtUserName.Text.Trim(), txtPassword.Password.Trim()))
                {
                    dd.login =true;
                    (this.Parent as Grid).Children.Remove(this);
                }
                else
                {
                    dd.login = false;
                    lblErrorLogin.Visibility = Visibility.Visible;
                    txtUserName.Clear();
                    txtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("LogIn:001" + ex.ToString()).ShowDialog()));
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            lblErrorLogin.Visibility = Visibility.Hidden;
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBlock1.Visibility = (txtUserName.Text != "") ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textBlock2.Visibility = (txtPassword.Password != "") ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
        }

        private void textBlock1_MouseEnter(object sender, MouseEventArgs e)
        {
            txtUserName.Focus();
        }

        private void textBlock2_MouseEnter(object sender, MouseEventArgs e)
        {
            txtPassword.Focus();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }
    }
}
