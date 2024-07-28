using BlueGravity.Interview.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace BlueGravity.Interview.Collectables
{
    public class CollectablesManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _collectablePrefab;

        [SerializeField]
        private ItemDatabaseSO _itemDatabaseSO;
        private List<InventoryItemSO> _itemList;

        [SerializeField]
        private Transform[] _itemPositionList;

        private void Start()
        {
            _itemList = _itemDatabaseSO.Items.Values.ToList();

            foreach (var item in _itemPositionList)
            {
                var go = Instantiate(_collectablePrefab, item.position, Quaternion.identity);

                var r = UnityEngine.Random.Range(0, _itemDatabaseSO.Items.Count);
                go.GetComponent<CollectableItem>().Init(_itemList[r]);
            }
        }


        internal void ItemCollected(GameObject item)
        {
            Vector3 position = item.transform.position;

            Destroy(item);

            StartCoroutine(SpawnNewItem(position));
        }

        private IEnumerator SpawnNewItem(Vector3 pos)
        {
            yield return new WaitForSeconds(2f);

            var go = Instantiate(_collectablePrefab, pos, Quaternion.identity);

            var r = UnityEngine.Random.Range(0, _itemDatabaseSO.Items.Count);
            go.GetComponent<CollectableItem>().Init(_itemList[r]);

        }
    }
}