using Godot;

public class LoadingSpinnerScript : Control
{
	private Label SlowMessage;
	private Timer DisplaySlowMessageTimer;

	public override void _Ready()
	{
		SlowMessage = GetNode<Label>("%SlowMessage");
	}

	public new void Show()
	{
		System.Threading.Thread.Sleep(150);

		DisplaySlowMessageTimer = new Timer
		{
			Autostart = true,
			WaitTime = 3.0f
		};

		DisplaySlowMessageTimer.Connect("timeout", this, "DisplaySlowMessage");

		AddChild(DisplaySlowMessageTimer);

		base.Show();
	}

	private void DisplaySlowMessage()
	{
		SlowMessage.Show();
	}
}
