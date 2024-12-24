using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace composition
{
    public partial class search : Window
    {
        private List<ProductEntity> Products = new List<ProductEntity>();
        private List<string> Categories = new List<string>();
        private SQLiteConnection conn;
        private DispatcherTimer searchTimer;

        public search()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadCategories();
            LoadProducts();
            ProductSearchBox.GotFocus += RemovePlaceholderText;
            ProductSearchBox.LostFocus += ShowPlaceholderText;

            // Ініціалізуємо таймер для пошуку
            searchTimer = new DispatcherTimer();
            searchTimer.Interval = TimeSpan.FromMilliseconds(500); // Затримка 500 мс після останнього вводу
            searchTimer.Tick += SearchTimer_Tick;
        }

        private void ShowPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductSearchBox.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void InitializeDatabase()
        {
            string connectionString = @"Data Source=C:\Users\Barni\Desktop\data.db;Version=3;";
            conn = new SQLiteConnection(connectionString);
            conn.Open();
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT DISTINCT CategoryName FROM Categories";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();

                Categories.Clear();
                Categories.Add("Усі категорії");

                while (reader.Read())
                {
                    Categories.Add(reader["CategoryName"].ToString());
                }

                CategoryFilter.ItemsSource = Categories;
                CategoryFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні категорій: " + ex.Message);
            }
        }

        private void LoadProducts()
        {
            try
            {
                string query = "SELECT Products.ProductID, ProductCode, ProductName, CategoryName, Quantity " +
                               "FROM Products JOIN Categories ON Products.CategoryID = Categories.CategoryID";

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();

                Products.Clear();

                while (reader.Read())
                {
                    Products.Add(new ProductEntity
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductCode = reader["ProductCode"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryID = reader["CategoryName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                SearchResultsGrid.ItemsSource = Products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження даних: " + ex.Message);
            }
        }

        // Метод пошуку
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        // Реалізація пошуку за текстом і категорією
        private void PerformSearch()
        {
            try
            {
                string searchQuery = ProductSearchBox.Text.Trim();
                string selectedCategory = CategoryFilter.SelectedItem.ToString();

                string query = "SELECT Products.ProductID, ProductCode, ProductName, CategoryName, Quantity " +
                               "FROM Products JOIN Categories ON Products.CategoryID = Categories.CategoryID " +
                               "WHERE (ProductName LIKE @searchQuery OR ProductCode LIKE @searchQuery)";

                if (selectedCategory != "Усі категорії")
                {
                    query += " AND CategoryName = @selectedCategory";
                }

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                if (selectedCategory != "Усі категорії")
                {
                    cmd.Parameters.AddWithValue("@selectedCategory", selectedCategory);
                }

                SQLiteDataReader reader = cmd.ExecuteReader();

                Products.Clear();

                while (reader.Read())
                {
                    Products.Add(new ProductEntity
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductCode = reader["ProductCode"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryID = reader["CategoryName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                // Оновлення джерела даних таблиці
                SearchResultsGrid.ItemsSource = null;
                SearchResultsGrid.ItemsSource = Products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка пошуку: " + ex.Message);
            }
        }

        // Обробник для події TextChanged
        private void ProductSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Скидаємо таймер на кожне введення тексту
            searchTimer.Stop();
            searchTimer.Start();
        }

        // Подія таймера для пошуку
        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            PerformSearch();
        }
    }
}
