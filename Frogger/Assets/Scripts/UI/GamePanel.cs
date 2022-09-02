using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePanel : CanvasSingleton<GamePanel>
{
    public TextMeshProUGUI timerText;


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
}
