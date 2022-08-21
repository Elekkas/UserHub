using Authorization.DataTransferObjects;

namespace Authorization.Interfaces
{
    public interface IAccessToken
    {
        string CreateAccessToken(UserDTO user);
        string GetName(string token);
        string GetRole(string token);
        bool IsTokenValid(string token);
        string GetEmail(string token);
    }
}