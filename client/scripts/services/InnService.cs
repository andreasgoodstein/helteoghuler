using Godot;
using HelteOgHulerShared.Models;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class InnService : BaseService
{
    const string RECRUIT_HERO_URL = "Inn/RecruitHero";

    public Task RecruitHero(Node httpRequestParent, string heroId)
    {
        var taskSource = new TaskCompletionSource<bool>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not recruit Hero");
            taskSource.SetResult(false);

            Clean();
        });

        requestNode.SetResponseHandler((byte[] body) =>
        {
            ResponseHandler.HandleGameStateResponse<Recruitment>(body);
            taskSource.SetResult(true);

            Clean();
        });

        requestNode.ExecuteRequest($"{RECRUIT_HERO_URL}?heroId={heroId}");

        return taskSource.Task;
    }
}
