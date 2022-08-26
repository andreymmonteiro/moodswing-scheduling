using Microsoft.IdentityModel.Tokens;

namespace Moodswing.Domain.Models.Authentication
{
    /// <summary>
    /// Cryto key configuration
    /// </summary>
    public interface ISigningConfigurations
    {
        SecurityKey Key { get; set; }
        SigningCredentials SigningCredentials { get; set; }
    }
}
