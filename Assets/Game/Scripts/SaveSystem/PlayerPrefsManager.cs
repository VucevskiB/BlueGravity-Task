using BlueGravity.Interview.Inventory;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager 
{
    private const string INVENTORY_KEY = "inventory";

    public static void SaveInventory(InventoryItemSlot[] data)
    {
        var json = JsonConvert.SerializeObject(data);
        
        PlayerPrefs.SetString(INVENTORY_KEY,json);
    }

    public static InventoryItemSlot[] LoadInventory() {
        var json = PlayerPrefs.GetString(INVENTORY_KEY);

        InventoryItemSlot[] savedInventory = JsonConvert.DeserializeObject <InventoryItemSlot[]>(json);

        return savedInventory;
    }
}
