using System.Windows;

namespace composition
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Головне вікно програми
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
