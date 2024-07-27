
namespace BlueGravity.Interview.Patterns
{
    /// <summary>
    /// Basic class to serve as a GameEvent for the <see cref="EventMessenger"/> implementation.
    /// </summary>
    public class GameEvent {
        public bool ShowDebug = true;

        public virtual string GetDebugText() 
        {
            return GetType().Name;
        }
    }
}
