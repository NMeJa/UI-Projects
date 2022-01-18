using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Inventory
{
    public class InventorySlotElement : VisualElement
    {
        public readonly Image icon;
        public string itemGuid = "";

        public InventorySlotElement()
        {
            //Create a new Image element and add it to the root
            icon = new Image();
            Add(icon);
            //Add USS style properties to the elements
            icon.AddToClassList("slotIcon");
            AddToClassList("slotContainer");
            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || itemGuid == "") return;
            icon.image = null;
            icon.sprite = null;
            InventoryUIController.StartDrag(evt.position, this);
        }

        public void HoldItem(ItemDetails details)
        {
            icon.sprite = details.Icon;
            itemGuid = details.GUID;
        }

        public void DropItem()
        {
            itemGuid = "";
            icon.sprite = null;
        }

        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<InventorySlotElement, UxmlTraits> { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        #endregion
    }
}