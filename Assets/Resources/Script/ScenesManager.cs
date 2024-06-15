using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScenesManager : MonoBehaviour
{
    float gameTimer = 0;
    float[] endLevelTimer = { 30, 30, 60 };
    int currentSceneNumber = 0;
    bool gameEnding = false;

    Scenes scenes;
    public enum Scenes
    {
        bootUp,
        title,
        shop,
        level1,
        level2,
        level3,
        gameOver
    }

    public enum MusicMode { noSound, fadeDown, musicOn }

    public MusicMode musicMode;

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));

        GetComponent<GameManager>().SetLivesDisplay(GameManager.playerLives);

        if (GameObject.Find("score"))
        {
            GameObject.Find("score").GetComponent<Text>().text = ScoreManager.playerScore.ToString();
        }
    }

    void Start()
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("Les game stats seront sauvegardées dans ce fichier : " + Application.persistentDataPath + "/GameStatsSaved.json");
    }

    void Update()
    {
        if (currentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }
        GameTimer();
    }

    private IEnumerator MusicVolume(MusicMode musicMode)
    {
        switch (musicMode)
        {
            case MusicMode.noSound:
                GetComponentInChildren<AudioSource>().Stop();
                break;
            case MusicMode.fadeDown:
                GetComponentInChildren<AudioSource>().volume -= Time.deltaTime * 0.33f;
                break;
            case MusicMode.musicOn:
                if (GetComponentInChildren<AudioSource>().clip != null)
                {
                    GetComponentInChildren<AudioSource>().Play();
                    GetComponentInChildren<AudioSource>().volume = 1;
                }
                break;
        }
        yield return new WaitForSeconds(0.1f);
    }


    void GetScene()
    {
        scenes = (Scenes)currentSceneNumber;
    }

    public void GameOver()
    {
        Debug.Log("ENDSCORE:" + GameManager.Instance.GetComponent<ScoreManager>().PlayersScore);
        SceneManager.LoadScene("gameOver");
    }

    void GameTimer()
    {
        switch (scenes)
        {
            case Scenes.level1:
            case Scenes.level2:
            case Scenes.level3:
                {
                    if (gameTimer < endLevelTimer[currentSceneNumber - 3])
                    {
                        //if level has not completed
                        gameTimer += Time.deltaTime;

                        if (GetComponentInChildren<AudioSource>().clip == null)
                        {
                            AudioClip lvlMusic = Resources.Load<AudioClip>("Sound/lvlMusic");
                            GetComponentInChildren<AudioSource>().clip = lvlMusic;
                            GetComponentInChildren<AudioSource>().Play();
                        }
                    }
                    else
                    {
                        //if level is completed
                        StartCoroutine(MusicVolume(MusicMode.fadeDown));
                        if (!gameEnding)
                        {
                            gameEnding = true;

                            GameObject player = GameObject.Find("Player");
                            player.GetComponent<Rigidbody>().isKinematic = true;
                            Player.mobile = false;
                            CancelInvoke();

                            if (SceneManager.GetActiveScene().name != "level3" || SceneManager.GetActiveScene().name != "Level3")
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().LevelEnds = true;
                            }
                            else
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().GameCompleted = true;
                            }

                            SendInJsonFormat(SceneManager.GetActiveScene().name);

                            Invoke("NextLevel", 4);
                        }
                    }




                    break;
                }
            default:
                {
                    GetComponentInChildren<AudioSource>().clip = null;
                    break;
                }
        }
    }

    private void SendInJsonFormat(string lastLevel)
    {
        if (lastLevel == "level3" || lastLevel == "Level3")
        {
            Debug.Log("Making a json from game stats");

            GameStats gameStats = new GameStats();
            gameStats.livesLeft = GameManager.playerLives;
            gameStats.completed = System.DateTime.Now.ToString();
            gameStats.score = ScoreManager.playerScore;
            string json = JsonUtility.ToJson(gameStats, true);
            Debug.Log(json);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/GameStatsSaved.json", json);
        }
    }

    void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene + 1);
        StartCoroutine(MusicVolume(MusicMode.musicOn));
    }

    public void ResetScene()
    {
        StartCoroutine(MusicVolume(MusicMode.noSound));
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene);
    }

    public void BeginGame(int gameLevel)
    {
        gameTimer = 0;
        SceneManager.LoadScene(gameLevel);
    }
}