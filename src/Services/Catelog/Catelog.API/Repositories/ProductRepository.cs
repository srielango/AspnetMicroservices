﻿using Catelog.API.Data;
using Catelog.API.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catelog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Product> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context.Products
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            var result = await _context.Products.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
