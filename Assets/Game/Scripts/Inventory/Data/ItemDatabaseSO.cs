using BlueGravity.Interview.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Inventory/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField]
    private InventoryItemSO[] list;

    public InventoryItemSO[] List { get { return list; } }
}
