using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandInstance : MonoBehaviour
{
    public GameObject[] prefabs;
    public BoxCollider area;
    public GameObject FlyingFruits;
    float timer = 0;

    private List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider>();
        for (int i = 0; i < 2; i++)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            Spawn();
        }
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }
    private void Spawn()
    {
        int selection = Random.Range(0, prefabs.Length);

        GameObject selectedPrefab = prefabs[selection];

        Vector3 spawnPos = GetRandomPosition();//랜덤위치함수

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        Vector3 Pos = instance.transform.position;
        Vector3 thro = new Vector3(0, 4, 0);
        instance.transform.parent = FlyingFruits.transform;
        instance.GetComponent<Rigidbody>().velocity = -Pos+thro;
        Destroy(instance, 2f);
        gameObjects.Add(instance);

    }
}
