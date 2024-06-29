using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerClient.Services;

public static class ResponseHandler
{
    public static void HandleGameStateResponse(byte[] body, RequestNode usedNode)
    {
        GameState newGameState = HHJsonSerializer.Deserialize<GameState>(body);

        GlobalGameState.Update(newGameState);

        usedNode.Clean();
    }

    public static void HandleGameStateResponse<T>(byte[] body, RequestNode usedNode) where T : IApplicable
    {
        T responseObject = HHJsonSerializer.Deserialize<T>(body);

        GlobalGameState.Update(responseObject);

        usedNode.Clean();
    }
}
