using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float SpawnRange;
    [SerializeField] private int ItemAmount;
    [SerializeField] private List<CollectableItem> ItemPrefabs;

    private int item1Count = 0;
    private int item2Count = 0;
    private int item3Count = 0;
    private int item4Count = 0;
    private int item5Count = 0;
    
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

            int rndIndex = Random.Range(0, ItemPrefabs.Count);
            
            switch (rndIndex)
            {
                case 0:
                    item1Count++;
                    break;
                case 1:
                    item2Count++;
                    break;
                case 2:
                    item3Count++;
                    break;
                case 3:
                    item4Count++;
                    break;
                case 4:
                    item5Count++;
                    break;
            }
            
            CollectableItem randomObject = ItemPrefabs[rndIndex];

            Instantiate(randomObject, randomPosition, randomObject.transform.rotation);

            // 0.0000000000001 chance for deadlock
            if (i == ItemAmount - 1 && (item1Count < 7 || item2Count < 7 || item3Count < 7 || item4Count < 7 || item5Count < 7))
            {
                i--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}