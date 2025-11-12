using System.Threading.Tasks;

namespace DemoEfWebApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task<bool> ValidateTokenAsync(string token);
    }
}
