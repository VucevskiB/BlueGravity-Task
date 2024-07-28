using BlueGravity.Interview.Patterns;

/// <summary>
/// Events dedicated for Keyboard Input by the player
/// </summary>

namespace BlueGravity.Interview.Controls
{
    public class OpenInventoryKeyPressedEvent : GameEvent
    {

    }
    public class ActionBarKeyPressedEvent : GameEvent
    {
        public int Num;
    }

    public class PlayerMoveKeyPressedEvent : GameEvent
    {

    }
    public class CloseGameKeyPressed : GameEvent
    {


    }
}