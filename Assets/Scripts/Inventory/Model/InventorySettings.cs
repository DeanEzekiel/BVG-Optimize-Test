using UnityEngine;

namespace Inventory.Model
{

    [CreateAssetMenu(fileName = "New Inventory Settings",
        menuName = "Exam / New Inventory Settings")]
    public class InventorySettings : ScriptableObject
    {

        #region Inspector Fields

        [SerializeField]
        [Tooltip("The text file that contains the JSON of Inventory Items to be loaded.")]
        private TextAsset itemJson;

        [SerializeField]
        [Tooltip("This is used in generating the items list. The number of additional copies to concat the list parsed from ItemJson.")]
        private uint itemGenerateScale = 10;

        [SerializeField]
        [Tooltip(tooltip: "Icons referenced by ItemData.IconIndex when instantiating new items.")]
        private Sprite[] icons;

        #endregion //Inspector Fields

        #region Accessors

        public TextAsset ItemJson => itemJson;
        public uint ItemGenerateScale => itemGenerateScale;

        public Sprite[] Icons => icons;

        #endregion

    }

}