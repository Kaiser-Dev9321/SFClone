using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAdditive(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    public AsyncOperation LoadSceneAsync(int sceneToLoad, LoadSceneMode loadSceneBy)
    {
        return SceneManager.LoadSceneAsync(sceneToLoad, loadSceneBy);
    }

    public AsyncOperation LoadSceneAsync(string sceneToLoad, LoadSceneMode loadSceneBy)
    {
        return SceneManager.LoadSceneAsync(sceneToLoad, loadSceneBy);
    }
}
