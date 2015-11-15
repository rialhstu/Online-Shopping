﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests3
    {
        [TestMethod]
        public void Add_New_Cart_Lines()
        {
            Product p1 = new Product { ProductID = 1, Name = "Apple" };
            Product p2 = new Product { ProductID = 2, Name = "Orange" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }
        [TestMethod]
        public void Add_New_Quantity()
        {
            Product p1 = new Product { ProductID = 1, Name = "Orrange" };
            Product p2 = new Product { ProductID = 1, Name = "Apple" };
            Product p3 = new Product { ProductID = 2, Name = "Orange" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            Assert.AreEqual(results[0].Quantity, 1);
            Assert.AreEqual(results[1].Quantity, 10);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {

            // Arrange - create some test products 
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            // Arrange - create a new cart 
            Cart target = new Cart();
            // Arrange - add some products to the cart 
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Act 
            target.RemoveLine(p2);
            Assert.AreEqual(target.Lines.Where(p => p.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {

            // Arrange - create some test products 
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Arrange - create a new cart 
            Cart target = new Cart();

            // Act 
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            Assert.AreEqual(result, 450M);

        }

        [TestMethod]
        public void Can_Clear_Contents()
        {

            // Arrange - create some test products 
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Arrange - create a new cart 
            Cart target = new Cart();

            // Arrange - add some items 
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Act - reset the cart 
            target.Clear();

            // Assert 
            Assert.AreEqual(target.Lines.Count(), 0);
        } 
    }
}
