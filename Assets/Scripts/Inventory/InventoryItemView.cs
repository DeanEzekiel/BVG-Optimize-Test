using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{

    #region Inspector Fields

    [Header("UI Elements")]

    [SerializeField]
    private Image imageIcon;
    [SerializeField]
    private TextMeshProUGUI textName;
    [SerializeField]
    private Button button;

    [Header("Color States")]

    [SerializeField]
    private Color colorIsActive = Color.red;
    [SerializeField]
    private Color colorIsInactive = Color.white;

    #endregion //Inspector Fields

    #region Private Fields

    private RectTransform rectTransform;
    private InventoryItemData itemData;

    #endregion //Private Fields

    #region Accessors

    public InventoryItemData ItemData => itemData;

    #endregion //Accessors

    #region Unity Callbacks

    private void Awake()
    {
        Init();
        CheckUIElements();
    }

    #endregion //Unity Callbacks

    #region Public API

    /// <summary>
    /// Prepares this item's content and 
    /// </summary>
    /// <param name="itemData">Contains the data to be shown to the user.</param>
    /// <param name="manager">The class to be referenced for additional data processes and updates.</param>
    public void SetUpView(InventoryItemData itemData, InventoryManager manager)
    {
        if (itemData == null || manager == null)
        {
            Debug.LogWarning($"{GetType().Name}: Cannot set up this view if " +
                $"ItemData and/or Manager is missing.", gameObject);
            return;
        }

        this.itemData = itemData;

        imageIcon.sprite = manager.Icons[itemData.IconIndex];
        textName.text = itemData.Name;
        button.onClick.AddListener(() => manager.InventoryItemOnClick(this));
        SetSelected(false);

        gameObject.SetActive(true);
        manager.ScrollTracker.SubscribeToOnViewRangeChange(OnViewRangeChange);
    }

    public void SetSelected(bool isSelected)
    {
        button.targetGraphic.color = isSelected ? colorIsActive : colorIsInactive;
    }

    #endregion //Public API

    #region Class Implementation

    private void Init()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void CheckUIElements()
    {
        if (imageIcon == null || textName == null || button == null)
        {
            Debug.LogWarning($"Missing UI Element(s). " +
                $"Some behaviours may not work properly.", gameObject);
        }
    }

    /// <summary>
    /// Sets this Item to active state if it is positioned somewhere within the
    /// viewable range of contents. Otherwise, sets this Item inactive to optimize
    /// performance.
    /// </summary>
    private void OnViewRangeChange(float minRange, float maxRange)
    {
        var posY = Mathf.Abs(rectTransform.localPosition.y);
        gameObject.SetActive((posY >= minRange && posY <= maxRange));
    }

    #endregion //Class Implementation

}
