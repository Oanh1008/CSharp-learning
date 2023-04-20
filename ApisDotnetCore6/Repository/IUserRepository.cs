using ApisDotnetCore6.Data;

namespace ApisDotnetCore6.Repository;

public interface IUserRepository 
{
    User CreateUser(User user);
    List<User> GetAllUser();
    User GetUserById(Guid guid);
}