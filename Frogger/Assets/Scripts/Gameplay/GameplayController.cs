using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public float gameTime = 0f;



    private void Start()
    {
        StartCoroutine(TimerCoroutrine());
    }



    IEnumerator TimerCoroutrine()
    {
        while (gameTime > 0)
        {
            GamePanel.Instance.UpdateTimer((int)gameTime);
            yield return new WaitForEndOfFrame();
            gameTime -= Time.deltaTime;
        }

        yield break;

    }


}
