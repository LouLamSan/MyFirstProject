using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api.Dtos
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public ICollection<Material> Materials { get; set; }
    }
}
