using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Inventory
{
    /// <summary>
    /// Data Object that contains information about a specific item that can be stored
    /// in the players inventory.
    /// </summary>
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/Inventory/InventoryItem")]
    public class InventoryItemSO : ScriptableObject
    {
        [SerializeField]
        private string _itemName;

        [SerializeField]
        private string _description;

        [SerializeField]
        private bool _isStackable;

        [SerializeField]
        private Sprite _itemImage;

        public string ItemName { get => _itemName;  }
        public string Description { get => _description;  }
        public bool IsStackable { get => _isStackable;  }
        public Sprite ItemImage { get => _itemImage; }
    }
}