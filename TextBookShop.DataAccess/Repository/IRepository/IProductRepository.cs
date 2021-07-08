using TextBookShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TextBookShop.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
