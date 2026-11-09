using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text _loaderText;


    private void Awake()
    {
        if (_loaderText != null)
            _loaderText.alpha = 0f;
    }  

    private void Start()
    {
        StartCoroutine(ShowLoaderText());
    }

    private IEnumerator ShowLoaderText()
    {
        yield return new WaitForSeconds(1.4f);
        
        _loaderText.alpha = 1f;

        yield return new WaitForSeconds(2.3f);

        SceneManager.LoadScene("Boot Scene");
    }
}
