using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;

    public static LoadingManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    private static LoadingManager Create()
    {
        return Instantiate(Resources.Load<LoadingManager>("LoadingUI"));
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Slider loadBar;

    private int loadSceneIndex;

    public void LoadScene(int sceneIndex)
    {
        gameObject.SetActive(true);
        loadSceneIndex = sceneIndex;
        StopAllCoroutines();
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        loadBar.value = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneIndex);

        while (!op.isDone)
        {
            loadBar.value = Mathf.Clamp01(op.progress / 0.9f);
            yield return null;
        }

        yield return StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        float startAlpha = isFadeIn ? 0f : 1f;
        float targetAlpha = isFadeIn ? 1f : 0f;

        canvasGroup.alpha = startAlpha;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
