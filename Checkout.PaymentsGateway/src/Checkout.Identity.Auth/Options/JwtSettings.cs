using System;

namespace Checkout.Identity.Auth.Options
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifeTime { get; set; }
        public TimeSpan RefreshTokenLifeTime { get; set; }
    }
}