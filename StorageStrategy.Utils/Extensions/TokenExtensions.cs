using System.Security.Claims;

namespace StorageStrategy.Utils.Extensions
{
    public static class TokenExtensions
    {
        public static int GetCompanyId(this ClaimsPrincipal user)
        {
            Claim companyClaim = user.Claims.FirstOrDefault(p => p.Type == "CompanyId");
            if(companyClaim == null)
                throw new Exception("Id empresa não encontrado no token");
                
            return int.Parse(companyClaim.Value);
        }
    }
}