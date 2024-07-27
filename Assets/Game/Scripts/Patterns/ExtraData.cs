using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueGravity.Interview.Patterns
{
    /// <summary>
    /// Basic class to serve as a data holder for extra parameters (resolved by cast)
    /// </summary>
    public class ExtraData 
    {
        public T GetData<T>() where T : ExtraData
        {
            return (T) this;
        }  
    }
}

