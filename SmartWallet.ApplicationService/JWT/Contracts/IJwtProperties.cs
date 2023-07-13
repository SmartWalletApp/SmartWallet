namespace SmartWallet.ApplicationService.JWT.Contracts
{
    public interface IJwtProperties
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}