using Microsoft.AspNetCore.Identity;
using Noter.DAL.Context;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Noter.DAL.User
{
    public class NoterUserStore : IUserStore<NoterUser>, IUserPasswordStore<NoterUser>
    {
        private readonly NoterContext context;

        public NoterUserStore(NoterContext context)
        {
            this.context = context;
        }

        public async Task<IdentityResult> CreateAsync(NoterUser user, CancellationToken cancellationToken)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(NoterUser user, CancellationToken cancellationToken)
        {
            var userToDelete = this.context.Users.FirstOrDefault(x => x.Id == user.Id);
            context.Users.Remove(userToDelete);
            context.SaveChanges();
            return await Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        public async Task<NoterUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            return await Task.FromResult(user);
        }

        public async Task<NoterUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = context.Users.FirstOrDefault(x => x.NormalizedUserName == normalizedUserName);
            return await Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(NoterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(NoterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(NoterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(NoterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(NoterUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetNormalizedUserNameAsync(NoterUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(NoterUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(NoterUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(NoterUser user, CancellationToken cancellationToken)
        {
            var userToSet = this.context.Users.FirstOrDefault(x => x.Id == user.Id);
            userToSet.UserName = user.UserName;
            userToSet.NormalizedUserName = user.NormalizedUserName;
            userToSet.PasswordHash = user.PasswordHash;
            context.SaveChanges();
            return await Task.FromResult(IdentityResult.Success);
        }
    }
}
