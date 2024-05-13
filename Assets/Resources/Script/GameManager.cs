using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject dirLight;

    void Start()
    {
        CameraSetup();
        LightSetup();
    }

    private void CameraSetup()
    {
        GameObject gameCamera = Camera.main.gameObject;
        gameCamera.transform.position = new Vector3(0, 0, -300);
        gameCamera.transform.eulerAngles = Vector3.zero;


        Camera camera = gameCamera.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(0, 0, 0, 255);
    }

    private void LightSetup()
    {
        dirLight.transform.eulerAngles = new Vector3(50, -30, 0);
        dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255);
    }
}
