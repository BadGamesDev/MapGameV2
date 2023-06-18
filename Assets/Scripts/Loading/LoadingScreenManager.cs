using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public Slider progressBar;
    public string mainGameSceneName = "Game";
    public float minLoadingTime = 3f;

    private bool isLoadingComplete = false;
    private float loadingStartTime;

    private void Start()
    {
        loadingStartTime = Time.time;
        StartCoroutine(LoadMainGameSceneAsync());
    }

    private IEnumerator LoadMainGameSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainGameSceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (progress >= 1f && !isLoadingComplete)
            {
                if (Time.time - loadingStartTime >= minLoadingTime)
                {
                    isLoadingComplete = true;
                    asyncLoad.allowSceneActivation = true;
                }
            }

            progressBar.value = progress;

            yield return null;
        }
    }
}