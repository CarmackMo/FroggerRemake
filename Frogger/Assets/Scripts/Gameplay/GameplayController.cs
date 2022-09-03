using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public float gameTime = 0f;
    public float obstacleGenerateInterval = 0f;
    public GameObject KnockBackObstacle;
    public FrogController frog;


    private void Start()
    {
        StartCoroutine(TimerCoroutrine());
        StartCoroutine(ObstacleGenerateCoroutine());
    }



    IEnumerator TimerCoroutrine()
    {
        while (gameTime > 0)
        {
            GamePanel.Instance.UpdateTimer((int)gameTime);
            yield return new WaitForEndOfFrame();
            gameTime -= Time.deltaTime;
        }

        frog.SetGameOver();
        GamePanel.Instance.ShowGameResult(false);
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
