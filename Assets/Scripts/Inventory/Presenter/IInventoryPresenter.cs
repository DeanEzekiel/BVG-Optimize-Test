using Inventory.Model;
using Inventory.View;
using Ren.Misc;

namespace Inventory.Presenter
{

    public interface IInventoryPresenter
    {

        ScrollRectVerticalContentTracker GetScrollTracker();
        InventorySettings GetSettings();

        void OnClickInventoryItem(InventoryListItemView itemClicked);

    }

}