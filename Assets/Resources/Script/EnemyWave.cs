using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour, IActorTemplate
{
    private int health;
    private int travelSpeed;
    private int fireSpeed;
    private int hitPower;
    private int score;


    [SerializeField] private float verticalSpeed = 2f;
    [SerializeField] private float verticalAmplitude = 1f;
    private Vector3 sineVer;
    private float time;


    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.hitPower;
        score = actorModel.score;
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        time += Time.deltaTime;
        sineVer.y = Mathf.Sin(time * verticalSpeed) * verticalAmplitude;
        transform.position = new Vector3(transform.position.x + travelSpeed * Time.deltaTime, transform.position.y + sineVer.y, transform.position.z);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (health > 0)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage();
            }
            else
            {
                Die();
                GameManager.Instance.GetComponent<ScoreManager>().SetScore(score);
            }
        }
    }
}
