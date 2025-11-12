using System;
//using System.Data.Objects;
using System.Linq;
using System.Threading.Tasks;
using DemoEfWebApi.Services.Interfaces;
using DemoEfWebApi.Models.Dto;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace DemoEfWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly Func<EFEntities> _contextFactory;

        // Use factory so it's easier to unit test and create/dispose contexts per call
        public ProductService(Func<EFEntities> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            using (var ctx = _contextFactory())
            {
                // use ObjectQuery (Entity SQL) through ObjectContext
                var oc = (ctx as IObjectContextAdapter)?.ObjectContext ?? (ObjectContext)(object)ctx;
                // If generated context is ObjectContext-derived, you can also cast directly.

                // simpler: use ctx.Products (if generated as ObjectSet<Product>)
                var product = await Task.Run(() =>
                {
                    var q = ctx.Products.Where(p => p.Id == id);
                    return q.FirstOrDefault();
                });

                if (product == null) return null;

                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                };
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            using (var ctx = _contextFactory())
            {
                var prod = ctx.Products.SingleOrDefault(p => p.Id == id);
                if (prod == null) return false;

                ctx.Products.Remove(prod); // ObjectContext/EntityObject model
                ctx.SaveChanges();
                return true;
            }
        }
    }
}