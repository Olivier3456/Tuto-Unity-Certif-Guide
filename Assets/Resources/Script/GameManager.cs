using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject dirLight;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public static int currentScene = 0;
    public static int gameLevelScene = 3;

    private bool died = false;
    public bool Died { get { return died; } set { died = value; } }

    public static int playerLives = 3;


    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }


    private void Awake()
    {
        CreateSingleton();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        LightAndCameraSetup(currentScene);
    }


    private void LightAndCameraSetup(int sceneNumber)
    {
        switch (sceneNumber)
        {
            case 3:
            case 4:
            case 5:
            case 6:     // Game levels
                {
                    LightSetup();
                    CameraSetup();
                    break;
                }
        }
    }

    private void CameraSetup()
    {
        GameObject goCamera = Camera.main.gameObject;
        goCamera.transform.position = new Vector3(0, 0, -300);
        goCamera.transform.eulerAngles = Vector3.zero;


        Camera camera = goCamera.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(0, 0, 0, 255);
    }

    private void LightSetup()
    {
        dirLight.transform.eulerAngles = new Vector3(50, -30, 0);
        dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255);
    }

    public void LifeLost()
    {
        if (playerLives > 0)
        {
            playerLives--;
            Debug.Log($"Lives left: {playerLives}.");
            GetComponent<ScenesManager>().ResetScene();
        }
        else
        {
            playerLives = 3;
            GetComponent<ScenesManager>().GameOver();
        }
    }
}
