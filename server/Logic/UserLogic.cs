using HelteOgHulerServer.Services;

public class UserLogic
{
    UserService _userService;

    private Dictionary<string, User> userDictionary;

    public UserLogic(UserService userService)
    {
        _userService = userService;

        userDictionary = new Dictionary<string, User>();

        GetAllUsers().ForEach(user =>
        {
            userDictionary[user.LoginName] = user;
        });
    }

    public void AddUser(string loginName)
    {
        var newUser = new User
        {
            CreatedAt = DateTime.UtcNow,
            LoginName = loginName,
            PlayerId = new Guid()
        };

        _userService.CreateAsync(newUser);

        userDictionary[loginName] = newUser;
    }

    public List<User> GetAllUsers()
    {
        return _userService.GetAsync().Result;
    }

    public User GetUser(string loginName)
    {
        return userDictionary[loginName];
    }
}
