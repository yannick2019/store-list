using StoreList.Infrastructure.Identity;

namespace StoreList.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Succeeded, string Token, IEnumerable<string> Errors)> RegisterAsync(AddOrUpdateAppUserModel model);
        Task<(bool Succeeded, string Token, IEnumerable<string> Errors)> LoginAsync(LoginModel model);
    }
}
