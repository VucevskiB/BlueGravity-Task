using BlueGravity.Interview.Controls;
using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BlueGravity.Interview.Inventory
{
    /// <summary>
    /// Component used to control the UI for the Inventory System;
    /// Storing and showing items in inventory;
    /// Action bar / Consume item thru mouse click;
    /// Drag and drop system for moving items around the inventory;
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _itemSlotUIPrefab;

        [SerializeField]
        private GameObject _equipBar;

        [SerializeField]
        private GameObject _inventoryWindow;

        [SerializeField]
        private GameObject _inventoryWindowContainer;

        [SerializeField]
        private List<InventorySlotUIElement> _itemSlotsGOList;

        [SerializeField]
        private Image _mouseDragIcon;

        private bool _isDragging;
        private InventoryUIStartedDraggingEvent _draggingEventData;

        private bool _windowOpened;
        private void Awake()
        {
            EventMessenger.Instance.AddListener<InventoryGeneratedEvent>(OnInventoryGenerated);
            EventMessenger.Instance.AddListener<InventoryLoadedEvent>(OnInventoryLoadedEvent);
            EventMessenger.Instance.AddListener<InventoryUpdatedEvent>(OnInventoryUpdated);

            EventMessenger.Instance.AddListener<InventoryUIStartedDraggingEvent>(OnInventoryUIStartedDragging);
            EventMessenger.Instance.AddListener<InventoryUIEndDragEvent>(OnInventoryUIEndDragEvent);

            EventMessenger.Instance.AddListener<OpenInventoryKeyPressedEvent>(OnOpenInventoryKeyPressedEvent);

            _itemSlotsGOList = new List<InventorySlotUIElement>();

            _windowOpened = false;
            _inventoryWindow.SetActive(false);
            _inventoryWindowContainer.SetActive(false);
        }

        /// <summary>
        /// Called when the inventory is loaded thru saved data
        /// </summary>
        /// <param name="eventData"></param>
        private void OnInventoryLoadedEvent(InventoryLoadedEvent eventData)
        {
            foreach (var item in eventData.ItemList)
            {
                _itemSlotsGOList[item.Id].Init(item);
            }
        }
        /// <summary>
        /// Open / Close the inventory Window
        /// </summary>
        /// <param name="eventData"></param>
        private void OnOpenInventoryKeyPressedEvent(OpenInventoryKeyPressedEvent eventData)
        {
            _windowOpened = !_windowOpened;

            _inventoryWindow.SetActive(_windowOpened);
            _inventoryWindowContainer.SetActive(_windowOpened);
        }

        /// <summary>
        /// Called when an item has ended being dragged in the inventory.
        /// Is placed in a new slot that is closest to the mouse position.
        /// </summary>
        /// <param name="eventData"></param>
        private void OnInventoryUIEndDragEvent(InventoryUIEndDragEvent eventData)
        {
            _isDragging = false;

            if (_draggingEventData == null)
            {
                return;
            }
            _mouseDragIcon.gameObject.SetActive(false);

            var nearestSlot = FindClosestItemSlotToMousePos();

            EventMessenger.Instance.Raise(new ItemPlacedEvent()
            {
                SlotPosition = _draggingEventData.SlotData.Id,
                SecondSlotPosition = nearestSlot.Id
            });

            _draggingEventData = null;
        }
        /// <summary>
        /// Helper function that is used to find the closest tile slot to the mouse position
        /// </summary>
        /// <returns></returns>
        private InventorySlotUIElement FindClosestItemSlotToMousePos()
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            _mouseDragIcon.transform.position = mouseWorldPos;

            return _itemSlotsGOList.OrderBy(item => Vector3.Distance(mouseWorldPos, item.transform.position)).First();
        }

        /// <summary>
        /// Called when the player started dragging an item in the inventory
        /// Saves the item data so it can do an action after dragging stops.
        /// </summary>
        /// <param name="eventData"></param>
        private void OnInventoryUIStartedDragging(InventoryUIStartedDraggingEvent eventData)
        {
            if (eventData.SlotData.InventoryItem == null)
            {
                return;
            }

            _mouseDragIcon.sprite = eventData.SlotData.InventoryItem.ItemImage;
            _mouseDragIcon.gameObject.SetActive(true);

            _draggingEventData = eventData;

            _isDragging = true;
        }
        /// <summary>
        /// Called when an inventory slot is updated, so that the UI can update accordingly.
        /// </summary>
        /// <param name="eventData"></param>
        private void OnInventoryUpdated(InventoryUpdatedEvent eventData)
        {
            _itemSlotsGOList[eventData.SlotPosition].Init(eventData.SlotData);
        }

        /// <summary>
        /// Called on Start when the inventory slots are generated
        /// </summary>
        /// <param name="eventData"></param>
        private void OnInventoryGenerated(InventoryGeneratedEvent eventData)
        {
            for (int i = 0; i < eventData.ItemList.Length; i++)
            {
                Transform parent = _inventoryWindow.transform;
                if (i < 10)
                {
                    parent = _equipBar.transform;
                }

                InventoryItemSlot item = eventData.ItemList[i];
                var go = Instantiate(_itemSlotUIPrefab, parent);
                var slotUIElement = go.GetComponent<InventorySlotUIElement>();
                slotUIElement.Init(item);
                _itemSlotsGOList.Add(slotUIElement);
            }
        }

        /// <summary>
        /// Manages the dragging utility for the items in the inventory
        /// </summary>
        private void Update()
        {
            if (_isDragging)
            {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f; // zero z
                _mouseDragIcon.transform.position = mouseWorldPos;
            }
        }
    }
}