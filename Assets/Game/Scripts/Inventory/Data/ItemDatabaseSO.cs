using BlueGravity.Interview.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Inventory/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField]
    private InventoryItemSO[] list;

    Dictionary<string, InventoryItemSO> _items;

    public Dictionary<string,  InventoryItemSO> Items { get
        {
            if(_items is  null || _items.Count == 0)
            {
                GenerateDictionary();
            }

            return _items;
        } 
    }

    private void GenerateDictionary()
    {
        _items = new Dictionary<string, InventoryItemSO>();
        foreach (var item in list)
        {
            _items.Add(item.ItemName, item);
        }
    }
}
