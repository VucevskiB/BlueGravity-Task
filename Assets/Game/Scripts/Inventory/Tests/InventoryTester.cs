using BlueGravity.Interview.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public InventoryItemSO[] items;

    public InventoryController inventoryController;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        inventoryController.AddItem(items[0], 1);
        inventoryController.AddItem(items[0], 10);

        inventoryController.AddItem(items[1], 1,10);

        inventoryController.AddItem(items[1], 1);

        var inv = inventoryController.GetItems();

        inventoryController.MoveItem(0, 1);

        inventoryController.GetItems();

        inventoryController.RemoveItemFromInventory(items[1],1);
        inventoryController.RemoveItemFromInventory(1,5);

        inventoryController.GetItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
