using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemSlotUIPrefab;

    [SerializeField]
    private List<InventorySlotUIElement> _itemSlotsGOList;

    [SerializeField]
    private Image _mouseDragIcon;

    private bool _isDragging;
    private InventoryUIStartedDraggingEvent _draggingEventData;
    private void Awake()
    {
        EventMessenger.Instance.AddListener<InventoryGeneratedEvent>(OnInventoryGenerated);
        EventMessenger.Instance.AddListener<InventoryUpdateEvent>(OnInventoryUpdated);

        EventMessenger.Instance.AddListener<InventoryUIStartedDraggingEvent>(OnInventoryUIStartedDragging);
        EventMessenger.Instance.AddListener<InventoryUIEndDragEvent>(OnInventoryUIEndDragEvent);

        _itemSlotsGOList = new List<InventorySlotUIElement>();
    }

    private void OnInventoryUIEndDragEvent(InventoryUIEndDragEvent eventData)
    {
        _isDragging = false;

        if (_draggingEventData == null)
        {
            return;
        }
        _mouseDragIcon.gameObject.SetActive(false);

        var nearestSlot = FindClosestItemSlotToMousePos();

        EventMessenger.Instance.Raise(new ItemPlacedEvent()
        {
            SlotPosition = _draggingEventData.SlotPosition,
            SecondSlotPosition = nearestSlot.transform.GetSiblingIndex()
        }) ;

        _draggingEventData = null;
    }

    private InventorySlotUIElement FindClosestItemSlotToMousePos()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z
        _mouseDragIcon.transform.position = mouseWorldPos;

        return _itemSlotsGOList.OrderBy(item => Vector3.Distance(mouseWorldPos, item.transform.position)).First();
    }

    private void OnInventoryUIStartedDragging(InventoryUIStartedDraggingEvent eventData)
    {
        if(eventData.SlotData == null)
        {
            return;
        }

        _mouseDragIcon.sprite = eventData.SlotData.InventoryItem.ItemImage;
        _mouseDragIcon.gameObject.SetActive(true);

        _draggingEventData = eventData;

        _isDragging = true;
    }

    private void OnInventoryUpdated(InventoryUpdateEvent eventData)
    {
        _itemSlotsGOList[eventData.SlotPosition].Init(eventData.SlotData);
    }

    private void OnInventoryGenerated(InventoryGeneratedEvent eventData)
    {
        foreach (var item in eventData.ItemList)
        {
            var go = Instantiate(_itemSlotUIPrefab, transform);
            var slotUIElement = go.GetComponent<InventorySlotUIElement>();
            slotUIElement.Init(item);
            _itemSlotsGOList.Add(slotUIElement);
        }
    }

    private void Update()
    {
        if (_isDragging)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f; // zero z
            _mouseDragIcon.transform.position = mouseWorldPos;
        }
    }
}
