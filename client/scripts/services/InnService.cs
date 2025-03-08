using System.Threading.Tasks;
using Godot;
using HelteOgHulerShared.Models;

namespace HelteOgHulerClient.Services;

public class InnService : BaseService
{
    const string RECRUIT_HERO_URL = "Inn/RecruitHero";
    const string UPGRADE_INN_URL = "Inn/UpgradeInn";

    public Task RecruitHero(Node httpRequestParent, string heroId)
    {
        var taskSource = new TaskCompletionSource<bool>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not recruit Hero");
            taskSource.SetResult(false);
        });

        requestNode.SetResponseHandler(body =>
        {
            ResponseHandler.HandleGameStateResponse<Recruitment>(body);
            taskSource.SetResult(true);
        });

        requestNode.ExecuteRequest($"{RECRUIT_HERO_URL}?heroId={heroId}");

        return taskSource.Task;
    }

    public Task<InnUpgrade> BuildInnUpgrade(Node httpRequestParent, InnUpgradeName upgrade)
    {
        var taskSource = new TaskCompletionSource<InnUpgrade>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not build Upgrade");
            taskSource.SetResult(null);
        });

        requestNode.SetResponseHandler(body =>
        {
            var innUpgrade = ResponseHandler.HandleGameStateResponse<InnUpgrade>(body);
            taskSource.SetResult(innUpgrade);
        });

        requestNode.ExecuteRequest($"{UPGRADE_INN_URL}?upgrade={upgrade}");

        return taskSource.Task;
    }
}
