using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    private Vector3 start_game_pos = new Vector3(-100, 0, 0);
    private Vector3 start_scene_pos;
    private Vector3 transitionToCompleteGame = new Vector3(7000, 0, 0);
    private Vector3 readyPos = new Vector3(9000, 0, 0);

    private float distCovered;
    private float journeyLength;

    private bool levelStarted = true;
    private bool speedOff = false;
    private bool levelEnds = false;
    private bool gameCompleted = false;

    public bool LevelEnds
    {
        get { return levelEnds; }
        set { levelEnds = value; }
    }
    public bool GameCompleted
    {
        get { return gameCompleted; }
        set { gameCompleted = value; }
    }


    private void Start()
    {
        transform.localPosition = Vector3.zero;
        start_scene_pos = transform.position;
        Distance();
    }

    private void Update()
    {
        if (levelStarted)
        {
            StartCoroutine(PlayerMovement(start_game_pos, 10f));
        }

        if (levelEnds)
        {
            GetComponent<Player>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            Distance();
            StartCoroutine(PlayerMovement(start_game_pos, 200f));
        }

        if (gameCompleted)
        {
            GetComponent<Player>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(PlayerMovement(start_game_pos, 200f));
        }

        if (speedOff)
        {
            Invoke("SpeedOff", 1f);
        }
    }

    private IEnumerator PlayerMovement(Vector3 arrival, float transitionSpeed)
    {
        if (Mathf.Round(transform.localPosition.x) >= readyPos.x - 5f &&
            Mathf.Round(transform.localPosition.x) <= readyPos.x + 5f &&
            Mathf.Round(transform.localPosition.y) >= readyPos.y - 5f &&
            Mathf.Round(transform.localPosition.y) <= readyPos.y + 5f)
        {
            Debug.Log("Player ship is at ready pos");

            if (levelEnds)
            {
                levelEnds = false;
                speedOff = true;
            }

            if (levelStarted)
            {
                levelStarted = false;
                distCovered = 0f;
                GetComponent<Player>().enabled = true;
            }
            yield return null;
        }
        else
        {
            distCovered += Time.deltaTime * transitionSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, arrival, fractionOfJourney);
        }
    }

    private void SpeedOff()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 800);
    }


    private void Distance()
    {
        journeyLength = Vector3.Distance(start_scene_pos, readyPos);
    }
}
