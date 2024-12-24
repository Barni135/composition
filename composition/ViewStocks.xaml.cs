using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace composition
{
    public partial class ViewStocks : Window
    {
        private SQLiteConnection conn;
        private SQLiteCommand cmd;

        public ViewStocks()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadStocks();
        }

        // Підключення до бази даних
        private void InitializeDatabase()
        {
            string connectionString = @"Data Source=C:\Users\Barni\Desktop\data.db;Version=3;";
            conn = new SQLiteConnection(connectionString);
            conn.Open();
        }

        // Завантаження залишків товарів
        private void LoadStocks()
        {
            try
            {
                string query = @"
                    SELECT p.ProductCode, p.ProductName, c.CategoryName AS Category, p.Quantity 
                    FROM Products p
                    JOIN Categories c ON p.CategoryID = c.CategoryID";

                cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                StocksGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження товарів: " + ex.Message);
            }
        }
    }
}
