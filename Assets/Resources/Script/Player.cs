using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IActorTemplate
{
    private int travelSpeed;
    private int health;
    private int hitPower;
    private GameObject actor;
    private GameObject fire;

    public int Health { get { return health; } set { health = value; } }
    public GameObject Fire { get { return fire; } set { fire = value; } }

    private GameObject _Player;
    private float width;
    private float height;


    private void Start()
    {
        height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);
        width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        _Player = GameObject.Find("_Player");
    }

    private void Update()
    {
        Movement();
        Attack();
    }

    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.hitPower;
        fire = actorModel.actorsBullets;
    }

    public int SendDamage()
    {
        return hitPower;
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    public void Die()
    {
        GameManager.Instance.LifeLost();
        Destroy(gameObject);
    }

    public void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(fire, transform.position, Quaternion.Euler(Vector3.zero));
            bullet.transform.SetParent(_Player.transform);
            bullet.transform.localScale = Vector3.one * 7;
        }
    }




    private void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float horizontalMovement = horizontalInput * Time.deltaTime * travelSpeed;

        if (horizontalInput > 0)
        {
            if (transform.localPosition.x < width + width / 0.9f)
            {
                transform.localPosition += new Vector3(horizontalMovement, 0, 0);
            }
        }

        if (horizontalInput < 0)
        {
            if (transform.localPosition.x > width + width / 6f)
            {
                transform.localPosition += new Vector3(horizontalMovement, 0, 0);
            }
        }


        float verticalInput = Input.GetAxisRaw("Vertical");
        float verticalMovement = verticalInput * Time.deltaTime * travelSpeed;

        if (verticalInput < 0)
        {
            if (transform.localPosition.y > -height / 3f)
            {
                transform.localPosition += new Vector3(0, verticalMovement, 0);
            }
        }

        if (verticalInput > 0)
        {
            if (transform.localPosition.y < height / 2.5f)
            {
                transform.localPosition += new Vector3(0, verticalMovement, 0);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (health > 0)
            {
                if (transform.Find("energy +1 (Clone)"))
                {
                    Destroy(transform.Find("energy +1 (Clone)").gameObject);
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
                else
                {
                    health -= 1;
                }
            }
            else
            {
                Die();
            }
        }
    }
}
