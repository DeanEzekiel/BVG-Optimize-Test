using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    #region Inspector Fields

    [SerializeField]
    private Image imageBackground;
    [SerializeField]
    private Image imageIcon;
    [SerializeField]
    private TextMeshProUGUI textName;
    [SerializeField]
    private Button button;

    #endregion

    private InventoryItemData itemData;
    public InventoryItemData ItemData => itemData;

    #region Public API

    public void SetUpView(
        InventoryItemData itemData, 
        InventoryManager manager)
    {
        if (itemData == null || manager == null)
        {
            return;
        }

        this.itemData = itemData;
        imageIcon.sprite = manager.Icons[itemData.IconIndex];
        textName.text = itemData.Name;
        transform.SetParent(manager.Container.transform);
        gameObject.SetActive(true);
        button.onClick.AddListener(() => { manager.InventoryItemOnClick(this); });
        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        imageBackground.color = isSelected ? Color.red : Color.white;
    }

    #endregion

}
