using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepositoryAsync : GenericRepositoryAsync<Produto>, IProductRepositoryAsync
    {
        private readonly DbSet<Produto> _products;

        public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _products = dbContext.Set<Produto>();
        }

        public Task<bool> IsUniqueBarcodeAsync(string CodigoDeBarras)
        {
            return _products
                .AllAsync(p => p.CodigoDeBarras != CodigoDeBarras);
        }
    }
}
