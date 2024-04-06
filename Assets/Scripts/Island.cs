using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class Island : MonoBehaviour
{
    [System.Serializable]
    public struct ItemInfo
    {
        public CollectableItem.ItemType Type;
        public int Count;
    }
    
    public List<ItemInfo> RequiredItems;

    public bool CanBuildIsland(List<CollectableItem> items)
    {
        return RequiredItems.All(item => items.Exists(i => i.Type == item.Type && i.Count == item.Count));
    }
}