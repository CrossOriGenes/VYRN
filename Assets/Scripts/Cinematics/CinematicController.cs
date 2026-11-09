using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    [Header("Cinematic References")]
    [SerializeField] private PlayableDirector _timelineDirector;
    [SerializeField] private GameObject _skipPrompt;

    [Header("Cinematic Settings")]
    [SerializeField] private float _cooldownTime = 2f;
    [SerializeField] private float _skipRevealTime = 35f;

    private CanvasGroup _skipBtnVisibility;
    private bool _hasStarted, 
    _hasCompleted, 
    _skipRequested,
    _skipUnlocked;
    public const string OPENINGCINEMATIC_STATE_STRING = "isOpeningCinematicCompleted";
    public bool HasCompletedOpeningCinematic => 
        PlayerPrefs.GetInt(OPENINGCINEMATIC_STATE_STRING, 0) == 1; 


    private void Awake()
    {
        _skipBtnVisibility = _skipPrompt.GetComponent<CanvasGroup>();
        _skipBtnVisibility.alpha = 0f;          
    }

    private void OnEnable()
    {
        if (_timelineDirector != null)
            _timelineDirector.stopped += OnTimelineStopped;
    } 

    private void Start()
    {
        StartCinematic();
    }

    private void Update()
    {
        if (!_hasStarted || 
        _hasCompleted ||
        _skipUnlocked ||
        _timelineDirector == null)
            return;

        if (_timelineDirector.state == PlayState.Playing &&
        _timelineDirector.time >= _skipRevealTime)
            ShowSkipPrompt();
    } 

    private void OnDisable()
    {
        if (_timelineDirector != null)
            _timelineDirector.stopped -= OnTimelineStopped;
    }

    public void StartCinematic()
    {
        if (_hasStarted || _hasCompleted)
            return;

        if (_timelineDirector == null)
        {
            Debug.LogError("Timeline Director reference is missing!");
            return;
        }

        _hasStarted = true;

        _timelineDirector.Play();
    }

    public void SkipCinematic()
    {
        if (!_hasStarted || _hasCompleted)
            return;

        if (_timelineDirector == null ||
        _timelineDirector.state != PlayState.Playing)
            return;

        _skipRequested = true;
        Debug.Log("Storyline skip requested.");

        _timelineDirector.time = _timelineDirector.duration;
        _timelineDirector.Evaluate();
        _timelineDirector.Stop();
    }

    private void ShowSkipPrompt()
    {
        _skipUnlocked = true;

        _skipBtnVisibility.alpha = 1;
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director != _timelineDirector || 
        _hasCompleted)
            return;

        CompletedCinematic();
    }

    private void CompletedCinematic()
    {
        if (_hasCompleted) return;

        _hasCompleted = true;

        _skipBtnVisibility.alpha = 0f;

        if (_skipRequested)
            Debug.Log("Storyline Skipped.");
        else 
            Debug.Log("Story Completed.");
        
        StartCoroutine(GoToNextScene());
    }

    private IEnumerator GoToNextScene()
    {
        PlayerPrefs.SetInt(OPENINGCINEMATIC_STATE_STRING, 1);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(_cooldownTime);
        SceneManager.LoadScene("Prolog");
    }
}
