using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerGO;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    [SerializeField]
    private float _moveSpeed;

    private void Awake()
    {
        EventMessenger.Instance.AddListener<ActionBarKeyPressedEvent>(OnActionBarKeyPressed);
    }

    private void OnActionBarKeyPressed(ActionBarKeyPressedEvent eventData)
    {
        EventMessenger.Instance.Raise(new InventoryUIItemClickedEvent() { SlotId = (eventData.Num-1) });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _rigidbody2D.velocity = new Vector2(_moveSpeed,0);
            _animator.SetTrigger("right");
            _animator.SetBool("idle", false);

        }
        else
        if (Input.GetKeyDown(KeyCode.A))
        {
            _rigidbody2D.velocity = new Vector2(-_moveSpeed, 0);
            _animator.SetTrigger("left");
            _animator.SetBool("idle", false);

        }
        else
        if (Input.GetKeyDown(KeyCode.W))
        {
            _rigidbody2D.velocity = new Vector2(0, _moveSpeed);
            _animator.SetTrigger("up");
            _animator.SetBool("idle", false);

        }
        else
        if (Input.GetKeyDown(KeyCode.S))
        {
            _rigidbody2D.velocity = new Vector2(0, -_moveSpeed);
            _animator.SetTrigger("down");
            _animator.SetBool("idle", false);


        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool("idle", true);
        }

    }
}
