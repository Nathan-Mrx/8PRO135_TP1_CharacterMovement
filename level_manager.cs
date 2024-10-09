using Godot;
using System;

public partial class LevelManager : Node
{
	private static LevelManager _instance;
	public static LevelManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LevelManager();
			}
			return _instance;
		}
	}

	private Node currentLevel;

	private LevelManager() { }

	public void LoadLevel(string path)
	{
		if (GetTree() == null)
		{
			GD.PrintErr("LevelManager is not added to the scene tree.");
			return;
		}

		if (currentLevel != null)
		{
			currentLevel.QueueFree();
		}
		var nextLevel = GD.Load<PackedScene>(path).Instantiate();
		GetTree().Root.AddChild(nextLevel);
		currentLevel = nextLevel;
	}
}
