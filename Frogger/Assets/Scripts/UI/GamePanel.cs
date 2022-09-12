using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePanel : Singleton<GamePanel>
{
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI protectText;
    public GameObject gameResult;


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void UpdateCoundDownTimer(int time)
    {
        int min = time / 60;
        int sec = time % 60;

        string text = "";

        if (min < 10)
            text += $"0{min}:";
        else
            text += $"{min}:";

        if (sec < 10)
            text += $"0{sec}";
        else
            text += $"{sec}";

        timerText.text = text;
    }

    public void UpdateHPText(int damage, int totalHP)
    {
        HPText.text = $"HP: {totalHP - damage}/{totalHP}";
    }

    /// <summary>
    /// Player win: status = true;
    /// Player lose: status = false; 
    /// </summary>
    public void ShowGameResult(bool status)
    {
        TextMeshProUGUI resultText = gameResult.GetComponent<TextMeshProUGUI>();

        if (status == true)
            resultText.text = $"YOU WIN!";
        else
            resultText.text = $"YOU LOSE!";

        gameResult.SetActive(true);
    }

    public void HideGameResult()
    {
        gameResult.SetActive(false);
    }

    public void HideProtectText()
    {
        protectText.gameObject.SetActive(false);
    }
}
