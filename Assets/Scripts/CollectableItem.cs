using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

public class CollectableItem : MonoBehaviour
{
    public enum ItemType 
    {
        Board, SlidePiece, Parasol, Thatch, Nails, Hammer, Saw, Brimstone, Sand, TreasureChest, Coins, Jewels
    }
    
    [Serializable]
    public record ItemInfo
    {
        public ItemType Type;
        public ReactiveProperty<int> Count;
        public Sprite Icon;
        
        public string Name => Type switch
        {
            CollectableItem.ItemType.Board => "Board",
            CollectableItem.ItemType.SlidePiece => "Slide Piece",
            CollectableItem.ItemType.Parasol => "Parasol",
            CollectableItem.ItemType.Thatch => "Thatch",
            CollectableItem.ItemType.Nails => "Nails",
            CollectableItem.ItemType.Hammer => "Hammer",
            CollectableItem.ItemType.Saw => "Saw",
            CollectableItem.ItemType.Brimstone => "Brimstone",
            CollectableItem.ItemType.Sand => "Sand",
            CollectableItem.ItemType.TreasureChest => "Treasure Chest",
            CollectableItem.ItemType.Coins => "Coins",
            CollectableItem.ItemType.Jewels => "Jewels",
            _ => ""
        };
    }
    
    public Transform FollowTarget;
    public CollectableItem Follower;
    public CollectableItem Leader;
    public float DistanceDelta = 0.5f;
    public float FloatingAmount = 0.1f;
    public ItemInfo Info;
    public bool Collected = false;

    private float sineSeed = 0;
    
    public static implicit operator ItemInfo(CollectableItem c) => c.Info;

    private void Awake()
    {
        FollowTarget = transform;
        sineSeed = Random.Range(0.0f, 15299.0f);
    }

    private void Update()
    {
        Vector3 delta = FollowTarget.position - transform.position;
        delta -= delta.normalized * DistanceDelta;
        delta.y = 0; // just to be safe
        
        transform.position = Vector3.MoveTowards(transform.position, transform.position + delta,
            Time.deltaTime * delta.magnitude * 8.0f);

        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 2.0f + sineSeed) * FloatingAmount, transform.position.z);
    }
}