using BlueGravity.Interview.Inventory;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Inventory
{
    /// <summary>
    /// It holds data for a specific slot in the inventory array.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class InventoryItemSlot
    {
        private InventoryItemSO _inventoryItem;
        private int _count;
        private int _id;
        private string _itemId;

        public InventoryItemSO InventoryItem { get => _inventoryItem; set => _inventoryItem = value; }

        [JsonProperty]
        public int Count { get => _count; set => _count = value; }
        [JsonProperty]
        public int Id { get => _id; internal set => _id = value; }

        [JsonProperty]
        public string ItemId { get
            {
                if (string.IsNullOrEmpty(_itemId))
                {
                    _itemId = _inventoryItem != null ? _inventoryItem.ItemName : "";
                    return _itemId;
                }
                return _itemId;
            }
            set => _itemId = value; }
    }
}