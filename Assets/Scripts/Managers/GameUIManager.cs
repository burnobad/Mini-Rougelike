using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup selectUpgScreen;

    [SerializeField]
    private CanvasGroup gameOverScreen;
    private void Start()
    {
        GameEventsManager.AddLevelEvent += AddLevel;
        GameEventsManager.UpgradeChosenEvent += UpgradeChosen;
        GameEventsManager.GameOverEvent += GameOver;
        GameEventsManager.RestartGame += RestartGame;
    }

    private void RestartGame()
    {
        CloseScreen(selectUpgScreen);
        CloseScreen(gameOverScreen);
    }
    private void AddLevel(Upgrades[] _availableUpgrades)
    {
        selectUpgScreen.GetComponent<SelectUpgradeManager>().SetArray(_availableUpgrades);
        ShowScreen(selectUpgScreen);
    }

    private void UpgradeChosen(Upgrades _upg)
    {
        CloseScreen(selectUpgScreen);
    }

    private void GameOver()
    {
        ShowScreen(gameOverScreen);
    }

    private void ShowScreen(CanvasGroup _toClose)
    {
        _toClose.alpha = 1;
        _toClose.interactable = true;
        _toClose.blocksRaycasts = true;
        _toClose.gameObject.SetActive(true);
    }

    private void CloseScreen(CanvasGroup _toClose)
    {
        _toClose.alpha = 0;
        _toClose.interactable = false;
        _toClose.blocksRaycasts = false;
        _toClose.gameObject.SetActive(false);
    }
}
