using System;
using System.Runtime.Serialization;

namespace BlueGravity.Interview.Inventory
{
    [Serializable]
    internal class InventorySlotPositionNotEmptyException : Exception
    {
        public InventorySlotPositionNotEmptyException()
        {
        }

        public InventorySlotPositionNotEmptyException(string message) : base(message)
        {
        }

        public InventorySlotPositionNotEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InventorySlotPositionNotEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}