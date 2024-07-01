using Godot;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class AdventureService
{
    const string START_ADVENTURE_URL = "http://localhost:7111/Adventure/Start";

    private RequestNode startAdventureNode;

    public Task StartAdventure(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<bool>();

        startAdventureNode?.Clean();
        startAdventureNode = new RequestNode(httpRequestParent, ResponseType.JSONCallback);

        startAdventureNode.SetResponseHandler((int result, int response_code, string[] headers, byte[] body) =>
        {
            // Unauthorized
            if (response_code == 401)
            {
                GD.PrintErr("Auth: Unauthorized");
                httpRequestParent.GetTree().ChangeScene("res://scenes/LoginMenuScene.tscn");
            }
            // Expected Errors
            else if (response_code == 400)
            {
                ResponseHandler.HandleGameStateResponse<HHError>(body, startAdventureNode);
                taskSource.SetResult(true);
            }
            // Unexpected Errors
            else if (response_code < 200 || response_code > 299)
            {
                GD.PrintErr("Network: Could not start Adventure");
                taskSource.SetResult(false);
            }
            // Success
            else
            {
                ResponseHandler.HandleGameStateResponse<Adventure>(body, startAdventureNode);
                taskSource.SetResult(true);
            }

            startAdventureNode?.Clean();
        });

        startAdventureNode.ExecuteRequest(START_ADVENTURE_URL);

        return taskSource.Task;
    }
}
