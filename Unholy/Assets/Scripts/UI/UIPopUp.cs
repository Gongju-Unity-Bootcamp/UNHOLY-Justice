using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Data;

public class UIPopUp : MonoBehaviour
{
    [SerializeField] public CanvasGroup[] _canvasGroup;

    enum popUpType
    {
        GameOver = 0,
        GameClear = 1
    }

    [SerializeField] float DELAYTIME = 3f;

    public bool isFadeFinish;

    public void InitializeUI()
    {
        foreach(CanvasGroup cg in _canvasGroup)
        {
            cg.alpha = 0f;
        }
    }

    /// <summary>
    /// Player의 End 상태에 따라 그에 맞는 image를 반환하는 메소드입니다.
    /// </summary>
    /// <returns>End 상태에 알맞는 canvasgroup을 반환합니다.</returns>
    private CanvasGroup EndState()
    {
        CanvasGroup _canvas = null;

        if (CombatManager._currentPlayerHP <= 0)
        {
            _canvas = _canvasGroup[(int)popUpType.GameOver];
        }
        else if(CombatManager._currentBossHP <= 0)
        {
            _canvas = _canvasGroup[(int)popUpType.GameClear];
        }

        return _canvas;
    }

    /// <summary>
    /// 게임을 끝내는 동작을 하는 메소드입니다.
    /// </summary>
    /// <param name="canvas">특정 이벤트에 해당되는 canvas입니다.</param>
    public void EndProcess()
    {
        CanvasGroup _canvas = EndState();

        if (isFadeFinish) { 
            SceneManager.LoadScene((int)SceneIndex.Title);
        }

        if(_canvas != null)
            StartCoroutine(FadeIn(_canvas, DELAYTIME));
    }

    /// <summary>
    /// 해당 canvas가 서서히 나타나게 해주는 IEnumerator 입니다.
    /// </summary>
    /// <param name="canvas">특정 이벤트에 해당되는 canvas group</param>
    /// <param name="duration">canvas가 fadein 되는 시간</param>
    /// <returns></returns>
    private IEnumerator FadeIn(CanvasGroup canvas, float duration)
    {
        isFadeFinish = false;
        float timer = 0f;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(startAlpha, endAlpha, timer);
        }

        isFadeFinish = true;
        canvas.alpha = 1f;
        yield return null;
    }
}
