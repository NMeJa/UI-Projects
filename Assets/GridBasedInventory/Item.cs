using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GridBasedInventory
{
    [CreateAssetMenu(menuName = "Create Item", fileName = "Item", order = 0)]
    public class Item : ScriptableObject
    {
        public string ID = Guid.NewGuid().ToString();
        public Sprite Icon;
        public Vector2Int size;
    }

    public class ItemVisual : VisualElement
    {
        private VisualElement root;

        public ItemVisual()
        {
            root = this;
        }
    }
}
