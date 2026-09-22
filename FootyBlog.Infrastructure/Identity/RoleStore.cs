using Dapper;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace FootyBlog.Infrastructure.Identity;

public class RoleStore : IRoleStore<ApplicationRole>
{
    private readonly string? _connectionString;

    public RoleStore(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = "INSERT INTO ROLES(ID, NAME, NORMALIZEDNAME, CONCURRENCYSTAMP) VALUES(@Id, @Name, @NormalizedName, @ConcurrencyStamp)";

        await connection.ExecuteAsync(sql, role);

        return IdentityResult.Success;
    }

    public async Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = "SELECT ID, NAME, NORMALIZEDNAME, CONCURRENCYSTAMP FROM ROLES WHERE NORMALIZEDNAME = @NormalizedName";

        return await connection.QueryFirstOrDefaultAsync<ApplicationRole>(
            sql,
            new { NormalizedName = normalizedRoleName });
    }
    public async Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = "SELECT ID, NAME, NORMALIZEDNAME, CONCURRENCYSTAMP FROM ROLES WHERE ID = @Id"; 

        return await connection.QueryFirstOrDefaultAsync<ApplicationRole>(
            sql,
            new { Id = roleId });
    }

    public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken) 
    {
        return Task.FromResult(role.Id);
    }

    public Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public void Dispose()
    {
    }
}
 