using System;
using Godot;

namespace HelteOgHulerClient.Services;

public enum ResponseType
{
	JSONCallback = 0,
	StringCallback = 1,
}

public class RequestNode
{
	const bool USE_PROD = false;
	const string SERVER_URL = "http://localhost:7111/";
	const string PROD_SERVER_URL = "https://helteoghulerapi.andreasgoodstein.com/";
	public string[] Headers = ["HHLoginName: "];

	private Node _parent;
	private HTTPRequest Request { get; set; }
	private ResponseWrapper Response { get; set; }

	public RequestNode(Node parent, ResponseType type)
	{
		Headers[0] = Headers[0] + parent.GetNode<Settings>("/root/Settings").LoginName;

		Request = new HTTPRequest();
		Response = new ResponseWrapper();

		var callbackName = type == ResponseType.JSONCallback ? "JSONCallback" : "StringCallback";

		Request.Connect("request_completed", Response, callbackName);

		_parent = parent;
		_parent.AddChild(Request);
		Request.AddChild(Response);
	}

	public void Clean()
	{
		Request?.CancelRequest();
		Request?.RemoveChild(Response);
		_parent?.RemoveChild(Request);

		Request = null;
		Response = null;
		_parent = null;
	}

	public void ExecuteRequest(string path)
	{
		var serverUrl = USE_PROD ? PROD_SERVER_URL : SERVER_URL;
		var encodedUrl = $"{serverUrl}{Uri.EscapeUriString(path)}";
		Request.Request(encodedUrl, Headers);
	}

	public void SetErrorHandler(Action handler)
	{
		Response.ErrorDelegate = handler;
	}
	public void SetResponseHandler(Action<byte[]> handler)
	{
		Response.JSONCallbackDelegate = handler;
	}
	public void SetResponseHandler(Action<string> handler)
	{
		Response.TextCallbackDelegate = handler;
	}
}
