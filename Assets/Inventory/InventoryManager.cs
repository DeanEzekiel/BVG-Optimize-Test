using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventoryInfoPanel InfoPanel;
    public InventoryItem InventoryItemPrefab;

    public GameObject Container;

    [Tooltip(tooltip:"Loads the list using this format.")]
    [Multiline]
    public string ItemJson;

    [Tooltip(tooltip:"This is used in generating the items list. The number of additional copies to concat the list parsed from ItemJson.")]
    public int ItemGenerateScale = 10;

    [Tooltip(tooltip:"Icons referenced by ItemData.IconIndex when instantiating new items.")]
    public Sprite[] Icons;

    [Serializable]
    private class InventoryItemDatas
    {
        public InventoryItemData[] ItemDatas;
    }

    private InventoryItemData[] ItemDatas;

    private List<InventoryItem> Items;

    // DGS03 FPS
    [SerializeField]
    private TextMeshProUGUI fpsText;
    public string Prefix = "FPS: ";
    private float cachedTime;
    private float refreshInterval = 1f;

    // DGS03 START - Scrolling determination whether to activate item or not
    [SerializeField]
    [Range(0f, 500f)]
    [Tooltip("Additional value for the range of content doled out by this script to its subscribers.")]
    private float buffer = 500f;

    [SerializeField]
    private ScrollRect scrollRect;
    private float scrollRectHeight;
    public static event Action<float, float> OnScroll;

    private void Awake()
    {
        scrollRect.onValueChanged.AddListener(OnScrollViewChange);
        scrollRectHeight = scrollRect.GetComponent<RectTransform>().rect.height;
    }

    private IEnumerator C_FinalizeScrollContent()
    {
        yield return new WaitForEndOfFrame();
        RemoveScrollContentDynamicitySetup();
        OnScrollViewChange(Vector2.zero);
    }

    private void RemoveScrollContentDynamicitySetup()
    {
        if (scrollRect.content.gameObject.TryGetComponent<ContentSizeFitter>(
            out var fitter))
        {
            Destroy(fitter);
        }
        if (scrollRect.content.gameObject.TryGetComponent<VerticalLayoutGroup>(
            out var vLayoutGroup))
        {
            Destroy(vLayoutGroup);
        }
    }

    private void OnScrollViewChange(Vector2 position)
    {
        var beginning = scrollRect.content.localPosition.y;
        var minRange = beginning - buffer;
        var maxRange = beginning + scrollRectHeight + buffer;

        OnScroll?.Invoke(minRange, maxRange);
    }

    // DGS03 END

    void Start()
    {
        // Clear existing items already in the list.
        var items = Container.GetComponentsInChildren<InventoryItem>();
        foreach (InventoryItem item in items) {
            item.gameObject.transform.SetParent(null);
        }

        ItemDatas = GenerateItemDatas(ItemJson, ItemGenerateScale);

        // Instantiate items in the Scroll View.
        Items = new List<InventoryItem>();
        foreach (InventoryItemData itemData in ItemDatas) {
            var newItem = GameObject.Instantiate<InventoryItem>(InventoryItemPrefab);
            newItem.Icon.sprite = Icons[itemData.IconIndex];
            newItem.Name.text = itemData.Name;
            newItem.transform.SetParent(Container.transform);
            newItem.Button.onClick.AddListener(() => { InventoryItemOnClick(newItem, itemData); });
            Items.Add(newItem);       
        }

        // Select the first item.
        InventoryItemOnClick(Items[0], ItemDatas[0]);

        // DGS03
        StartCoroutine(C_FinalizeScrollContent());

        //DGS03 time
    }

    // DGS03 START - FPS Check
    private void Update()
    {
        RefreshFPSText();
    }

    private void RefreshFPSText()
    {
        if(cachedTime == 0f)
        {
            cachedTime = Time.unscaledTime + refreshInterval;
        }
        if(Time.unscaledTime > cachedTime)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = Prefix + fps.ToString();
            print(fpsText.text);

            print($"Average Frame rate: {Time.frameCount / Time.time}");

            cachedTime = Time.unscaledTime + refreshInterval;
        }
    }
    // DGS03 END - FPS Check

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
        for (var i = 0; i < itemDatas.Length; i++) {
            for (var j = 0; j < scale; j++) {
                finalItemDatas[i + j*itemDatas.Length] = itemDatas[i];
            }
        }

        return finalItemDatas;
    }

    private void InventoryItemOnClick(InventoryItem itemClicked, InventoryItemData itemData) 
    {
        foreach (var item in Items) {
            item.Background.color = Color.white;
        }
        itemClicked.Background.color = Color.red;

        // DGS01 Start - Show the details of the item clicked on the info panel
        InfoPanel.Icon.sprite = itemClicked.Icon.sprite;
        InfoPanel.NameText.text = itemData.Name;
        InfoPanel.DescriptionText.text = itemData.Description;
        InfoPanel.StatText.text = itemData.Stat.ToString();
        // DGS01 End
    }
}
