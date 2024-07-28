using BlueGravity.Interview.Inventory;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Utility
{
    /// <summary>
    /// PlayerPrefs wrapper class that manages calls to the PlayerPrefs class from the Unity Engine.
    /// </summary>
    public static class PlayerPrefsManager
    {
        private const string INVENTORY_KEY = "inventory";

        /// <summary>
        /// Convert the inventory data to JSON to save as a text file
        /// </summary>
        /// <param name="data"></param>
        public static void SaveInventory(InventoryItemSlot[] data)
        {
            var json = JsonConvert.SerializeObject(data);

            PlayerPrefs.SetString(INVENTORY_KEY, json);
        }

        /// <summary>
        /// Loads the inventory data from a text file from PlayerPrefs.
        /// </summary>
        /// <returns></returns>
        public static InventoryItemSlot[] LoadInventory()
        {
            var json = PlayerPrefs.GetString(INVENTORY_KEY);

            InventoryItemSlot[] savedInventory = JsonConvert.DeserializeObject<InventoryItemSlot[]>(json);

            return savedInventory;
        }
    }
}