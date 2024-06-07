using Godot;
using System;

public class TopMenuScript : HBoxContainer
{
	public override void _Ready()
	{
		GetNode<Button>("AdventureButton").Connect("pressed", this, "GoToAdventure");
		GetNode<Button>("InnButton").Connect("pressed", this, "GoToInn");
		GetNode<Button>("WorldButton").Connect("pressed", this, "GoToWorld");
	}

	private void GoToAdventure()
	{
		GetTree().ChangeScene("res://scenes/AdventureScene.tscn");
	}

	private void GoToInn()
	{
		GetTree().ChangeScene("res://scenes/InnScene.tscn");
	}

	private void GoToWorld()
	{
		GetTree().ChangeScene("res://scenes/WorldScene.tscn");
	}
}
