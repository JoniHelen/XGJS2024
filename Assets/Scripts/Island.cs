using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Island : MonoBehaviour
{
    [SerializeField] private ItemDisplayPanel DisplayPanelPrefab;
    [SerializeField] private Transform PanelParent;
    [SerializeField] private GameObject IslandModel;
    [SerializeField] private GameObject BuoyModel;
    [SerializeField] private TMP_Text buildText;
    [SerializeField] private AudioSource BuildSound;
    public List<CollectableItem.ItemInfo> RequiredItems;
    public List<ItemDisplayPanel> DisplayPanels;

    private bool built;
    
    private void Awake()
    {
        for (var i = 0; i < RequiredItems.Count; i++)
        {
            RequiredItems[i].Count.Value = Random.Range(5, 8);
            DisplayPanels.Add(Instantiate(DisplayPanelPrefab, PanelParent));
            DisplayPanels[i].UpdateDisplay(RequiredItems[i]);
        }
    }

    public bool CanBuildIsland(List<CollectableItem> items)
    {
        return RequiredItems.All(item => items.Exists(i => i.Info.Type == item.Type && i.Info.Count.Value >= item.Count.Value)) && !built;
    }
    
    private float easeOutBack(float x) {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1.0f;

        return 1 + c3 * Mathf.Pow(x - 1.0f, 3.0f) + c1 * Mathf.Pow(x - 1.0f, 2.0f);
    }
    
    private float easeOutElastic(float x) {
        const float c4 = (2.0f * Mathf.PI) / 3.0f;

        return x switch
        {
            0 => 0,
            1 => 1,
            _ => Mathf.Pow(2.0f, -10.0f * x) * Mathf.Sin((x * 10.0f - 0.75f) * c4) + 1.0f
        };
    }

    private IEnumerator IslandLerp()
    {
        float t = 0;

        while (t < 1)
        {
            IslandModel.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, new Vector3(0.04f, 0.03f, 0.04f), easeOutElastic(t));
            t += Time.deltaTime;
            yield return null;
        }

        IslandModel.transform.localScale = new Vector3(0.04f, 0.03f, 0.04f);
    }

    public void Build()
    {
        if (built) return;
        built = true;
        BuoyModel.SetActive(false);
        IslandModel.transform.localScale = Vector3.zero;
        IslandModel.SetActive(true);
        PanelParent.gameObject.SetActive(false);
        StartCoroutine(IslandLerp());
        BuildSound.Play();
        buildText.text = "Build the island! 1/1";
    }
}