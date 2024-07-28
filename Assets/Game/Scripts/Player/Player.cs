using BlueGravity.Interview.Collectables;
using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private CollectablesManager _collectablesManager;

        /// <summary>
        /// If the player game object collides with a collectable item
        /// then add that item to their inventory
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Item")
                return;

            var item = collision.GetComponent<CollectableItem>().ItemData;

            EventMessenger.Instance.Raise(new AddItemToInventoryEvent() { Item = item });

            _collectablesManager.ItemCollected(collision.gameObject);
        }
    }
}