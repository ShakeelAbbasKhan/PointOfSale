namespace PointOfSale.Helper
{
    public class JWTSettings
    {
        public string JWT { get; } = "JWTSettings";
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string TokenExpiryTimeInHour { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
        public string Secret { get; set; }
    }
}
