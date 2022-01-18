using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        private static VisualElement m_GhostIcon;
        private static bool isDragging;
        private static InventorySlotElement m_OriginalSlot;

        private readonly List<InventorySlotElement> inventoryItems = new();
        private readonly List<InventorySlotElement> navbarItems = new();

        private VisualElement root;
        private VisualElement slotContainer;

        private void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            m_GhostIcon = root.Q<VisualElement>("GhostContainer");

            slotContainer = root.Q<VisualElement>("SlotContainer");

            foreach (InventorySlotElement element in slotContainer.Children())
            {
                inventoryItems.Add(element);
            }

            var secondaryContainer = root.Q<VisualElement>("SecondarySlotContainer");

            foreach (InventorySlotElement element in secondaryContainer.Children())
            {
                navbarItems.Add(element);
            }

            GameManager.OnInventoryChanged += GameController_OnInventoryChanged;
            m_GhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            m_GhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!isDragging) return;
            m_GhostIcon.style.top = evt.position.y - m_GhostIcon.layout.height / 2;
            m_GhostIcon.style.left = evt.position.x - m_GhostIcon.layout.width / 2;
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!isDragging) return;
            var slots = inventoryItems.Where(x => x.worldBound.Overlaps(m_GhostIcon.worldBound)).ToList();
            if (!slots.Any())
            {
                m_OriginalSlot.icon.image = GameManager.GetItemByGuid(m_OriginalSlot.itemGuid).Icon.texture;
                var navbarSlots = navbarItems.Where(x => x.worldBound.Overlaps(m_GhostIcon.worldBound)).ToList();
                if (navbarSlots.Any())
                {
                    var closestSlot = navbarSlots
                                      .OrderBy(x => Vector2.Distance(x.worldBound.position,
                                                                     m_GhostIcon.worldBound.position))
                                      .First();
                    closestSlot.HoldItem(GameManager.GetItemByGuid(m_OriginalSlot.itemGuid));
                }
            }
            else
            {
                var closestSlot = slots
                                  .OrderBy(x => Vector2.Distance(x.worldBound.position,
                                                                 m_GhostIcon.worldBound.position))
                                  .First();
                if (closestSlot.itemGuid == "")
                {
                    closestSlot.HoldItem(GameManager.GetItemByGuid(m_OriginalSlot.itemGuid));
                    m_OriginalSlot.DropItem();
                }
                else
                {
                    m_OriginalSlot.icon.image = GameManager.GetItemByGuid(m_OriginalSlot.itemGuid).Icon.texture;
                }
            }

            isDragging = false;
            m_OriginalSlot = null;
            m_GhostIcon.visible = false;
        }

        private void GameController_OnInventoryChanged(string[] itemGuid)
        {
            foreach (string item in itemGuid)
            {
                var emptySlot = inventoryItems.FirstOrDefault(x => x.itemGuid.Equals(""));
                emptySlot?.HoldItem(GameManager.GetItemByGuid(item));
            }
        }

        public static void StartDrag(Vector2 position, InventorySlotElement originalSlot)
        {
            isDragging = true;
            m_OriginalSlot = originalSlot;
            m_GhostIcon.style.top = position.y - m_GhostIcon.layout.height / 2;
            m_GhostIcon.style.left = position.x - m_GhostIcon.layout.width / 2;

            m_GhostIcon.style.backgroundImage = GameManager.GetItemByGuid(originalSlot.itemGuid).Icon.texture;
            m_GhostIcon.style.visibility = Visibility.Visible;
        }
    }
}