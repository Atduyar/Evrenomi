using Microsoft.IdentityModel.Tokens;

namespace Core.Utilities.Security.Encyption
{
    public class SigningCredentialsHalper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);//http://www.w3.org/2001/04/xmldsig-more#hmac-sha256
            //return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);//http://www.w3.org/2001/04/xmldsig-more#hmac-sha256
        }
    }
}