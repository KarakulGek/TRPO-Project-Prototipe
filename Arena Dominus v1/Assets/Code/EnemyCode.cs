using System.Collections;
using UnityEngine;

public class EnemyCode : MonoBehaviour
{
    [SerializeField] private float xMove;
    [SerializeField] private float zMove;
    public float speed = 5f;
    public float Rotatespeed = 250f;
    [SerializeField] bool move = false;
    private GameObject ShotEnemyBullet;
    public GameObject SB;
    public GameObject player;
    public GameObject Explosion;
    public GameObject TimerCount;
    public int health = 2;

    void Start()
    {
        player = GameObject.Find("Player");
        health = 2;
        StartCoroutine(MoveState());
        StartCoroutine(Shoot()); 
    }

    void FixedUpdate()
    {
        EnemyMove();
        if (health <= 0)
        {
            GameObject Expl = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(Expl, 2f);
            Destroy(gameObject);
        }
    }

    void EnemyMove()
    {
        if (move == true) 
        {
            if ((xMove == 1) && (gameObject.transform.position.x >= 50))
                xMove = 0;
            if ((xMove == -1) && (gameObject.transform.position.x <= -50))
                xMove = 0;
            if ((zMove == 1) && (gameObject.transform.position.z >= 50))
                zMove = 0;
            if ((zMove == -1) && (gameObject.transform.position.z <= -50))
                zMove = 0;
            transform.Translate(new Vector3(xMove, 0, zMove).normalized * speed * Time.fixedDeltaTime, Space.World);
        }
        if (xMove != 0 || zMove != 0)
        {
            Quaternion ToRotate = Quaternion.LookRotation(new Vector3(xMove, 0, zMove), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ToRotate, Rotatespeed * Time.deltaTime);
        }
    }

    private IEnumerator MoveState()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            move = !move;
            if (move == true)
            {
                xMove = UnityEngine.Random.Range(-1, 2);
                zMove = UnityEngine.Random.Range(-1, 2);
            }
        }
    }
    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2,6));
            EnemyShoot();
        }
    }
    private void EnemyShoot()
    {
        Vector3 NewDirection = Vector3.RotateTowards(transform.forward, player.transform.position - transform.position, 500 * Time.deltaTime, 0f);
        NewDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(NewDirection);
        ShotEnemyBullet = SB;
        ShotEnemyBullet.GetComponent<PlayerBullet>().PlayerType = false;
        Instantiate(ShotEnemyBullet, transform.position, transform.rotation);
    }
    private void OnDestroy()
    {
        GameObject TimerCount = GameObject.Find("TimerCount");
        TimerCount.GetComponent<TimerScript>().EnemiesLeft -= 1;
        TimerCount.GetComponent<TimerScript>().EnemiesCountUpdate();
        TimerCount.GetComponent<TimerScript>().Enemies.Remove(gameObject);
    }
}
