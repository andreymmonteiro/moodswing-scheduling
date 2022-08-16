using Microsoft.IdentityModel.Tokens;
using Moodswing.Domain.Models.Authentication;
using System.Text;

namespace Moodswing.Service.Authentication
{
    public class SigningConfigurations : ISigningConfigurations
    {
        private const string SECRET = "privateKeyLittleTalks";

        public SecurityKey Key { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfigurations()
        {
            var provider = Encoding.ASCII.GetBytes(SECRET);
            Key = new SymmetricSecurityKey(provider);

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
