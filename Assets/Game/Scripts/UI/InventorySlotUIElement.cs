using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUIElement : MonoBehaviour
{
    private InventoryItemSlot _data;

    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemCount;

    [SerializeField]
    private int _customId = -1;

    public int Id { get => _data.Id; }

    internal void Init(InventoryItemSlot item)
    {
        _data = item;

        if (item.InventoryItem == null)
        {
            _itemImage.gameObject.SetActive(false);
            _itemCount.gameObject.SetActive(false);
            return;
        }

        _itemImage.sprite = item.InventoryItem.ItemImage;
        _itemCount.text = item.Count.ToString();

        _itemImage.gameObject.SetActive(true);
        _itemCount.gameObject.SetActive(true);
    }

    public void OnBeginDrag()
    {
        Debug.Log("BeginDrag");
        EventMessenger.Instance.Raise(new InventoryUIStartedDraggingEvent() { 
            SlotData = _data, 
            SlotPosition = Id });

    }

    public void OnPointerUp()
    {
        Debug.Log("End Drag");
        EventMessenger.Instance.Raise(new InventoryUIEndDragEvent());
    }
}
