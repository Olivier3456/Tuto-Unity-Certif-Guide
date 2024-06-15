using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleComponent : MonoBehaviour
{
    //void Start()
    //{
    //    GameManager.playerLives = 3;
    //}

    void Update()
    {
        if (GameManager.playerLives <= 2)
        {
            GameManager.playerLives = 3;
        }


        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("shop");
        }
    }
}