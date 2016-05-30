using System;
using System.Collections.Generic;
using ModestTree;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class StandardSingletonDeclaration
    {
        public StandardSingletonDeclaration(
            SingletonId id, List<TypeValuePair> args, SingletonTypes type, object singletonSpecificId)
        {
            Id = id;
            Type = type;
            SpecificId = singletonSpecificId;
            Arguments = args;
        }

        public StandardSingletonDeclaration(
            Type concreteType, string concreteIdentifier, List<TypeValuePair> args,
            SingletonTypes type, object singletonSpecificId)
            : this(
                new SingletonId(concreteType, concreteIdentifier), args,
                type, singletonSpecificId)
        {
        }

        public List<TypeValuePair> Arguments
        {
            get;
            private set;
        }

        public SingletonId Id
        {
            get;
            private set;
        }

        public SingletonTypes Type
        {
            get;
            private set;
        }

        public object SpecificId
        {
            get;
            private set;
        }
    }
}

