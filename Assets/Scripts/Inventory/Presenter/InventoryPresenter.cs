using UnityEngine;
using Ren.Misc;
using Inventory.Model;
using Inventory.View;

namespace Inventory.Presenter
{

    /// <summary>
    /// Contains all the logical operations pertaining to Inventory items.
    /// [1] Loads and initializes list items views,
    /// [2] manages list items' selected/unselected states,
    /// [3] executes the display of a selected list item's full details.
    /// 
    /// -Renelie Salazar
    /// </summary>
    public class InventoryPresenter : MonoBehaviour, IInventoryPresenter
    {

        #region Inspector Fields

        [SerializeField]
        private InventorySettings settings;

        [Space]

        [SerializeField]
        private InventoryInfoPanelView viewInfoPanel;
        [SerializeField]
        private InventoryListItemView viewInventoryItem;
        [SerializeField]
        private ScrollRectVerticalContentTracker scrollTracker;

        #endregion //Inspector Fields

        #region Private Fields

        /// <summary>
        /// The cached reference of the previously highlighted inventory item.
        /// </summary>
        private InventoryListItemView previousSelectedItem;

        #endregion //Private Fields

        #region Unity Callbacks

        private void Awake()
        {
            var ItemDatas = GenerateItemDatas(settings.ItemJson.text,
                (int)settings.ItemGenerateScale);
            var firstItem = InstantiateAllItems(ItemDatas);

            // Select the first item.
            OnClickInventoryItem(firstItem);

            Destroy(viewInventoryItem.gameObject);
        }

        private void Start()
        {
            scrollTracker.FinalizeScrollContentAndTriggerRangeUpdate();
        }

        #endregion //Unity Callbacks

        #region IInventoryManager Implementations

        public ScrollRectVerticalContentTracker GetScrollTracker() => scrollTracker;
        public InventorySettings GetSettings() => settings;

        public void OnClickInventoryItem(InventoryListItemView itemClicked)
        {
            if (previousSelectedItem != null)
            {
                previousSelectedItem.SetSelected(false);
            }

            if (itemClicked == null)
            {
                return;
            }

            itemClicked.SetSelected(true);
            previousSelectedItem = itemClicked;
            viewInfoPanel.DisplayInfo(itemClicked.ItemData, this);
        }

        #endregion

        #region Class Implementation

        /// <summary>
        /// Instantiate items in the Scroll View.
        /// </summary>
        private InventoryListItemView InstantiateAllItems(InventoryItemData[] ItemDatas)
        {
            if (ItemDatas == null)
            {
                return null;
            }

            InventoryListItemView firstItem = null;
            foreach (InventoryItemData itemData in ItemDatas)
            {
                var newItem = Instantiate(viewInventoryItem,
                    scrollTracker.ScrollRect.content.transform);
                newItem.SetUpView(itemData, this);

                if (firstItem == null)
                {
                    firstItem = newItem;
                }
            }

            return firstItem;
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

        #endregion //Class Implementation

    }

}