using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 50f;
    public bool PlayerType = true;
    public GameObject HitExplosion;

    private void Start()
    {
        StartCoroutine(BulletDestroy());
    }
    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, 1) * speed * Time.fixedDeltaTime, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerType == false)     
        {
            other.GetComponent<PlayerMovement>().health -= 1;
            GameObject Expl = Instantiate(HitExplosion, other.transform.position, other.transform.rotation);
            Destroy(Expl, 1f);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy") && PlayerType == true)
        {
            other.GetComponent<EnemyCode>().health -= 1;
            other.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(0.5f * other.GetComponent<EnemyCode>().health, 0, 0));
            GameObject Expl = Instantiate(HitExplosion, other.transform.position, other.transform.rotation);
            Destroy(Expl, 1f);
            Destroy(gameObject);
        }
    }
    private IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
