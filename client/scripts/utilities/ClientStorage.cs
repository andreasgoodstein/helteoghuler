using Godot;

namespace HelteOgHulerClient.Utilities;

public static class ClientStorage
{
	const string GetLoginNameCommand = "globalThis?.localStorage?.getItem('userGuid') ?? ''";
	const string SetLoginNameCommand = "globalThis?.localStorage?.setItem('userGuid','USER_GUID')";

	public static string GetLoginName()
	{
		var loginName = (string)JavaScript.Eval(GetLoginNameCommand);
		return loginName;
	}

	public static void SetLoginName(string loginName)
	{
		JavaScript.Eval(SetLoginNameCommand.Replace("USER_GUID", loginName));
	}
}
