using BlueGravity.Interview.Inventory;
using BlueGravity.Interview.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    enum MoveDirection
    {
        Idle,
        Up,
        Down, 
        Left, 
        Right
    }

    [SerializeField]
    private GameObject _playerGO;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    [SerializeField]
    private float _moveSpeed;

    private MoveDirection moveDirection;

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
        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody2D.velocity = new Vector2(_moveSpeed,0);

            if(moveDirection != MoveDirection.Right)
                _animator.SetTrigger("right");

            _animator.SetBool("idle", false);
            moveDirection = MoveDirection.Right;

        }
        else
        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody2D.velocity = new Vector2(-_moveSpeed, 0);

            if(moveDirection != MoveDirection.Left)
                _animator.SetTrigger("left");

            _animator.SetBool("idle", false);
            moveDirection = MoveDirection.Left;

        }
        else
        if (Input.GetKey(KeyCode.W))
        {
            _rigidbody2D.velocity = new Vector2(0, _moveSpeed);

            if(moveDirection != MoveDirection.Up)
            _animator.SetTrigger("up");

            _animator.SetBool("idle", false);
            moveDirection = MoveDirection.Up;

        }
        else
        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody2D.velocity = new Vector2(0, -_moveSpeed);

            if(moveDirection != MoveDirection.Down)
            _animator.SetTrigger("down");

            _animator.SetBool("idle", false);
            moveDirection = MoveDirection.Down;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool("idle", true);
            moveDirection = MoveDirection.Idle;
        }

        _playerGO.transform.position = new Vector3(_playerGO.transform.position.x, _playerGO.transform.position.y, _playerGO.transform.position.y);
    }
}
