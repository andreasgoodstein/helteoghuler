using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;

public class UserLogic
{
    private readonly EventService _eventService;
    private readonly GameStateLogic _gameStateLogic;
    private readonly PlayerLogic _playerLogic;
    private readonly UserService _userService;

    private readonly Dictionary<string, User> userDictionary;

    public UserLogic(EventService eventService, GameStateLogic gameStateLogic, PlayerLogic playerLogic, UserService userService)
    {
        _eventService = eventService;
        _gameStateLogic = gameStateLogic;
        _playerLogic = playerLogic;
        _userService = userService;

        userDictionary = new Dictionary<string, User>();

        var allUsers = _userService.GetAsync().Result;

        if (allUsers.Count < 1)
        {
            CreateAdminUser();
        }

        allUsers.ForEach(user =>
        {
            userDictionary[user.LoginName] = user;
        });
    }

    public User AddUser(string loginName)
    {
        var newUser = new User
        {
            CreatedAt = DateTime.UtcNow,
            LoginName = loginName,
            IsAdmin = false,
            PlayerId = Guid.NewGuid(),
        };

        _userService.CreateAsync(newUser);

        userDictionary[loginName] = newUser;

        return newUser;
    }

    private void CreateAdminUser()
    {
        var adminUser = new User
        {
            CreatedAt = DateTime.UtcNow,
            LoginName = Guid.NewGuid().ToString(),
            IsAdmin = true,
            PlayerId = Guid.NewGuid(),
        };

        _userService.CreateAsync(adminUser);

        userDictionary[adminUser.LoginName] = adminUser;

        var newPlayerEvent = new NewPlayerEvent_V1
        {
            CreatedAt = DateTime.UtcNow,
            Player = _playerLogic.CreatePlayer(_gameStateLogic.Get(), adminUser.PlayerId, "The Castle", "The Marquee"),
        };

        _eventService.CreateAsync(newPlayerEvent);

        _gameStateLogic.UpdateGameState(newPlayerEvent);
    }

    public User? GetUser(string? loginName)
    {
        if (String.IsNullOrWhiteSpace(loginName))
        {
            return null;
        }

        if (!userDictionary.ContainsKey(loginName))
        {
            return null;
        }

        return userDictionary[loginName];
    }
}
