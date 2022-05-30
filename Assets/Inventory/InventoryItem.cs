using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Image Background;
    public Image Icon;
    public TextMeshProUGUI Name;
    public Button Button;

    // DGS03 START
    // for determining the position and will determine if this should be active
    private RectTransform rect;

    #region Unity Callbacks
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        InventoryManager.OnScroll += OnScrollViewChange;
    }

    private void OnApplicationQuit()
    {
        InventoryManager.OnScroll -= OnScrollViewChange;
    }
    #endregion // Unity Callbacks

    #region Implementation
    private void OnScrollViewChange(float min, float max)
    {
        var posY = Mathf.Abs(rect.localPosition.y);
        gameObject.SetActive(posY >= min && posY <= max);
    }
    #endregion // Implementation

    // DGS03 END
}
