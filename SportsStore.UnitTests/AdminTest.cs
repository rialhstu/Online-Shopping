using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]

        public void Index_Contains_All_Products()
        {
            // Arrange - create the mock repository 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                new Product {ProductID = 1, Name = "P1"}, 
                new Product {ProductID = 2, Name = "P2"}, 
                new Product {ProductID = 3, Name = "P3"}, 
            }.AsQueryable());

            AdminController ad = new AdminController(mock.Object);

            Product[] result = ((IEnumerable<Product>)ad.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0].Name, "P1");
            Assert.AreEqual(result[1].Name, "P2");
            Assert.AreEqual(result[2].Name, "P3");
        }
        [TestMethod]
        public void Edit_All_Products()
        {
            // Arrange - create the mock repository 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                new Product {ProductID = 1, Name = "P1"}, 
                new Product {ProductID = 2, Name = "P2"}, 
                new Product {ProductID = 3, Name = "P3"}, 
            }.AsQueryable());

            AdminController ad = new AdminController(mock.Object);

            Product f1 = ad.Edit(1).ViewData.Model as Product;
            Product f2 = ad.Edit(2).ViewData.Model as Product;
            Product f3 = ad.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(f1.ProductID, 1);
        }
    }
}