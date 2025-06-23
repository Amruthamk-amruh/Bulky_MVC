using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyWeb.Data;
using BulkyWeb.Models;
using BulkyWeb.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Repository
{
    public class ProductsRepository :Repository<Products>, IProductsRepository
    {
        private ApplicationDbContext _db;
        public ProductsRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(Products obj)
        {
            _db.Productss.Update(obj);
        }


    }


}
