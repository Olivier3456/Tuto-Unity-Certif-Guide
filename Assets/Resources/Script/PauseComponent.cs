using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseComponent : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(false);
        Invoke("DelayPauseAppear", 5);
    }

    private void DelayPauseAppear()
    {
        SetPauseButtonActive(true);
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        SetPauseButtonActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(true);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        GameManager.Instance.GetComponent<ScoreManager>().ResetScore();
        GameManager.Instance.GetComponent<ScenesManager>().BeginGame(0);
    }


    private void SetPauseButtonActive(bool isActive)
    {
        ColorBlock col = GetComponentInChildren<Toggle>().colors;

        if (!isActive)
        {
            col.normalColor = new Color(0, 0, 0, 0);
            col.highlightedColor = new Color(0, 0, 0, 0);
            col.pressedColor = new Color(0, 0, 0, 0);
            col.disabledColor = new Color(0, 0, 0, 0);
        }
        else
        {
            col.normalColor = new Color32(245, 245, 245, 255);
            col.highlightedColor = new Color32(245, 245, 245, 255);
            col.pressedColor = new Color32(200, 200, 200, 255);
            col.disabledColor = new Color32(200, 200, 200, 128);
        }

        GetComponentInChildren<Toggle>().interactable = isActive;
        GetComponentInChildren<Toggle>().colors = col;
        GetComponentInChildren<Toggle>().transform.GetChild(0).GetChild(0).gameObject.SetActive(isActive);
    }


}
