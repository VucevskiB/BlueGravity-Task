using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CollectablesManager _collectablesManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Item")
            return;

        var item = collision.GetComponent<CollectableItem>().ItemData;

        EventMessenger.Instance.Raise(new AddItemToInventoryEvent() { Item = item });

        _collectablesManager.ItemCollected(collision.gameObject);
    }
}
