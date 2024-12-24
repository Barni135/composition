using NUnit.Framework;

namespace composition.Tests
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void ProductConstructor_ShouldInitializeCorrectly()
        {
            // Arrange
            int id = 1;
            string title = "Test Product";
            string category = "Electronics";
            decimal price = 99.99m;
            string image = "test.jpg";
            string description = "Test description";

            // Act
            var product = new global::Product
            {
                Id = id,
                Title = title,
                CategoryID = category,
                Price = price,
                Image = image,
                Description = description
            };

            // Assert
            Assert.That(product.Id, Is.EqualTo(id));
            Assert.That(product.Title, Is.EqualTo(title));
            Assert.That(product.CategoryID, Is.EqualTo(category));
            Assert.That(product.Price, Is.EqualTo(price));
            Assert.That(product.Image, Is.EqualTo(image));
            Assert.That(product.Description, Is.EqualTo(description));
        }

        [Test]
        public void ProductDefaultConstructor_ShouldHaveDefaultValues()
        {
            // Act
            var product = new global::Product();

            // Assert
            Assert.That(product.Id, Is.EqualTo(0));
            Assert.That(product.Title, Is.Null);
            Assert.That(product.CategoryID, Is.Null);
            Assert.That(product.Price, Is.EqualTo(0m));
            Assert.That(product.Image, Is.Null);
            Assert.That(product.Description, Is.Null);
        }
    }
}
