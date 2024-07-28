using BlueGravity.Interview.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Inventory
{
    public class InventoryGeneratedEvent : GameEvent
    {
        public InventoryItemSlot[] ItemList;
    }
    public class InventoryLoadedEvent : GameEvent {
        public InventoryItemSlot[] ItemList;
    }

    public class InventoryUpdatedEvent : GameEvent 
    {
        public int SlotPosition;
        public InventoryItemSlot SlotData;
    }

    public class InventoryUIStartedDraggingEvent : GameEvent {

        public int SlotPosition;
        public InventoryItemSlot SlotData;

    }

    public class AddItemToInventoryEvent : GameEvent
    {
        public InventoryItemSO Item;
    }

    public class InventoryUIEndDragEvent : GameEvent
    {
        
    }

    public class InventoryUIItemClickedEvent : GameEvent
    {
        public int SlotId;
    }

    public class ItemPlacedEvent : GameEvent
    {
        public int SlotPosition;
        public int SecondSlotPosition;
    }
}