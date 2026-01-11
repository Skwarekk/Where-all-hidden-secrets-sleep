using System;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static event EventHandler OnSceneLoad;

    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        OnSceneLoad?.Invoke(null, EventArgs.Empty);
    }

    public static void LoadLoadingScene()
    {
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
