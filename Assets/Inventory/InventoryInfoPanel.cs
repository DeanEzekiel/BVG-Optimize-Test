using UnityEngine;
using TMPro;

public class InventoryInfoPanel : MonoBehaviour
{

    #region Inspector Fields

    [SerializeField]
    private TextMeshProUGUI textName;

    [SerializeField]
    private TextMeshProUGUI textDescription;

    [SerializeField]
    private TextMeshProUGUI textStats;

    #endregion

    public void DisplayInfo(InventoryItemData itemData)
    { 
        if(itemData == null)
        {
            return;
        }

        textName.text = itemData.Name;
        textDescription.text = itemData.Description;
        textStats.text = itemData.Stat.ToString();
    }

}
