using GameClientApi.Models;

namespace GameClientApi.DatabaseAccessors;

public interface IAdminDatabaseAccessor
{
        string? GetPassword(int adminId);
        AdminModel GetAdmin(int adminId);
}
