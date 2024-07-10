namespace HelteOgHulerClient.Services;

public partial class BaseService
{
    public RequestNode requestNode;

    public void Clean(RequestNode optionalNewRequestNode = null)
    {
        requestNode?.Clean();
        requestNode = optionalNewRequestNode;
    }
}
