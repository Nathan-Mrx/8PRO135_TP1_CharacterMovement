using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if event escape is pressed, tree is paused and menu visible
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			if (Visible)
			{
				hide();
			}
			else
			{
				show();
			}
		}
	}

	public void show()
	{
		
		
		GetTree().Paused = true;
		Visible = true;
	}

	public void hide()
	{
		Visible = false;
		GetTree().Paused = false;
	}

	public void save_game()
	{
		CustomGameLoop.Get().GetSaveManager().SaveGame("res://save.json");
	}

	private void _on_save_button_button_up()
	{
		save_game();
		hide();
	}

	private void _on_resume_button_button_up()
	{
		hide();
	}

	private void _on_quit_button_button_up()
	{
		save_game();
		GetTree().Quit();
	}
}
