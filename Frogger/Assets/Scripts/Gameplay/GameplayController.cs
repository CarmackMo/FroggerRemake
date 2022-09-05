using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : Singleton<GameplayController>
{
    public bool gameOver = false;
    public float gameTime = 0f;
    public float obstacleGenerateInterval = 0f;

    public GameObject KnockBackObstacle;
    public FrogController frog;

    private int objectiveNum = 0;
    private int achieveNum = 0;


    private void Start()
    {
        StartCoroutine(TimerCoroutrine());
        StartCoroutine(ObstacleGenerateCoroutine());

        GamePanel.Instance.UpdateHPText(0, frog.totalHP);

        objectiveNum = GameObject.FindGameObjectsWithTag("Ends").Length;

    }


    private void Update() { }



    /// <summary>
    /// Player win: status = true;
    /// Player lose: status = false;
    /// </summary>
    public void SetGameOver(bool status)
    {
        gameOver = true;
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
            GamePanel.Instance.UpdateTimer((int)gameTime);
            yield return new WaitForEndOfFrame();
            gameTime -= Time.deltaTime;
        }

        SetGameOver(false);
        yield break;

    }


    IEnumerator ObstacleGenerateCoroutine()
    {
        float count = obstacleGenerateInterval;
        while (true)
        {
            if(count <= 0)
            {
                Instantiate(KnockBackObstacle);
                count = obstacleGenerateInterval;
            }

            count -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
