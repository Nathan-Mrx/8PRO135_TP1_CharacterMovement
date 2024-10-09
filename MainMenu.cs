using Godot;

public partial class MainMenu : Control
{

	private void _on_start_button_button_up()
	{
		CustomGameLoop.Get().GetLevelManager().LoadLevel("res://main_game.tscn");
	}

	private void _on_quit_button_button_up()
	{
		GetTree().Quit();
	}

	private void _on_load_button_button_up()
	{
		CustomGameLoop.Get().GetLevelManager().LoadLevel("res://main_game.tscn");
		CustomGameLoop.Get().GetSaveManager().LoadGame("res://save.json");
	}

}
