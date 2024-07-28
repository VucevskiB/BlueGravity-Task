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
    private GameObject _equipBar;

    [SerializeField]
    private GameObject _inventoryWindow;

    [SerializeField] 
    private GameObject _inventoryWindowContainer;

    [SerializeField]
    private List<InventorySlotUIElement> _itemSlotsGOList;

    [SerializeField]
    private Image _mouseDragIcon;

    private bool _isDragging;
    private InventoryUIStartedDraggingEvent _draggingEventData;

    private bool _windowOpened;
    private void Awake()
    {
        EventMessenger.Instance.AddListener<InventoryGeneratedEvent>(OnInventoryGenerated);
        EventMessenger.Instance.AddListener<InventoryLoadedEvent>(OnInventoryLoadedEvent);
        EventMessenger.Instance.AddListener<InventoryUpdatedEvent>(OnInventoryUpdated);

        EventMessenger.Instance.AddListener<InventoryUIStartedDraggingEvent>(OnInventoryUIStartedDragging);
        EventMessenger.Instance.AddListener<InventoryUIEndDragEvent>(OnInventoryUIEndDragEvent);

        EventMessenger.Instance.AddListener<OpenInventoryKeyPressedEvent>(OnOpenInventoryKeyPressedEvent);

        _itemSlotsGOList = new List<InventorySlotUIElement>();

        _windowOpened = false;
        _inventoryWindow.SetActive(false);
        _inventoryWindowContainer.SetActive(false);
    }

    private void OnInventoryLoadedEvent(InventoryLoadedEvent eventData)
    {
        foreach (var item in eventData.ItemList)
        {
            _itemSlotsGOList[item.Id].Init(item);
        }
    }

    private void OnOpenInventoryKeyPressedEvent(OpenInventoryKeyPressedEvent eventData)
    {
        _windowOpened = !_windowOpened;

        _inventoryWindow.SetActive(_windowOpened);
        _inventoryWindowContainer.SetActive(_windowOpened);
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
            SlotPosition = _draggingEventData.SlotData.Id,
            SecondSlotPosition = nearestSlot.Id
        }) ;

        _draggingEventData = null;
    }

    private InventorySlotUIElement FindClosestItemSlotToMousePos()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        _mouseDragIcon.transform.position = mouseWorldPos;

        return _itemSlotsGOList.OrderBy(item => Vector3.Distance(mouseWorldPos, item.transform.position)).First();
    }

    private void OnInventoryUIStartedDragging(InventoryUIStartedDraggingEvent eventData)
    {
        if(eventData.SlotData.InventoryItem == null)
        {
            return;
        }

        _mouseDragIcon.sprite = eventData.SlotData.InventoryItem.ItemImage;
        _mouseDragIcon.gameObject.SetActive(true);

        _draggingEventData = eventData;

        _isDragging = true;
    }

    private void OnInventoryUpdated(InventoryUpdatedEvent eventData)
    {
        _itemSlotsGOList[eventData.SlotPosition].Init(eventData.SlotData);
    }

    private void OnInventoryGenerated(InventoryGeneratedEvent eventData)
    {
        for (int i = 0; i < eventData.ItemList.Length; i++)
        {
            Transform parent = _inventoryWindow.transform;
            if(i < 10)
            {
                parent = _equipBar.transform;
            }
            
            InventoryItemSlot item = eventData.ItemList[i];
            var go = Instantiate(_itemSlotUIPrefab, parent);
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
