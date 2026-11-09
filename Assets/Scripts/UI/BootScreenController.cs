using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class BootScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text _instructionText; 

    public static event Action ContinueRequested;
    
    private IDisposable _inputListener;
    private bool _canContinue;
    private bool _continueRequested;


    private void Awake()
    {
        if (_instructionText != null)
            _instructionText.alpha = 0f;
    }

    private void OnDisable()
    {
        _canContinue = false;

        _inputListener?.Dispose();
        _inputListener = null;
    }

    private void Start()
    {
        StartCoroutine(RevealTextInstructionForNextScene());
    }

    private IEnumerator RevealTextInstructionForNextScene()
    {
        yield return new WaitForSeconds(2.4f);

        if (_instructionText != null)
            _instructionText.alpha = 1f;
            
        _canContinue = true;

        _inputListener = 
            InputSystem.onAnyButtonPress.CallOnce(OnAnyButtonPressed);
    }

    private void OnAnyButtonPressed(InputControl control)
    {
        if (!_canContinue || _continueRequested)
            return;

        _continueRequested = true;
        _canContinue = false;

        ContinueRequested?.Invoke();
    }
}
