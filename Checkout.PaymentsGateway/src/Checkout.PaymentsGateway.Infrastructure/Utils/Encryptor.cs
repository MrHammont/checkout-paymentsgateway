using Microsoft.AspNetCore.DataProtection;

namespace Checkout.PaymentsGateway.Infrastructure.Utils
{
    public class Encryptor : IEncryptor
    {
        private readonly IDataProtectionProvider _provider;
        private const string Purpose = "SqlDataProtection";

        public Encryptor(IDataProtectionProvider provider)
        {
            _provider = provider;
        }

        public string Encrypt(string data)
        {
            var protector = _provider.CreateProtector(Purpose);
            return protector.Protect(data);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _provider.CreateProtector(Purpose);
            return protector.Unprotect(cipherText);
        }
    }
}