namespace Authorization.Interfaces
{
    public interface ICryptography
    {
        string Hash(string message);
        bool VerifyHash(string hash, string message);
    }
}