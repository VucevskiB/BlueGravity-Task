using BlueGravity.Interview.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControls : MonoBehaviour
{
    [SerializeField]
    private KeyCode _openInventoryKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(_openInventoryKey))
        {
            EventMessenger.Instance.Raise(new OpenInventoryKeyPressedEvent());
        }
    }
}
