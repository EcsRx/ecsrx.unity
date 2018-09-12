using System;
using LazyData.Mappings.Types.Primitives.Checkers;
using UnityEngine;

namespace EcsRx.Persistence.Primitives.Checkers
{
    public class UnityPrimitiveChecker : IPrimitiveChecker
    {
        public bool IsPrimitive(Type type)
        {
            return type == typeof(Vector2) ||
                   type == typeof(Vector3) ||
                   type == typeof(Vector4) ||
                   type == typeof(Quaternion);
        }
    }
}