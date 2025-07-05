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
            var objFromDb = _db.Productss.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.Category = obj.Category;
                objFromDb.Author = obj.Author;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
  
        }


    }


}
