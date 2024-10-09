using Godot;
using System;

[GlobalClass]
public partial class CustomGameLoop : SceneTree
{
    private static CustomGameLoop _instance;
    private LevelManager _levelManager;
    // private SaveManager _saveManager;

    public CustomGameLoop()
    {
        if (_instance != null)
        {
            throw new Exception("CustomGameLoop instance already exists!");
        }
        _instance = this;

        _levelManager = LevelManager.Instance;
        this.Root.AddChild(_levelManager);
        // _saveManager = SaveManager.Instance;
        // this.Root.AddChild(_saveManager);
    }

    public static CustomGameLoop Get()
    {
        if (_instance == null)
        {
            throw new Exception("CustomGameLoop instance does not exist!");
        }
        return _instance;
    }

    public LevelManager GetLevelManager()
    {
        return _levelManager;
    }

    /* 
    public SaveManager GetSaveManager()
    {
        return _saveManager;
    } 
    */
}