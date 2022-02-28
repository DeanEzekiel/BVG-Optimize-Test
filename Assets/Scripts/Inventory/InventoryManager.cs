using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ren.Misc;

public class InventoryManager : MonoBehaviour
{

    #region Inspector Fields

    public InventoryInfoPanel InfoPanel;
    public InventoryItemView InventoryItemPrefab;

    [Space]

    public ScrollRectVerticalContentTracker ScrollTracker;

    public ContentSizeFitter fitter;
    public VerticalLayoutGroup layoutGroup;

    [Space]

    public GameObject Container;

    [Tooltip(tooltip:"Loads the list using this format.")]
    [Multiline]
    public string ItemJson;

    [Tooltip(tooltip:"This is used in generating the items list. The number of additional copies to concat the list parsed from ItemJson.")]
    public int ItemGenerateScale = 10;

    [Tooltip(tooltip:"Icons referenced by ItemData.IconIndex when instantiating new items.")]
    public Sprite[] Icons;

    #endregion

    private InventoryItemView cachedSelectedItem;

    #region Unity Callbacks

    private void Awake()
    {
        var ItemDatas = GenerateItemDatas(ItemJson, ItemGenerateScale);
        var firstItem = InstantiateAllItems(ItemDatas);

        // Select the first item.
        InventoryItemOnClick(firstItem);

        Destroy(InventoryItemPrefab.gameObject);
        Icons = null;
        ItemJson = null;
    }

    private void Start()
    {
        ScrollTracker.TriggerRangeUpdate();
        Application.targetFrameRate = 120;
        StartCoroutine(C_OffContent());
    }

    #endregion

    #region Class Implementation

    /// <summary>
    /// Instantiate items in the Scroll View.
    /// </summary>
    private InventoryItemView InstantiateAllItems(InventoryItemData[] ItemDatas)
    {
        if (ItemDatas == null)
        {
            return null;
        }

        InventoryItemView firstItem = null;
        foreach (InventoryItemData itemData in ItemDatas)
        {
            var newItem = Instantiate(InventoryItemPrefab, Container.transform);
            newItem.SetUpView(itemData, this);

            if (firstItem == null)
            {
                firstItem = newItem;
            }
        }

        return firstItem;
    }

    private IEnumerator C_OffContent()
    {
        yield return new WaitForEndOfFrame();
        Destroy(fitter);
        Destroy(layoutGroup);
    }

    /// <summary>
    /// Generates an item list.
    /// </summary>
    /// <param name="json">JSON to generate items from. JSON must be an array of InventoryItemData.</param>
    /// <param name="scale">Concats additional copies of the array parsed from json.</param>
    /// <returns>An array of InventoryItemData</returns>
    private InventoryItemData[] GenerateItemDatas(string json, int scale)
    {
        var itemDatas = JsonUtility.FromJson<InventoryItemDatas>(json).ItemDatas;
        var finalItemDatas = new InventoryItemData[itemDatas.Length * scale];
        for (var i = 0; i < itemDatas.Length; i++)
        {
            for (var j = 0; j < scale; j++)
            {
                finalItemDatas[i + j * itemDatas.Length] = itemDatas[i];
            }
        }

        return finalItemDatas;
    }

    public void InventoryItemOnClick(InventoryItemView itemClicked)
    {
        if (cachedSelectedItem != null)
        {
            cachedSelectedItem.SetSelected(false);
        }

        if (itemClicked == null)
        {
            return;
        }

        itemClicked.SetSelected(true);
        cachedSelectedItem = itemClicked;
        InfoPanel.DisplayInfo(itemClicked.ItemData);
    }

    #endregion

}
