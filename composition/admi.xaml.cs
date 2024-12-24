using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace composition
{
    public partial class admi : Window
    {
        private SQLiteConnection conn;
        private SQLiteCommand cmd;
        private ObservableCollection<global::Product> products;

        public admi()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadCategories();
            LoadProducts();
        }

        // Подключение к базе данных
        private void InitializeDatabase()
        {
            string connectionString = @"Data Source=C:\Users\Barni\Desktop\data.db;Version=3;";
            conn = new SQLiteConnection(connectionString);
            conn.Open();
        }

        // Загрузка категорий товаров в ComboBox
        private void LoadCategories()
        {
            try
            {
                string query = "SELECT CategoryName FROM Categories";
                cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = reader["CategoryName"].ToString();
                    CategoryBox.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message);
            }
        }

        // Загрузка товаров в DataGrid
        // Загрузка товаров в DataGrid
        private void LoadProducts()
        {
            products = new ObservableCollection<global::Product>();

            try
            {
                string query = "SELECT Products.ProductID, ProductCode, ProductName, CategoryName, Quantity " +
                               "FROM Products JOIN Categories ON Products.CategoryID = Categories.CategoryID";
                cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Создаем объект Product с использованием конструктора без параметров
                    global::Product product = new global::Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductCode = reader["ProductCode"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryID = reader["CategoryName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };

                    products.Add(product);  // Добавляем объект Product в ObservableCollection
                }

                ProductsGrid.ItemsSource = products;  // Привязываем к DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки товаров: " + ex.Message);
            }
        }



        // Добавление товара
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    // Запрос для добавления товара в базу данных
                    string query = "INSERT INTO Products (ProductCode, ProductName, CategoryID, Quantity) " +
                                   "VALUES (@code, @name, (SELECT CategoryID FROM Categories WHERE CategoryName = @category), @quantity)";

                    // Выполнение запроса
                    cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@code", ProductCodeBox.Text);
                    cmd.Parameters.AddWithValue("@name", ProductNameBox.Text);
                    cmd.Parameters.AddWithValue("@category", ((ComboBoxItem)CategoryBox.SelectedItem).Content.ToString());
                    cmd.Parameters.AddWithValue("@quantity", int.Parse(QuantityBox.Text));
                    cmd.ExecuteNonQuery();

                    // Отображение сообщения об успешном добавлении
                    MessageBox.Show("Товар успешно добавлен!");

                    // Перезагрузка товаров в DataGrid, чтобы увидеть добавленный товар
                    LoadProducts();

                    // Очистка полей ввода
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления товара: " + ex.Message);
                }
            }
        }

        // Редактирование товара
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem != null && ValidateInputs())
            {
                try
                {
                    global::Product selectedProduct = (global::Product)ProductsGrid.SelectedItem;

                    string query = "UPDATE Products SET ProductCode=@code, ProductName=@name, " +
                                   "CategoryID=(SELECT CategoryID FROM Categories WHERE CategoryName=@category), " +
                                   "Quantity=@quantity WHERE ProductID=@id";

                    cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@code", ProductCodeBox.Text);
                    cmd.Parameters.AddWithValue("@name", ProductNameBox.Text);
                    cmd.Parameters.AddWithValue("@category", ((ComboBoxItem)CategoryBox.SelectedItem).Content.ToString());
                    cmd.Parameters.AddWithValue("@quantity", int.Parse(QuantityBox.Text));
                    cmd.Parameters.AddWithValue("@id", selectedProduct.ProductID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Товар успешно обновлен!");
                    LoadProducts();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования товара: " + ex.Message);
                }
            }
        }

        // Удаление товара
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem != null)
            {
                try
                {
                    global::Product selectedProduct = (global::Product)ProductsGrid.SelectedItem;

                    string query = "DELETE FROM Products WHERE ProductID=@id";
                    cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedProduct.ProductID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Товар успешно удален!");
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления товара: " + ex.Message);
                }
            }
        }

        // Проверка введенных данных
        private bool ValidateInputs()
        {
            return !string.IsNullOrWhiteSpace(ProductCodeBox.Text) &&
                   !string.IsNullOrWhiteSpace(ProductNameBox.Text) &&
                   CategoryBox.SelectedItem != null &&
                   int.TryParse(QuantityBox.Text, out _);
        }

        // Очищение полей ввода
        private void ClearInputs()
        {
            ProductCodeBox.Text = "";
            ProductNameBox.Text = "";
            CategoryBox.SelectedIndex = -1;
            QuantityBox.Text = "";
        }
    }
}
