using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private SOActorModel actorModel;
    private GameObject playerShip;


    private void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        // Create player
        //actorModel = Instantiate(Resources.Load("/Script/ScriptableObject/Player_Default") as SOActorModel);
        //actorModel = Resources.Load("/Script/ScriptableObject/Player_Default") as SOActorModel;
        playerShip = Instantiate(actorModel.actor);
        playerShip.GetComponent<Player>().ActorStats(actorModel);   // Initialize Player stats.

        // Set Player up
        playerShip.transform.rotation = Quaternion.Euler(0, 180, 0);
        playerShip.transform.localScale = Vector3.one * 60;
        playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = Vector3.one * 25f;
        playerShip.name = "Player";
        playerShip.transform.SetParent(transform);
        playerShip.transform.position = Vector3.zero;
    }
}
