using composition;

public class Product
{
    private string category;

    public int Id { get; set; }
    public string Title { get; set; }
    public string CategoryID { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public int ProductID { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }

    // Конструктор по умолчанию
    public Product() { }

    // Конструктор с параметрами
    public class Producten
    {
        private string category;

        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryID { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int ProductIdentifier { get; set; } // Змінено назву
        public string ProductCodeValue { get; set; } // Змінено назву
        public string ProductDisplayName { get; set; } // Змінено назву

        // Конструктор за замовчуванням
        public Producten() { }

        // Конструктор з параметрами
        public Producten(int id, string title, string category, decimal price, string image, string description)
        {
            Id = id;
            Title = title;
            CategoryID = category;
            this.category = category;
            Price = price;
            Image = image;
            Description = description;
        }
    }
}