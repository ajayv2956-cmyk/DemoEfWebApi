using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using DemoEfWebApi.Services.Interfaces;

namespace DemoEfWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly Func<EFEntities> _contextFactory;
        private readonly TimeSpan _tokenLifetime = TimeSpan.FromHours(6);

        public AuthService(Func<EFEntities> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            using (var ctx = _contextFactory())
            {
                // For demo, password is plain text; in production compare hashed password
                var user = await ctx.Users.SingleOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
                if (user == null) return null;

                var tokenValue = Guid.NewGuid().ToString("N"); // simple token
                var expires = DateTime.UtcNow.Add(_tokenLifetime);

                var token = new Token
                {
                    UserId = user.Id,
                    TokenValue = tokenValue,
                    ExpiresAt = expires
                };

                ctx.Tokens.Add(token);
                await ctx.SaveChangesAsync();

                return tokenValue;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            using (var ctx = _contextFactory())
            {
                var t = ctx.Tokens.SingleOrDefaultAsync(x => x.TokenValue == token);
                //var t = await ctx.Tokens.SingleOrDefaultAsync(x => x.TokenValue == token);
                if (t == null) return false;
                if (t.Result.ExpiresAt < DateTime.UtcNow) return false;
                return true;
            }
        }
    }
}