using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Island : MonoBehaviour
{
    [System.Serializable]
    public struct ItemInfo
    {
        public CollectableItem.ItemType Type;
        public int Count;
        
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
    
    public List<ItemInfo> RequiredItems;

    [SerializeField] private TMP_Text text;


    private void Awake()
    {
        text.text = "Items required:";

        foreach (var item in RequiredItems)
        {
            text.text += $" {item.Count}x {item.Name}";
        }
    }

    public bool CanBuildIsland(List<CollectableItem> items)
    {
        return RequiredItems.All(item => items.Exists(i => i.Type == item.Type && i.Count == item.Count));
    }
}