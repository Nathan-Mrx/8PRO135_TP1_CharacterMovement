using Godot;
using System;
using System.IO;
using FileAccess = Godot.FileAccess;

public partial class SaveManager : Node
{

	private Node _rootNode;

	private static SaveManager _instance;
	public static SaveManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SaveManager(CustomGameLoop.Get().Root);
			}
			return _instance;
		}
	}

	public SaveManager(Node rootNode)
	{
		_rootNode = rootNode;
	}

	private Node FindNodeByName(Node root, string name)
	{
		if (root.Name == name)
			return root;

		foreach (Node child in root.GetChildren())
		{
			var result = FindNodeByName(child, name);
			if (result != null)
				return result;
		}

		return null;
	}
	
	public void LoadGame(string filePath)
	{
		if (!FileAccess.FileExists(filePath))
		{
			return;
		}
		

		var saveNodes = CustomGameLoop.Get().GetNodesInGroup("Persist");
		foreach (Node saveNode in saveNodes)
		{
			saveNode.QueueFree();
		}

	
		using var saveFile = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);

		while (saveFile.GetPosition() < saveFile.GetLength())
		{
			var jsonString = saveFile.GetLine();

			var json = new Json();
			var parseResult = json.Parse(jsonString);
			if (parseResult != Error.Ok)
			{
				GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
				continue;
			}

			// Get the data from the JSON object.
			var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

			// Firstly, we need to create the object and add it to the tree and set its position.
			var newObjectScene = GD.Load<PackedScene>(nodeData["Filename"].ToString());
			var newObject = newObjectScene.Instantiate<Node>();
			GetTree().Root.GetNode(nodeData["Parent"].ToString()).AddChild(newObject);
			newObject.Set(Node2D.PropertyName.Position, new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));

		
		}
	}

	public void SaveGame(string filePath)
	{
		using var saveFile = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);

		var saveNodes = CustomGameLoop.Get().GetNodesInGroup("Persist");
		GD.Print($"Found {saveNodes.Count} nodes to save");
		foreach (Node saveNode in saveNodes)
		{
			GD.Print($"Saving {saveNode.Name}");
			// Check the node is an instanced scene so it can be instanced again during load.
			if (string.IsNullOrEmpty(saveNode.SceneFilePath))
			{
				GD.Print($"persistent node '{saveNode.Name}' is not an instanced scene, skipped");
				continue;
			}

			// Check the node has a save function.
			if (!saveNode.HasMethod("Save"))
			{
				GD.Print($"persistent node '{saveNode.Name}' is missing a Save() function, skipped");
				continue;
			}

			// Call the node's save function.
			var nodeData = saveNode.Call("Save");

			// Json provides a static method to serialized JSON string.
			var jsonString = Json.Stringify(nodeData);

			// Store the save dictionary as a new line in the save file.
			saveFile.StoreLine(jsonString);
		}
	}
}
