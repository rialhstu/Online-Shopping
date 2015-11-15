using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {

            // Arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
        new Product {ProductID = 1, Name = "P1"}, 
        new Product {ProductID = 2, Name = "P2"}, 
        new Product {ProductID = 3, Name = "P3"}, 
        new Product {ProductID = 4, Name = "P4"}, 
        new Product {ProductID = 5, Name = "P5"} 
    }.AsQueryable());

            // create a controller and make the page size 3 items 
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act 
            ProductsListViewModel result
                = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert 
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");

        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange 
            // - create the mock repository 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
        new Product {ProductID = 1, Name = "P1", Category = "Cat1"}, 
        new Product {ProductID = 2, Name = "P2", Category = "Cat2"}, 
        new Product {ProductID = 3, Name = "P3", Category = "Cat1"}, 
        new Product {ProductID = 4, Name = "P4", Category = "Cat2"}, 
        new Product {ProductID = 5, Name = "P5", Category = "Cat3"} 
    }.AsQueryable());
            // Arrange - create a controller and make the page size 3 items 
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action 
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model)
                .Products.ToArray();




            // Assert 
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]

        public void Can_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
        new Product {ProductID = 1, Name = "P1", Category = "Orange"}, 
        new Product {ProductID = 2, Name = "P2", Category = "Apple"}, 
        new Product {ProductID = 3, Name = "P3", Category = "Judges"}
       
    }.AsQueryable());

            NavController target = new NavController(mock.Object);

            string[] result = ((IEnumerable<string>)target.Menu().Model).ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], "Apple");
            Assert.AreEqual(result[1], "Orange");
            Assert.AreEqual(result[1], "Judges");




        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange 
            // - create the mock repository 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
        new Product {ProductID = 1, Name = "P1", Category = "Apples"}, 
        new Product {ProductID = 4, Name = "P2", Category = "Oranges"}, 
    }.AsQueryable());
            // Arrange - create the controller  
            NavController target = new NavController(mock.Object);
            // Arrange - define the category to selected 
            string categoryToSelect = "Apples";
            // Action 
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

              


            // Assert 
            Assert.AreEqual(categoryToSelect, result);
        }
        [TestMethod]
        public void Can_Add_New_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            CartLine[] results = target.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);

        }
        [TestMethod]
        public void Can_Add_Existing_Quantity()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);

        }

        public void RemoveLine()
        {
            Product p1 = new Product { ProductID = 1, Name = "p1" };
            Product p2 = new Product { ProductID = 2, Name = "p2" };
            Product p3 = new Product { ProductID = 3, Name = "p3" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.AddItem(p3, 3);

            target.RemoveLine(p2);
            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        public void Calculate_Total_Cost()
        {
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 500M };
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 150M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.AddItem(p1, 4);

            decimal result = target.ComputeTotalValue();

            Assert.AreEqual(result, 2800M);
        }

        public void Can_Cleaer()
        {
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 500M };
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 150M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.AddItem(p1, 4);

            target.Clear();

            Assert.AreEqual(target.Lines.Count(), 0);
        }


        public void Add_To_Cart()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]

                {
                   new Product {ProductID=1, Name= "p1", Category="Apples"}
                }.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object);

            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);



        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // Arrange - create the mock repository 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                new Product {ProductID = 1, Name = "P1", Category = "Apples"}, 
            }.AsQueryable());

            // Arrange - create a Cart 
            Cart cart = new Cart();

            // Arrange - create the controller 
            CartController target = new CartController(mock.Object);

            RedirectToRouteResult result = target.AddToCart(cart, 2, "MyUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "MyUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {

            Cart cart = new Cart();

            CartController target = new CartController(null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "MyUrl").ViewData.Model;
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl,"MyUrl");
        }

    }
}
