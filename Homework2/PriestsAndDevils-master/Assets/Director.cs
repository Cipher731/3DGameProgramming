using Interface;
using UnityEngine;

public class Director : object
{
    private static readonly Director Instance = new Director();

    private Director()
    {
    }

    public ISceneController CurrentSceneController { get; set; }

    public static Director GetInstance()
    {
        return Instance;
    }

    public int GetFps()
    {
        return Application.targetFrameRate;
    }

    public static void SetFps(int fps)
    {
        Application.targetFrameRate = fps;
    }
}