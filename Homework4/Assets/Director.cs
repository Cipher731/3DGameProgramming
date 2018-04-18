using Interface;
using UnityEngine;

public class Director : object
{
    public static readonly Director Instance = new Director();

    private Director()
    {
    }

    public void SetFps(int fps)
    {
        Application.targetFrameRate = fps;
    }
    public ISceneController CurrentSceneController;
}