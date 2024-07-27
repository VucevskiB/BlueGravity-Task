using System;
using System.Runtime.Serialization;

namespace BlueGravity.Interview.Inventory
{
    [Serializable]
    internal class InvetoryFullException : Exception
    {
        public InvetoryFullException()
        {
        }

        public InvetoryFullException(string message) : base(message)
        {
        }

        public InvetoryFullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvetoryFullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}