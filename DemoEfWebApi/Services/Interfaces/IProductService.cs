using System.Threading.Tasks;
using DemoEfWebApi.Models.Dto;

namespace DemoEfWebApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
    }
}
