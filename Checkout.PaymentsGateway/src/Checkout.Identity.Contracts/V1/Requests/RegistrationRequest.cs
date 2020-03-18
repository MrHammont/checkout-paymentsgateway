using System.ComponentModel.DataAnnotations;

namespace Checkout.Identity.Contracts.V1.Requests
{
    public class RegistrationRequest
    {
        [EmailAddress] public string Email { get; set; }

        public string Password { get; set; }
    }
}