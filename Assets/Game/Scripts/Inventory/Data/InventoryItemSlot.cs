using BlueGravity.Interview.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Inventory
{
    /// <summary>
    /// It holds data for a specific slot in the inventory array.
    /// </summary>
    public class InventoryItemSlot
    {
        public InventoryItemSO InventoryItem { get; set; }

        public int Count { get; set; }
        public int Id { get; internal set; }
    }
}