using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Island : MonoBehaviour
{
    /*public enum IslandType
    {
        Leisure, Hut, Volcanic, Treasure
    }

    public IslandType Type;*/
    [SerializeField] private ItemDisplayPanel DisplayPanelPrefab;
    [SerializeField] private Transform PanelParent;
    public List<CollectableItem.ItemInfo> RequiredItems;
    public List<ItemDisplayPanel> DisplayPanels;
    
    private void Awake()
    {
        for (var i = 0; i < RequiredItems.Count; i++)
        {
            RequiredItems[i].Count.Value = Random.Range(1, 6);
            DisplayPanels.Add(Instantiate(DisplayPanelPrefab, PanelParent));
            DisplayPanels[i].UpdateDisplay(RequiredItems[i]);
        }
    }

    public bool CanBuildIsland(List<CollectableItem> items)
    {
        return RequiredItems.All(item => items.Exists(i => i.Info.Type == item.Type && i.Info.Count.Value >= item.Count.Value));
    }

    public void Build()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
}