using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private List<CollectableItem> collectedItems = new();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectableItem item))
        {
            if (collectedItems.Contains(item)) return;
            item.FollowTarget = collectedItems.Count > 0 ? collectedItems[^1].transform : transform;
            collectedItems.Add(item);
            Debug.Log($"Collected {item.Name}!");
        }
        else if (other.TryGetComponent(out Island island))
        {
            if (island.CanBuildIsland(collectedItems))
                Debug.Log("Can build island!!!!! :D");
            else
                Debug.Log("Can't build this yet...");
        }
    }
}