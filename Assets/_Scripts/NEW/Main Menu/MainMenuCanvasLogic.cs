using EasyTextEffects;
using TMPro;
using UnityEngine;

public class MainMenuCanvasLogic : MonoBehaviour
{
    static readonly string _logTag = "MainMenuCanvasLogic";
    [SerializeField] private MainMenuCanvasHeaderLogic headerLogic;
    [SerializeField] private MainMenuCanvasBodyLogic bodyLogic;

    private void OnDestroy()
    {
        MainMenuManager.OnContinue -= OnContinueHandler;
        MainMenuCanvasHeaderLogic.OnContinueAnimationComplete -= OnContinueAnimationCompleteHandler;
    }

    private void Awake()
    {
        MainMenuManager.OnContinue += OnContinueHandler;
        MainMenuCanvasHeaderLogic.OnContinueAnimationComplete += OnContinueAnimationCompleteHandler;

        headerLogic.gameObject.SetActive(true);
        bodyLogic.gameObject.SetActive(false);
    }

    private void OnContinueHandler()
    {
        headerLogic.PlayContinueAnimation();
    }

    private void OnContinueAnimationCompleteHandler()
    { 
        bodyLogic.gameObject.SetActive(true);
    }
}
