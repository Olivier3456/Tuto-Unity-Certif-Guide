using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SOActorModel actorModel;
    [SerializeField] private float spawnRate;
    [SerializeField, Range(0, 10)] private int quantity;

    private GameObject enemies;

    private void Awake()
    {
        enemies = GameObject.Find("_Enemies");
        StartCoroutine(FireEnemy(quantity, spawnRate));
    }

    private IEnumerator FireEnemy(int quantity, float spawnRate)
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        for (int i = 0; i < quantity; i++)
        {
            GameObject enemyUnit = CreateEnemy();
            enemyUnit.gameObject.transform.SetParent(transform);
            enemyUnit.transform.position = transform.position;
            yield return wait;
        }
    }

    private GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(actorModel.actor);
        enemy.GetComponent<IActorTemplate>().ActorStats(actorModel);
        enemy.name = actorModel.actorName;
        return enemy;
    }
}
