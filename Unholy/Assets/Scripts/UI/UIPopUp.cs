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
    /// Player�� End ���¿� ���� �׿� �´� image�� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <returns>End ���¿� �˸´� canvasgroup�� ��ȯ�մϴ�.</returns>
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
    /// ������ ������ ������ �ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="canvas">Ư�� �̺�Ʈ�� �ش�Ǵ� canvas�Դϴ�.</param>
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
    /// �ش� canvas�� ������ ��Ÿ���� ���ִ� IEnumerator �Դϴ�.
    /// </summary>
    /// <param name="canvas">Ư�� �̺�Ʈ�� �ش�Ǵ� canvas group</param>
    /// <param name="duration">canvas�� fadein �Ǵ� �ð�</param>
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
