using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class ScenesManager : MonoBehaviour
{
    Scenes scenes;
    public enum Scenes
    {
        bootUp,
        title,
        shop,
        level1,
        level2,
        level3,
        gameover
    }

    private float gameTimer = 0f;
    private float[] endLevelTimer = { 30f, 30f, 45f };
    private int currentSceneNumber = 0;
    private bool gameEnding = false;

    public void ResetScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameTimer = 0f;
        SceneManager.LoadScene(GameManager.currentScene);
    }

    private void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0f;
        SceneManager.LoadScene(GameManager.currentScene + 1);
    }

    public void BeginGame(int gameLevel)
    {
        //SceneManager.LoadScene("testLevel");
        SceneManager.LoadScene(gameLevel);    
    }

    public void GameOver()
    {
        Debug.Log("ENDSCORE:" + GameManager.Instance.GetComponent<ScoreManager>().PlayersScore);
        SceneManager.LoadScene("gameOver");
    }


    private void Update()
    {
        if (currentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }

        GameTimer();
    }

    private void GameTimer()
    {
        switch (scenes)
        {
            case Scenes.level1:
            case Scenes.level2:
            case Scenes.level3:
                {
                    if (gameTimer < endLevelTimer[currentSceneNumber - 3])
                    {
                        gameTimer += Time.deltaTime;
                    }
                    else
                    {
                        if (!gameEnding)
                        {
                            gameEnding = true;
                            if (SceneManager.GetActiveScene().name != "level3")
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().LevelEnds = true;
                            }
                            else
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().GameCompleted = true;
                            }

                            Invoke("NextLevel", 4f);
                        }
                    }
                    break;
                }
        }
    }

    private void GetScene()
    {
        scenes = (Scenes)currentSceneNumber;
    }
}