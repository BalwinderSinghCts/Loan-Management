using WebApi.Entities;

namespace WebApi.Repository
{
    public interface IUserService
    {
        bool UserExists(string email);
        User GetUser(string email);
    }
}
