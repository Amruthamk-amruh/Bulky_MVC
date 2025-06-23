using BulkyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWeb.Repository.IRepository
{
    public interface IProductsRepository : IRepository<Products>
    {
        void Update(Products obj);
        //void Save();
    }
}
