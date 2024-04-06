using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image itemThumbnail;
    public CollectableItem.ItemType Type;

    public void UpdateDisplay(CollectableItem.ItemInfo itemInfo)
    {
        itemThumbnail.sprite = itemInfo.Icon;
        countText.text = itemInfo.Count.Value.ToString();
        Type = itemInfo.Type;
    }
}