using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlee : MonoBehaviour, IActorTemplate
{
    [SerializeField]
    SOActorModel actorModel;
    int health;
    int travelSpeed;
    int hitPower;
    int score;

    [SerializeField] private float enemyDistanceRun = 200f;
    private GameObject player;
    private bool gameStarts;
    private NavMeshAgent enemyAgent;


    private void Start()
    {
        ActorStats(actorModel);
        Invoke("DelayedStart", 0.5f);
    }


    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        hitPower = actorModel.hitPower;
        score = actorModel.score;
        GetComponent<NavMeshAgent>().speed = actorModel.speed;
    }


    private void DelayedStart()
    {
        gameStarts = true;
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include).gameObject;

        enemyAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (gameStarts)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < enemyDistanceRun)
                {
                    //Debug.Log("Enemy detected!");

                    Vector3 dirToPlayer = transform.position - player.transform.position;
                    Vector3 newPos = transform.position + dirToPlayer;

                    enemyAgent.SetDestination(newPos);
                }
            }
        }
    }




    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }
    public int SendDamage()
    {
        return hitPower;
    }
    public void Die()
    {
        GameObject explode = Instantiate(Resources.Load("Prefab/explode")) as GameObject;
        explode.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // if the player or their bullet hits you....
        if (other.tag == "Player")
        {
            if (health >= 1)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage();
            }
            if (health <= 0)
            {
                //died by player, apply score to 
                GameManager.Instance.GetComponent<ScoreManager>().SetScore(score);
                Die();
            }
        }
    }
}
