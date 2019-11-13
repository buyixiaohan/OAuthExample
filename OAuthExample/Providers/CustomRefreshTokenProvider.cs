using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.Owin.Security.Infrastructure;
using OAuthExample.Entities;
using OAuthExample.Infrastructure;

namespace OAuthExample.Providers
{
    public class CustomRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private ILog logger = LogManager.GetLogger(typeof(CustomRefreshTokenProvider));

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }
            //为刷新令牌生成一个唯一的标识符，这里我们使用Guid，也可以自己单独写一个字符串生成的算法
            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (AuthRepository _repo = new AuthRepository())
            {
                //从Owin上下文中读取令牌生存时间，并将生存时间设置到刷新令牌
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };
                //为刷新令牌设置有效期
                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
                //负责对票证内容进行序列化，稍后我们将次序列化字符串持久化到数据
                token.ProtectedTicket = context.SerializeTicket();
                var result = await _repo.AddRefreshToken(token);

                //在响应中文中发送刷新令牌Id
                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Receives the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //设置跨域访问
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            //获取到刷新令牌，hash后在数据库查找是否已经存在
            string hashedTokenId = Helper.GetHash(context.Token);
            using (AuthRepository _repo = new AuthRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);


                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    //删除当前刷新令牌,然后再次生成新令牌保存到数据库
                    var result = await _repo.RemoveRefreshToken(hashedTokenId);
                }
            }
        }
    }
}