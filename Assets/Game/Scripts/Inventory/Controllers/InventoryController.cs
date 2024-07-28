using BlueGravity.Interview.Controls;
using BlueGravity.Interview.Patterns;
using BlueGravity.Interview.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BlueGravity.Interview.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private const int INVENTORY_SPACE = 34;
        [SerializeField]
        private ItemDatabaseSO _itemDatabaseSO;

        private InventoryItemSlot[] _inventorySlots;
        // Start is called before the first frame update
        void Awake()
        {
            EventMessenger.Instance.AddListener<ItemPlacedEvent>(OnItemPlacedEvent);
            EventMessenger.Instance.AddListener<InventoryUIItemClickedEvent>(OnItemConsumed);

            EventMessenger.Instance.AddListener<AddItemToInventoryEvent>(OnAddItemToInventoryEvent);
            EventMessenger.Instance.AddListener<CloseGameKeyPressed>(OnCloseGame);
            GenerateInventory();

        }
        private void Start()
        {
            EventMessenger.Instance.Raise(new InventoryGeneratedEvent() { ItemList = _inventorySlots });

            LoadInventoryFromSave();
        }

        /// <summary>
        /// Generates the inventory slots
        /// </summary>
        private void GenerateInventory()
        {
            _inventorySlots = new InventoryItemSlot[INVENTORY_SPACE];

            for (int i = 0; i < INVENTORY_SPACE; i++)
            {
                _inventorySlots[i] = new InventoryItemSlot() { Id = i };
            }
        }

        /// <summary>
        /// Called when the game is closed
        /// </summary>
        /// <param name="eventData"></param>
        private void OnCloseGame(CloseGameKeyPressed eventData)
        {
            SaveInventory();
        }

        /// <summary>
        /// Called when a new items is trying to be added to the inventory
        /// </summary>
        /// <param name="eventData"></param>
        private void OnAddItemToInventoryEvent(AddItemToInventoryEvent eventData)
        {
            AddItem(eventData.Item,1);
        }

        /// <summary>
        /// Called when an item from the inventory is used/consumed
        /// </summary>
        /// <param name="eventData"></param>
        private void OnItemConsumed(InventoryUIItemClickedEvent eventData)
        {
            if (_inventorySlots[eventData.SlotId].InventoryItem is null)
                return;

            Debug.Log($"<color=#00FF00> {_inventorySlots[eventData.SlotId].InventoryItem.name} consumed...</color>");

            RemoveItemFromInventory(eventData.SlotId, 1);
        }

        /// <summary>
        /// Saves the current state of the inventory in PlayerPrefs
        /// </summary>
        public void SaveInventory()
        {
            PlayerPrefsManager.SaveInventory(_inventorySlots);
        }

        /// <summary>
        /// Loads last saved inventory state from PlayerPrefs
        /// </summary>
        public void LoadInventoryFromSave()
        {
            var savedInv = PlayerPrefsManager.LoadInventory();
            if(savedInv is not null && savedInv.Count() > 0)
            {
                _inventorySlots = savedInv;
            }

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                _inventorySlots[i].InventoryItem = _itemDatabaseSO.Items.GetValueOrDefault(_inventorySlots[i].ItemId,null);
            }

            EventMessenger.Instance.Raise(new InventoryLoadedEvent() { ItemList = _inventorySlots });
        }

        /// <summary>
        /// Adds a new item in the inventory in a specific slot,
        /// or if a slot position isn't determined it finds the first
        /// empty slot position.
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="count"></param>
        /// <param name="slotPosition"></param>
        /// <exception cref="InvetoryFullException"></exception>
        public void AddItem(InventoryItemSO itemData, int count, int? slotPosition = null)
        {

            // if no specified slot position and item is stackable then find if item already exists in inventory to stack it
            if(slotPosition == null && itemData.IsStackable)
            {
                InventoryItemSlot slot = TryFindItemInInventory(itemData);
                if(slot != null)
                {
                    slot.Count += count;

                    EventMessenger.Instance.Raise(new InventoryUpdatedEvent() { SlotPosition = Array.IndexOf(_inventorySlots,slot), SlotData = slot });


                    return;
                }
            }

            // if no specified slot position then find the first empty slot position
            if(slotPosition == null)
            {
                slotPosition = GetFirstEmptySlotPosition();
            }

            if(slotPosition == -1)
            {
                throw new InvetoryFullException("There isn't an empty slot in the inventory");
            }
            if(slotPosition >= _inventorySlots.Length)
            {
                throw new InvetoryFullException("Slot number exceeds the inventory size");
            }
            if (_inventorySlots[slotPosition.Value].InventoryItem is not null && (_inventorySlots[slotPosition.Value].InventoryItem != itemData || _inventorySlots[slotPosition.Value].InventoryItem.IsStackable == false))
            {
                throw new InventorySlotPositionNotEmptyException();
            }

            _inventorySlots[slotPosition.Value] = new InventoryItemSlot() { Count = count, InventoryItem = itemData, Id = slotPosition.Value };

            EventMessenger.Instance.Raise(new InventoryUpdatedEvent() { SlotPosition = slotPosition.Value, SlotData = _inventorySlots[slotPosition.Value] });
        }

        /// <summary>
        /// Moves an existing item in the inventory to a new
        /// position in the inventory.
        /// If needed it also makes a position swap if an item already
        /// exists in the new inventory position.
        /// </summary>
        /// <param name="beforeSlotNumber"></param>
        /// <param name="newSlotNumber"></param>
        public void MoveItem(int beforeSlotNumber, int newSlotNumber)
        {
            InventoryItemSlot beforeItemSlot = new InventoryItemSlot { 
                Id = _inventorySlots[beforeSlotNumber].Id,
                Count = _inventorySlots[beforeSlotNumber].Count,
                InventoryItem = _inventorySlots[beforeSlotNumber].InventoryItem
            };

            _inventorySlots[beforeSlotNumber] = _inventorySlots[newSlotNumber];
            _inventorySlots[newSlotNumber] = beforeItemSlot;

            _inventorySlots[newSlotNumber].Id = newSlotNumber;
            _inventorySlots[beforeSlotNumber].Id = beforeSlotNumber;

            EventMessenger.Instance.Raise(new InventoryUpdatedEvent() { SlotPosition = beforeSlotNumber, SlotData = _inventorySlots[beforeSlotNumber] });
            EventMessenger.Instance.Raise(new InventoryUpdatedEvent() { SlotPosition = newSlotNumber, SlotData = _inventorySlots[newSlotNumber] });
        }

        private void OnItemPlacedEvent(ItemPlacedEvent eventData)
        {
            MoveItem(eventData.SlotPosition, eventData.SecondSlotPosition);
        }


        /// <summary>
        /// Tries to find a slot that contains the same itemData and removes a specific amount.
        /// if the count goes to 0 then the item is removed from the slot. 
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="amount"></param>
        public void RemoveItemFromInventory(InventoryItemSO itemData, int amount) {

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventoryItemSlot item = _inventorySlots[i];
                if (item is not null && item.InventoryItem == itemData)
                {
                    item.Count -= amount;
                    if(item.Count <= 0)
                    {
                        _inventorySlots[i].InventoryItem = null;
                    }

                    EventMessenger.Instance.Raise(new InventoryUpdatedEvent() { SlotPosition = i, SlotData = _inventorySlots[i] });

                    return;
                }
            }
        }
        /// <summary>
        /// Removes amount from a specific slot in the inventory.
        /// If count goes to 0 then the item is removed from the inventory
        /// </summary>
        /// <param name="slotNumber"></param>
        /// <param name="amount"></param>
        public void RemoveItemFromInventory(int slotNumber, int amount) {

            if (_inventorySlots[slotNumber] is not null)
            {
                _inventorySlots[slotNumber].Count -= amount;

                if (_inventorySlots[slotNumber].Count <= 0)
                {
                    _inventorySlots[slotNumber].InventoryItem = null;
                }

                EventMessenger.Instance.Raise(new InventoryUpdatedEvent() { SlotPosition = slotNumber, SlotData = _inventorySlots[slotNumber] });
            }
        
        }

        /// <summary>
        /// Used for testing if inventory works as intended.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public InventoryItemSlot[] GetItems()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventoryItemSlot item = _inventorySlots[i];
                string name = item.InventoryItem == null ? "null" : item.InventoryItem.ItemName;
                sb.Append(i + " - " + name + ": " + item?.Count);
                sb.Append("\n");
            }
            Debug.Log(sb.ToString());
            return _inventorySlots;

        }


        private InventoryItemSlot TryFindItemInInventory(InventoryItemSO itemData)
        {
            foreach (var item in _inventorySlots)
            {
                if(item is not null && item.InventoryItem == itemData)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Helper function that finds the first empty slot
        /// in the inventory list
        /// </summary>
        /// <returns></returns>
        private int GetFirstEmptySlotPosition()
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                if (_inventorySlots[i].InventoryItem == null)
                    return i;
            }
            return -1;
        }
    }
}