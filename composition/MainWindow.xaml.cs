using System.Windows;

namespace composition
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Закриття програми
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Вікно "Про програму"
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Програма управління складом для компанії TA-DA. Версія 1.0.",
                            "Про програму", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Відкриття вікна пошуку товарів
        private void SearchProduct_Click(object sender, RoutedEventArgs e)
        {
            search searchWindow = new search(); // Створюємо екземпляр вікна
            searchWindow.Show(); // Відкриваємо вікно
        }

        // Відкриття вікна перегляду залишків
        private void ViewStocks_Click(object sender, RoutedEventArgs e)
        {
            ViewStocks stocksWindow = new ViewStocks();  // Створюємо екземпляр вікна
            stocksWindow.Show();  // Відкриваємо вікно
        }

        // Відкриття вікна введення паролю
        private void AdminPanel_Click(object sender, RoutedEventArgs e)
        {
            // Створюємо вікно паролю при натисканні на кнопку
            PasswordWindow passwordWindow = new PasswordWindow();
            bool? result = passwordWindow.ShowDialog(); // Відкриваємо вікно паролю в модальному режимі

            if (result == true) // Якщо пароль правильний
            {
                admi adminWindow = new admi(); // Створюємо екземпляр вікна адміністрування
                adminWindow.Show(); // Відкриваємо вікно адміністрування
            }
        }
    }
}
