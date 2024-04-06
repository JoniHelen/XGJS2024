using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public enum ItemType 
    {
        Board, SlidePiece, Parasol, Thatch, Nails, Hammer, Saw, Brimstone, Sand, TreasureChest, Coins, Jewels
    }

    public Transform FollowTarget;
    public ItemType Type;
    public float DistanceDelta = 0.5f;
    public int Count = 1;
    
    public string Name => Type switch
    {
        ItemType.Board => "Board",
        ItemType.SlidePiece => "Slide Piece",
        ItemType.Parasol => "Parasol",
        ItemType.Thatch => "Thatch",
        ItemType.Nails => "Nails",
        ItemType.Hammer => "Hammer",
        ItemType.Saw => "Saw",
        ItemType.Brimstone => "Brimstone",
        ItemType.Sand => "Sand",
        ItemType.TreasureChest => "Treasure Chest",
        ItemType.Coins => "Coins",
        ItemType.Jewels => "Jewels",
        _ => ""
    };

    private void Awake()
    {
        FollowTarget = transform;
    }

    private void Update()
    {
        Vector3 delta = FollowTarget.position - transform.position;

        if (delta.magnitude <= DistanceDelta + Time.deltaTime * 8.0f) return;
        
        delta.y = 0; // just to be safe
        
        transform.Translate(Time.deltaTime * 8.0f * delta.normalized, Space.Self);
    }
}