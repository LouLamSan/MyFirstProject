using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Api.Dtos;

namespace WebApplication.Api.Services
{
    public class ProductService
    {
        public static ProductService Current { get; } = new ProductService();

        public List<Product> Products { get; }

        private ProductService()
        {
            Products = new List<Product>
            {
                new Product
                {
                    ID =1,
                    Name = "Coffee",
                    Price = 2.5f
                },

                new Product
                {
                    ID =2,
                    Name = "Milk",
                    Price = 3.0f
                },

                new Product
                {
                    ID=3,
                    Name = "Beer",
                    Price=5.5f
                }
            };
        }
    }
}
