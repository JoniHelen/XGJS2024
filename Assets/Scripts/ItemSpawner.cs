using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float SpawnRange;
    [SerializeField] private int ItemAmount;
    [SerializeField] private List<CollectableItem> ItemPrefabs;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ItemAmount; i++)
        {
            float randomAngle = Random.Range(0.0f, Mathf.PI * 2);
            float randomLength = Random.Range(5.0f, SpawnRange);
            Vector2 randomDir = new(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            randomDir *= randomLength;

            Vector3 randomPosition = new(randomDir.x, 0.0f, randomDir.y);
            CollectableItem randomObject = ItemPrefabs[Random.Range(0, ItemPrefabs.Count)];

            Instantiate(randomObject, randomPosition, randomObject.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}