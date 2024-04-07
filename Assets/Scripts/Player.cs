using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform PanelParent;
    [SerializeField] private ItemDisplayPanel inventoryPanelPrefab;
    [SerializeField] private AudioSource collectSound;
    private List<ItemDisplayPanel> inventoryDisplays = new();
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
            if (item.Collected) return;
            
            item.Collected = true;
            
            if (collectedItems.Exists(i => i.Info.Type == item.Info.Type))
            {
                var old = collectedItems.Find(i => i.Info.Type == item.Info.Type);
                old.Info.Count.Value += item.Info.Count.Value;
                Destroy(item.gameObject);
            }
            else
            {
                if (collectedItems.Count > 0)
                {
                    var leader = collectedItems[^1];
                    item.FollowTarget = leader.transform;
                    item.Leader = leader;
                    leader.Follower = item;
                }
                else
                {
                    item.FollowTarget = transform;
                }
                
                item.Info.Count.Subscribe(value => {
                    if (value > 0) return;
                    collectedItems.Remove(item);
                    if (item.Follower != null)
                    {
                        item.Follower.FollowTarget = item.FollowTarget;
                        
                        if (item.Leader != null)
                            item.Follower.Leader = item.Leader;
                    }
                    Destroy(item.gameObject);
                }).AddTo(item);
                
                collectedItems.Add(item);
                
                var display = Instantiate(inventoryPanelPrefab, PanelParent);
                display.UpdateDisplay(item);
                
                item.Info.Count.Subscribe(value => {
                    display.UpdateDisplay(item);
                    if (value > 0) return;
                    inventoryDisplays.Remove(display);
                    Destroy(display.gameObject);
                }).AddTo(item);
                
                inventoryDisplays.Add(display);
            }
            
            collectSound.PlayOneShot(collectSound.clip);
            Debug.Log($"Collected {item.Info.Name}!");
        }
        else if (other.TryGetComponent(out Island island))
        {
            if (island.CanBuildIsland(collectedItems))
            {
                Debug.Log("Can build island!!!!! :D");
                
                island.RequiredItems.ForEach(ri => {
                    collectedItems.Find(i => i.Info.Type == ri.Type).Info.Count.Value -= ri.Count.Value;
                });
                
                island.Build();
            }
            else
                Debug.Log("Can't build this yet...");
        }
    }
}