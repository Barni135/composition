using System.Windows;

namespace composition
{
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Задайте пароль, який ви хочете використовувати
            string correctPassword = "admin123";

            if (PasswordBox.Password == correctPassword)
            {
                DialogResult = true; // Повертаємо успішний результат
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильний пароль. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
