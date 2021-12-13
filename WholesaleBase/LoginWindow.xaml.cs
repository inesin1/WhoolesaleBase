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
using System.Windows.Shapes;

namespace WholesaleBase
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        bool isLogin = false;
        public LoginWindow()
        {
            InitializeComponent();
        }
        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DbService db = new DbService();

            string login = tbLogin.Text;
            string password = tbPassword.Password;

            try
            {
                user user = db.users.Where((u) => u.Login == login && u.Password == password).Single();
                MessageBox.Show("Успешно!", $"Привет, {user.Name}!");

                isLogin = true;

                if (user.ID == 1)
                {
                    MessageBoxResult result = MessageBox.Show("Хотите отредактировать список пользователей?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        new UserWindow().ShowDialog();
                }

                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка!", $"Неверный логин или пароль!");
            }
        }

        private void LoginWindow_Closed(object sender, EventArgs e)
        {
            if (!isLogin)
                App.Current.Shutdown();
        }
    }
}
