using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using DishesApplication;
using System.Configuration;

namespace DishesApplication.Tests
{
    [TestClass]
    public class ApplicationTests
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

        [TestMethod]
        public void TestAddToBasket()
        {
            // Arrange
            var basketForm = new BasketForm(new List<BasketItem>());
            var product = new Product
            {
                ProductName = "Test Product",
                ProductDescription = "Test Description",
                ProductPhoto = "/picture.png",
                ProductManufacturer = "Test Manufacturer",
                ProductCost = 100.00m
            };

            // Act
            basketForm.AddToBasket(product);

            // Assert
            Assert.AreEqual(1, basketForm.basketItems.Count);
            Assert.AreEqual("Test Product", basketForm.basketItems[0].ProductName);
        }

        [TestMethod]
        public void TestRemoveFromBasket()
        {
            // Arrange
            var basketItem = new BasketItem
            {
                ProductName = "Test Product",
                ProductDescription = "Test Description",
                ProductPhoto = "/picture.png",
                ProductManufacturer = "Test Manufacturer",
                ProductCost = 100.00m
            };
            var basketForm = new BasketForm(new List<BasketItem> { basketItem });

            // Act
            basketForm.RemoveFromBasket(basketItem);

            // Assert
            Assert.AreEqual(0, basketForm.basketItems.Count);
        }

        [TestMethod]
        public void TestUpdateTotalCost()
        {
            // Arrange
            var basketItems = new List<BasketItem>
            {
                new BasketItem { ProductCost = 100.00m },
                new BasketItem { ProductCost = 200.00m }
            };
            var basketForm = new BasketForm(basketItems);

            // Act
            basketForm.UpdateTotalCost();

            // Assert
            Assert.AreEqual("300,00", basketForm.EndCostOrderTextBlock.Text);
        }

        [TestMethod]
        public void TestUserAuthentication()
        {
            // Arrange
            var logInForm = new LogIn();
            string login = "testuser";
            string password = "testpassword";

            // Act
            bool result = logInForm.AuthenticateUser(login, password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAddProductToDatabase()
        {
            // Arrange
            var productAddForm = new ProductAddForm();
            productAddForm.ProductArticleNumberTextBox.Text = "12345";
            productAddForm.ProductNameTextBox.Text = "Test Product";
            productAddForm.ProductDescriptionTextBox.Text = "Test Description";
            productAddForm.ProductCategoryTextBox.Text = "Test Category";
            productAddForm.ProductManufacturerTextBox.Text = "Test Manufacturer";
            productAddForm.ProductCostTextBox.Text = "100.00";
            productAddForm.ProductQuantityInStockTextBox.Text = "10";
            productAddForm.productPhotoPath = "/picture.png";

            // Act
            bool result = productAddForm.AddProductToDatabaseAsync().Result;

            // Assert
            Assert.IsTrue(result);

            // Cleanup
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM [dbo].[Product] WHERE [ProductArticleNumber] = @ProductArticleNumber";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductArticleNumber", "12345");
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
