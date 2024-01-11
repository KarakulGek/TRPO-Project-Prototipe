using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public int Count;
    public GameObject Enemy;
    public GameObject EnemiesCount;
    public List<GameObject> Enemies;
    public int EnemiesLeft;
    public GameObject EndText;

    void Start()
    {
        EnemiesLeft = 10;
        EnemiesCountUpdate();
        StartCoroutine(TimerCount());
    }

    IEnumerator TimerCount()
    {
        yield return new WaitForSecondsRealtime(1);
        Count += 1;
        gameObject.GetComponent<Text>().text = Count.ToString();
        if ((Count % 5 == 0) && (Enemies.Count < 5) && (Enemies.Count < EnemiesLeft))
        {
            Enemies.Add(Instantiate(Enemy, new Vector3(Random.Range(-20,20),0.5f, Random.Range(-20, 20)), new Quaternion(0,0,0,0)));
        }
        StartCoroutine(TimerCount());
    }
    public void EnemiesCountUpdate()
    {
        EnemiesCount.GetComponent<Text>().text = EnemiesLeft.ToString();
        if (EnemiesLeft == 0)
            EndText.GetComponent<Text>().text = "Победа";
    }
}
