using UnityEngine;
using TMPro;
using Inventory.Model;

namespace Inventory.View
{

    public class InventoryInfoPanelView : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        private TextMeshProUGUI textName;

        [SerializeField]
        private TextMeshProUGUI textDescription;

        [SerializeField]
        private TextMeshProUGUI textStats;

        #endregion

        #region Public API

        public void DisplayInfo(InventoryItemData itemData)
        {
            if (itemData == null)
            {
                return;
            }

            textName.text = itemData.Name;
            textDescription.text = itemData.Description;
            textStats.text = itemData.Stat.ToString();
        }

        #endregion //Public API

    }

}