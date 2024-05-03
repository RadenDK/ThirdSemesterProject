using GameClientApi.Models;

namespace GameClientApi.DatabaseAccessors;

public interface IAdminDatabaseAccessor
{
        string? GetPassword(int adminId);
        AdminLoginModel GetAdmin(int adminId);
}
