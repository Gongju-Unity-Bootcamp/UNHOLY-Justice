using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialImage;

    private PlayerInput _playerUIInput;
    private InputActionMap _UIActionMap;
    private InputAction _exitGame;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if (!Cursor.visible) Cursor.visible = true;

        if (_playerUIInput == null)
        {
            _playerUIInput = GetComponent<PlayerInput>();
            _UIActionMap = _playerUIInput.actions.FindActionMap("UI");
            _exitGame = _playerUIInput.actions.FindAction("ESC");

            _exitGame.performed += context =>
            {
                ExitGame();
            };
        }

        //SoundManager.Instance.Play(SoundType.Bgm, "test-bgm");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TutorialButton()
    {
        SoundManager.Instance.Play(SoundType.Effect, "TitleButton");
        _tutorialImage.SetActive(true);
    }

    public void CloseButton()
    {
        SoundManager.Instance.Play(SoundType.Effect, "TitleButton");
        _tutorialImage.SetActive(false);
    }

    public void StartGame()
    {
        SoundManager.Instance.Play(SoundType.Effect, "TitleButton");
        LoadingManager.Instance.LoadScene((int)SceneIndex.Play);
    }

    public IEnumerator LoadPlayScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)SceneIndex.Play);
        yield return null;
    }
}
