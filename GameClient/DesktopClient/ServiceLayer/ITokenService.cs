using DesktopClient.ModelLayer;

namespace DesktopClient.ServiceLayer
{
    public interface ITokenService
    {
        Task<TokensModel> GetDesktopClientTokens(ApiAccountModel accountModel);
        Task<TokensModel> RefreshTokens(RefreshRequestModel refreshToken);
    }
}
