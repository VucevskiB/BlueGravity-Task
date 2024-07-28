using BlueGravity.Interview.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Inventory
{
    public class CollectableItem : MonoBehaviour
    {
        private InventoryItemSO _itemData;

        public InventoryItemSO ItemData { get => _itemData; set => _itemData = value; }


        public void Init(InventoryItemSO data)
        {
            _itemData = data;
            GetComponentInChildren<SpriteRenderer>().sprite = _itemData.ItemImage;
        }
    }
}