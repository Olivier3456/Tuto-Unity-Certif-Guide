using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
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
    private Scenes scenes;

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Debug.Log($"ENDSCORE: {GameManager.Instance.GetComponent<ScoreManager>().PlayerScore}.");
        SceneManager.LoadScene("gameOver");
    }

    public void BeginGame()
    {
        SceneManager.LoadScene("testLevel");
    }
}
