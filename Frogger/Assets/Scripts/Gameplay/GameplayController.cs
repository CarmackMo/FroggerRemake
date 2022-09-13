using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : Singleton<GameplayController>
{
    public bool gameOver = false;
    public float gameTime = 0f;
    public enum GameState { PROTECT, PLAY, OVER, };

    public GameState state = GameState.PROTECT;
    public FrogController frog;

    private int objectiveNum = 0;
    private int achieveNum = 0;



    private void Start()
    {
        GamePanel.Instance.UpdateHPText(0, frog.totalHP);

        objectiveNum = GameObject.FindGameObjectsWithTag("Ends").Length;

        Time.timeScale = 0;
    }


    private void Update() 
    {
        DetectGameState();
    }



    /// <summary>
    /// Player win: status = true;
    /// Player lose: status = false;
    /// </summary>
    public void SetGameOver(bool status)
    {
        state = GameState.OVER;
        Time.timeScale = 0;
        GamePanel.Instance.ShowGameResult(status);
    }


    public void IncreaseAchievedObjective()
    {
        achieveNum++;

        if (achieveNum == objectiveNum)
        {
            SetGameOver(true);
        }
    }


    IEnumerator TimerCoroutrine()
    {
        while (gameTime > 0)
        {
            GamePanel.Instance.UpdateCoundDownTimer((int)gameTime);
            yield return new WaitForEndOfFrame();
            gameTime -= Time.deltaTime;
        }

        SetGameOver(false);
        yield break;

    }

    public void DetectGameState()
    {
        if (state == GameState.PROTECT && Input.anyKey == true)
        {
            state = GameState.PLAY;
            Time.timeScale = 1;
            GamePanel.Instance.HideProtectText();
            StartCoroutine(TimerCoroutrine());
        }
    }

}
