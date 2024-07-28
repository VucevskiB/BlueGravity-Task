using BlueGravity.Interview.Patterns;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace BlueGravity.Interview.Controls
{
    /// <summary>
    /// Manages Input keys from the player from the keyboard
    /// </summary>
    public class UserControls : MonoBehaviour
    {
        [SerializeField]
        private KeyCode _openInventoryKey;

        [SerializeField]
        private KeyCode _actionBar1;
        [SerializeField]
        private KeyCode _actionBar2;
        [SerializeField]
        private KeyCode _actionBar3;
        [SerializeField]
        private KeyCode _actionBar4;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(_openInventoryKey))
            {
                EventMessenger.Instance.Raise(new OpenInventoryKeyPressedEvent());
            }

            if (Input.GetKeyDown(_actionBar1))
                EventMessenger.Instance.Raise(new ActionBarKeyPressedEvent() { Num = 1 });

            if (Input.GetKeyDown(_actionBar2))
                EventMessenger.Instance.Raise(new ActionBarKeyPressedEvent() { Num = 2 });

            if (Input.GetKeyDown(_actionBar3))
                EventMessenger.Instance.Raise(new ActionBarKeyPressedEvent() { Num = 3 });

            if (Input.GetKeyDown(_actionBar4))
                EventMessenger.Instance.Raise(new ActionBarKeyPressedEvent() { Num = 4 });

            if (Input.GetKeyDown(KeyCode.X))
            {
                EventMessenger.Instance.Raise(new CloseGameKeyPressed());
                Application.Quit();
            }
        }

        private void OnApplicationQuit()
        {
            EventMessenger.Instance.Raise(new CloseGameKeyPressed());
        }
    }
}