using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
    public ItemInfo Info;
    public bool Collected = false;
    
    public static implicit operator ItemInfo(CollectableItem c) => c.Info;

    private void Awake()
    {
        FollowTarget = transform;
    }

    private void Update()
    {
        Vector3 delta = FollowTarget.position - transform.position;

        if (delta.magnitude <= DistanceDelta + Time.deltaTime * 8.0f) return;
        
        delta.y = 0; // just to be safe
        
        transform.Translate(Time.deltaTime * 5.0f * delta, Space.World);
    }
}