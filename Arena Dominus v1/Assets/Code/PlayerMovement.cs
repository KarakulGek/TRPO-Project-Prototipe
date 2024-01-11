using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float xMove;
    [SerializeField] private float zMove;
    [SerializeField] private float rot;
    public float speed = 10f;
    public float Rotatespeed = 10f;
    public static bool Shooting = false;
    public LayerMask PlaneLayer;
    public GameObject ShootingLine;
    public GameObject SB;
    private GameObject ShotPlayerBullet;
    public GameObject Explosion;
    public GameObject EndText;
    public int health = 5;
    private Animator animator;

    public GameObject cam;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        health = 5;
    }
    void FixedUpdate()
    {
        if (Shooting == false)
        {
            PlayerMove();
        }
        else
        {
            PlayerShootMode();

        }
        if (health <= 0)
        {
            GameObject Expl = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(Expl, 2f);
            EndText.GetComponent<Text>().text = "Поражение";
            gameObject.SetActive(false);
        }
    }
    void PlayerMove()
    {
        xMove = Input.GetAxisRaw("XMove");
        zMove = Input.GetAxisRaw("ZMove");
        if ((xMove == 1) && (gameObject.transform.position.x >= 50))
            xMove = 0;
        if ((xMove == -1) && (gameObject.transform.position.x <= -50))
            xMove = 0;
        if ((zMove == 1) && (gameObject.transform.position.z >= 50))
            zMove = 0;
        if ((zMove == -1) && (gameObject.transform.position.z <= -50))
            zMove = 0;
        if (xMove != 0 || zMove != 0)
        {
            animator.SetBool("Walking", true);
            transform.Translate(new Vector3(0, 0, 1) * speed * Time.fixedDeltaTime, Space.Self);
            Quaternion ToRotate = Quaternion.LookRotation(new Vector3(xMove, 0, zMove), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ToRotate, Rotatespeed * Time.deltaTime);
            if (animator.GetBool("RotateBool") == true) { animator.SetBool("RotateBool", false); }
        }
        else { animator.SetBool("Walking", false); }
        cam.GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 15, GetComponent<Transform>().position.z - 15);
    }
    void PlayerShootMode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, PlaneLayer))
        {
            if (animator.GetBool("RotateBool") == false) { animator.SetBool("RotateBool", true); }
            Vector3 NewDirection = Vector3.RotateTowards(transform.forward, hit.point - transform.position, 500 * Time.deltaTime, 0f);
            NewDirection.y = 0;
            rot = (transform.rotation.y - Quaternion.LookRotation(NewDirection).y)*10;
            rot = (float)Math.Round((decimal)rot, 4, MidpointRounding.AwayFromZero);
            if (rot < 0) { animator.SetBool("RotateSide", false); }
            else if (rot > 0) { animator.SetBool("RotateSide", true); }
            animator.SetFloat("Rotate", rot);
            transform.rotation = Quaternion.LookRotation(NewDirection);
            ShootingLine.GetComponent<LineRenderer>().SetPositions(new[] { transform.position, hit.point });
            ShootingLine.SetActive(true);
            if (Input.GetMouseButton(0) == true) PlayerShoot();
        }
    }
    void PlayerShoot()
    {
        Shooting = false;
        ShootingLine.SetActive(false);
        ShotPlayerBullet = SB;
        ShotPlayerBullet.GetComponent<PlayerBullet>().PlayerType = true;
        Instantiate(ShotPlayerBullet, new Vector3(transform.position.x, 0.5f, transform.position.z), transform.rotation);
    }
}