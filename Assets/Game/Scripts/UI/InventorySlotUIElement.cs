using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Component placed on the InventorySlotUIElement prefab
/// that helps manage all input controls of the Inventory Item Slot element
/// </summary>
public class InventorySlotUIElement : MonoBehaviour
{
    private InventoryItemSlot _data;

    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemCount;

    [SerializeField]
    private int _customId = -1;

    [SerializeField]
    private Vector2 _toolTipOffset;

    public int Id { get => _data.Id; }


    /// <summary>
    /// Init slot item from the inventory.
    /// If slot does not have a item in it, then hide visual components 
    /// </summary>
    /// <param name="item"></param>
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

    /// <summary>
    /// Called from the EventTrigger Component BeginDrag
    /// </summary>
    public void OnBeginDrag()
    {
        Debug.Log("BeginDrag");
        EventMessenger.Instance.Raise(new InventoryUIStartedDraggingEvent() { 
            SlotData = _data, 
            SlotPosition = Id 
        });

    }
    /// <summary>
    /// Called from the EventTrigger Component PointerUp
    /// </summary>
    public void OnPointerUp()
    {
        Debug.Log("End Drag");
        EventMessenger.Instance.Raise(new InventoryUIEndDragEvent());
    }
    /// <summary>
    /// Called from the EventTrigger Component PointerClick
    /// </summary>
    public void OnPointerClick()
    {
        if (_data.InventoryItem == null)
            return;

        EventMessenger.Instance.Raise(new InventoryUIItemClickedEvent() {
            SlotId = _data.Id
        });

        Debug.Log("Click!");
    }

    /// <summary>
    /// Called from the EventTrigger Component PointerEnter
    /// </summary>
    public void OnMouseHover()
    {
        if (_data.InventoryItem == null)
            return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector2 screenPos2D = new Vector2(screenPos.x, screenPos.y);

        ToolTipController.Instance.Open(_data.InventoryItem.Description, screenPos2D, _toolTipOffset, 0);
    }

    /// <summary>
    /// Called from the EventTrigger Component PointerExit
    /// </summary>
    public void OnPointerExit()
    {
        ToolTipController.Instance.Close();
    }
}
