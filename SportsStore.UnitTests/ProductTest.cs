using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.WebUI;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
   public class ProductTest
    {
       public void Can_Pagigt()
       {
           Mock<IProductRepository> mkc = new Mock<IProductRepository>();
            mkc.Setup(m=>m.Products).Returns(new Product[]
                {   new Product {ProductID=1,Name="S1"},
                    new Product {ProductID=2,Name="S2"},
                    new Product {ProductID=3,Name="S3"},
                    new Product {ProductID=4,Name="S4"},
                    new Product {ProductID=5,Name="S5"},
                }.AsQueryable());

            ProductController cnt = new ProductController(mkc.Object);

            cnt.PageSize = 3;

            
                    
              }
    }
}
