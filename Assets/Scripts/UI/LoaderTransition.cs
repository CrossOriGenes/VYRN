using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoaderTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private string _nextSceneName = "Main Menu";
    [SerializeField] private float _minimumLoadingTime = 2.5f;

    private bool _isLoading;
    private CanvasGroup _loaderCanvasGroup;


    private void Awake()
    {
        _loaderCanvasGroup = GetComponent<CanvasGroup>();
        if (_loaderCanvasGroup != null)
        {
            _loaderCanvasGroup.alpha = 0f;
            _loaderCanvasGroup.interactable = false;
            _loaderCanvasGroup.blocksRaycasts = false;
        }
    }

    private void OnEnable()
    {
        BootScreenController.ContinueRequested += HandleContinueRequested;
    }

    private void OnDisable()
    {
        BootScreenController.ContinueRequested -= HandleContinueRequested;
    }

    private void HandleContinueRequested()
    {
        if (_isLoading) return;

        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        if (_loaderCanvasGroup == null)
        {
            Debug.LogError("Loading CanvasGroup reference is missing!");
            yield break;
        }

        if (!Application.CanStreamedLevelBeLoaded(_nextSceneName))
        {
            Debug.LogError(
                $"Scene '{_nextSceneName}' is missing from Build Profiles / Scene List!"
            );
            yield break;
        }

        _isLoading = true;

        _loaderCanvasGroup.alpha = 1f;
        _loaderCanvasGroup.interactable = false;
        _loaderCanvasGroup.blocksRaycasts = true;

        AsyncOperation operation = 
            SceneManager.LoadSceneAsync(_nextSceneName);
        if (operation == null)
        {
            Debug.LogError("Scene loading operation could not start!");
            yield break; 
        }    

        operation.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(_minimumLoadingTime);

        while (operation.progress < .9f)
            yield return null;

        operation.allowSceneActivation = true;
    }
}
