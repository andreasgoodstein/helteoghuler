using Godot;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class PlayerService
{
    const string NEW_PLAYER_URL = "http://localhost:7111/Player/New";

    private RequestNode startAdventureNode;

    public Task CreateNewPlayer(Node httpRequestParent, string playerName, string innName)
    {
        var taskSource = new TaskCompletionSource<bool>();

        startAdventureNode?.Clean();
        startAdventureNode = new RequestNode(httpRequestParent, ResponseType.JSONCallback);

        startAdventureNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not start Adventure");
            taskSource.SetResult(false);

            startAdventureNode?.Clean();
            startAdventureNode = null;
        });

        startAdventureNode.SetResponseHandler((byte[] body) =>
        {
            ResponseHandler.HandleGameStateResponse(body);
            taskSource.SetResult(true);

            startAdventureNode?.Clean();
            startAdventureNode = null;
        });

        startAdventureNode.ExecuteRequest(NEW_PLAYER_URL + $"?playerName={playerName}&innName={innName}");

        return taskSource.Task;
    }
}
