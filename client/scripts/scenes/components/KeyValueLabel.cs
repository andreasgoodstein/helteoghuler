using Godot;
using System;

public class KeyValueLabel : HBoxContainer
{
	private Label Value;

	public override void _Ready()
	{
		Value = GetNode<Label>("Value");
	}

	public void Set(string value)
	{
		Value.Text = value;
	}

	public void Set(ulong? value)
	{
		Value.Text = value.ToString();
	}
}
