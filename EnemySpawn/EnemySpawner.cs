using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int EnemyCount = 10;
    public List<GameObject> EnemyType = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyCount == 0)
        {
            GameObject.Find("NextRoomDoor").tag = "Finosh";
        }
    }

    public void Spawn(int height, int width)
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            int randEnemy = Random.Range(0, EnemyType.Count);
            GameObject enemy = Instantiate(EnemyType[randEnemy], new Vector3(Random.Range(transform.position.x + 1, transform.position.x+height - 1), Random.Range(transform.position.y + 1, transform.position.y+ width - 1), -1), Quaternion.identity);
            enemy.name = $"{EnemyType[randEnemy].name}";
            enemy.transform.parent = transform;
        }
    }
}
