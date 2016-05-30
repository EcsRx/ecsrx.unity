using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using Zenject.Internal;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class ValidationMarker
    {
        public ValidationMarker(Type markedType)
        {
            MarkedType = markedType;
        }

        public Type MarkedType
        {
            get;
            private set;
        }
    }
}

