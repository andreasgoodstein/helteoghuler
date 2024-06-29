using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;

public class UserLogic
{
    private readonly EventService _eventService;

    private readonly GameStateLogic _gameStateLogic;

    private readonly UserService _userService;

    private Dictionary<string, User> userDictionary;

    public UserLogic(GameStateLogic gameStateLogic, EventService eventService, UserService userService)
    {
        _gameStateLogic = gameStateLogic;
        _eventService = eventService;
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

    // TODO: Refactor to be event driven
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

    // TODO: Refactor to be event driven
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

        var newPlayerEvent = new NewPlayerEvent
        {
            CreatedAt = DateTime.UtcNow,
            Player = new Player
            {
                Id = adminUser.PlayerId,
                Inn = new Inn
                {
                    Chest = new Chest
                    {
                        Gold = 0,
                        Id = Guid.NewGuid(),
                    },
                    Id = Guid.NewGuid(),
                    Name = "Admin Inn",
                },
                Name = "Admin",
            },
        };

        _eventService.CreateAsync(newPlayerEvent);

        _gameStateLogic.UpdateGameState(newPlayerEvent);
    }

    public User? GetUser(string loginName)
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
