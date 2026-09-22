using Dapper;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace FootyBlog.Infrastructure.Identity;

public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>
{ 
    private readonly string? _connectionString;

    public UserStore(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = " INSERT INTO Users (Id, UserName, NormalizedUserName, PasswordHash, RoleId) VALUES (@Id, @UserName, @NormalizedUserName, @PasswordHash, @RoleId)";

        await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.UserName,
            user.NormalizedUserName,
            user.PasswordHash,
            RoleId = "2"
        });

        return IdentityResult.Success;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) 
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = " SELECT Id, UserName, NormalizedUserName, PasswordHash, RoleId FROM Users WHERE NormalizedUserName = @NormalizedUserName";

        return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(
            sql,
            new { NormalizedUserName = normalizedUserName });
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = " SELECT Id, UserName, NormalizedUserName, PasswordHash, RoleId FROM Users WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(
            sql,
            new { Id = userId });
    }
     
    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)      
    {
        return Task.FromResult(user.Id);
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser user, string? normalizeName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizeName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash != null);
    }

    public void Dispose()
    {
    }

    public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = " SELECT Name FROM Roles WHERE Id =@RoleId";

        var role = connection.QueryFirstOrDefault<string>(
            sql, new { RoleId = user.RoleId });

        return Task.FromResult<IList<string>>(new List<string> { role });
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
