namespace OAuthExample.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using OAuthExample.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<OAuthExample.Entities.OAuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OAuthExample.Entities.OAuthContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var clients = new List<Client> {
                new Client{
                    Id="sswyg14om2l8hv7ki9",
                    Secret="kUt4cRHQd7oYSsKmCFL1",
                    Name="IOS Native",
                    ApplicationType=ApplicationTypes.NativeConfidential,
                    AllowedOrigin="*",
                    RefreshTokenLifeTime=14400,
                    Active=true
                },
                new Client{
                    Id="ssuzje6a1h08csm4ok",
                    Secret="adb0e6c4%71uw@jp(r3&",
                    Name="Android Native",
                    ApplicationType=ApplicationTypes.NativeConfidential,
                    AllowedOrigin="*",
                    RefreshTokenLifeTime=14400,

                    Active=true
                },
                new Client{
                    Id="ssyxn2sg0bulrcdzme",
                    Secret="28cgs*pykv(53mb1x&fa",
                    Name="Hybrid App",
                    ApplicationType=ApplicationTypes.JavaScript,
                    AllowedOrigin="*",
                    RefreshTokenLifeTime=14400,
                    Active=true
                },
                new Client{
                    Id="ss8ak5ihd6n0jloyfb",
                    Secret="1x)^fvltc3&og2i(h%7w",
                    Name="H5 App",
                    ApplicationType=ApplicationTypes.JavaScript,
                    AllowedOrigin="*",
                    RefreshTokenLifeTime=14400,
                    Active=true
                },
                new Client{
                    Id="sstx2euq9nmy4vbj1c",
                    Secret="g*kc&hu#^)tjsr%qz2pv",
                    Name="Wx App",
                    ApplicationType=ApplicationTypes.JavaScript,
                    AllowedOrigin="*",
                    RefreshTokenLifeTime=14400,
                    Active=true
                }
            };
            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }
}
