using Dapper;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace FootyBlog.Infrastructure.Identity;

public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
{ 
    private readonly string? _connectionString;

    public UserStore(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = " INSERT INTO Users (Id, UserName, NormalizedUserName, PasswordHash, RoleId, Email) VALUES (@Id, @UserName, @NormalizedUserName, @PasswordHash, @RoleId, @Email)";

        await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.UserName,
            user.NormalizedUserName,
            user.PasswordHash,
            user.Email,
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

        string sql = " SELECT Id, UserName, NormalizedUserName, PasswordHash, Email, RoleId FROM Users WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(
            sql,
            new { Id = userId });
    }
     
    public  Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)      
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

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = " SELECT Name FROM Roles WHERE Id =@RoleId";

        var role = await connection.QueryFirstOrDefaultAsync<string>(
            sql, new { RoleId = user.RoleId });

        if (role == null)
        {
            return new List<string>();
        }

        return new List<string> { role };
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        var sql = "UPDATE Users SET Email = @Email WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new
        {
            Email = email,
            Id = user.Id
        });

        user.Email = email;
    }

    public async Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        String sql = " SELECT Email FROM Users WHERE Id = @Id";

        var email = await connection.QueryFirstOrDefaultAsync<string>(
            sql, new { Id = user.Id });

        return email;
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = " SELECT Id, UserName, NormalizedUserName, Email, PasswordHash, RoleId FROM Users WHERE Email = @Email";

        return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(
            sql,
            new { Email = normalizedEmail });
    }

    public  Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public  Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
