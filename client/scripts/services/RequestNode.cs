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
	private readonly string SERVER_URL = OS.IsDebugBuild()
		? "http://helteoghuler.test:7111/"
		: "https://helteoghulerapi.andreasgoodstein.com/";

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
		var encodedUrl = $"{SERVER_URL}{Uri.EscapeUriString(path)}";
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
