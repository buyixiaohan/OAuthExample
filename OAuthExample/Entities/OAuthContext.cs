using System.Data.Entity;

namespace OAuthExample.Entities
{
    public class OAuthContext : DbContext
    {
        public OAuthContext() : base("OAuthConnection")
        {
            //Database.SetInitializer<OAuthContext>(null);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}