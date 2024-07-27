using BlueGravity.Interview.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BlueGravity.Interview.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private InventoryItemSlot[] _inventorySlots;
        // Start is called before the first frame update
        void Awake()
        {
            EventMessenger.Instance.AddListener<ItemPlacedEvent>(OnItemPlacedEvent);

            _inventorySlots = new InventoryItemSlot[16];

            //TO DO: add loading inventory from playerprefs
        }

        private void Start()
        {
            EventMessenger.Instance.Raise(new InventoryGeneratedEvent() { ItemList = _inventorySlots });
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

                    EventMessenger.Instance.Raise(new InventoryUpdateEvent() { SlotPosition = Array.IndexOf(_inventorySlots,slot), SlotData = slot });


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
            if (_inventorySlots[slotPosition.Value] is not null && (_inventorySlots[slotPosition.Value].InventoryItem != itemData || _inventorySlots[slotPosition.Value].InventoryItem.IsStackable == false))
            {
                throw new InventorySlotPositionNotEmptyException();
            }

            _inventorySlots[slotPosition.Value] = new InventoryItemSlot() { Count = count, InventoryItem = itemData };

            EventMessenger.Instance.Raise(new InventoryUpdateEvent() { SlotPosition = slotPosition.Value, SlotData = _inventorySlots[slotPosition.Value] });
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
            InventoryItemSlot beforeItemSlot = _inventorySlots[beforeSlotNumber];
            _inventorySlots[beforeSlotNumber] = _inventorySlots[newSlotNumber];
            _inventorySlots[newSlotNumber] = beforeItemSlot;

            EventMessenger.Instance.Raise(new InventoryUpdateEvent() { SlotPosition = beforeSlotNumber, SlotData = _inventorySlots[beforeSlotNumber] });
            EventMessenger.Instance.Raise(new InventoryUpdateEvent() { SlotPosition = newSlotNumber, SlotData = _inventorySlots[newSlotNumber] });
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
                        _inventorySlots[i] = null;
                    }

                    EventMessenger.Instance.Raise(new InventoryUpdateEvent() { SlotPosition = i, SlotData = _inventorySlots[i] });

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
                    _inventorySlots[slotNumber] = null;
                }

                EventMessenger.Instance.Raise(new InventoryUpdateEvent() { SlotPosition = slotNumber, SlotData = _inventorySlots[slotNumber] });
            }
        
        }
        public InventoryItemSlot[] GetItems()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventoryItemSlot item = _inventorySlots[i];
                string name = item == null ? "null" : item.InventoryItem.ItemName;
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
                if (_inventorySlots[i] == null)
                    return i;
            }
            return -1;
        }
    }
}