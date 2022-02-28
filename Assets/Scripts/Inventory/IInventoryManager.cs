using Ren.Misc;

public interface IInventoryManager
{

    ScrollRectVerticalContentTracker GetScrollTracker();
    InventorySettings GetSettings();

    void OnClickInventoryItem(InventoryItemView itemClicked);

}
