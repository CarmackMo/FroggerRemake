using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePanel : CanvasSingleton<GamePanel>
{
    public TextMeshProUGUI timerText;

    public GameObject gameResult;


    public void UpdateTimer(int time)
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

}
