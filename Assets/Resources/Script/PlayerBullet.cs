using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IActorTemplate
{
    [SerializeField] private SOActorModel bulletModel;

    private GameObject actor;
    private int hitPower;
    private int health;
    private int travelSpeed;

    private void Awake()
    {
        ActorStats(bulletModel);
    }


    public void ActorStats(SOActorModel actorModel)
    {
        hitPower = actorModel.hitPower;
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        actor = actorModel.actor;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public int SendDamage()
    {
        return hitPower;
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }


    private void Update()
    {
        transform.position += new Vector3(travelSpeed, 0, 0) * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<IActorTemplate>() != null)
            {
                if (health > 0)
                {
                    TakeDamage(other.GetComponent<IActorTemplate>().SendDamage());
                }
                else
                {
                    Die();
                }
            }
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
