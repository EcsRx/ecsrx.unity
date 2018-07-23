using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public abstract class StaticMemoryPoolBase<TValue> : IDespawnableMemoryPool<TValue>, IDisposable
        where TValue : class, new()
    {
        readonly Stack<TValue> _stack = new Stack<TValue>();

        Action<TValue> _onDespawnedMethod;
        int _activeCount;

        public StaticMemoryPoolBase(Action<TValue> onDespawnedMethod)
        {
            _onDespawnedMethod = onDespawnedMethod;
            StaticMemoryPoolRegistry.Add(this);
        }

        public Action<TValue> OnDespawnedMethod
        {
            set { _onDespawnedMethod = value; }
        }

        public int NumTotal
        {
            get { return NumInactive + NumActive; }
        }

        public int NumActive
        {
            get { return _activeCount; }
        }

        public int NumInactive
        {
            get { return _stack.Count; }
        }

        public Type ItemType
        {
            get { return typeof(TValue); }
        }

        public void Resize(int desiredPoolSize)
        {
            Assert.That(desiredPoolSize >= 0, "Attempted to resize the pool to a negative amount");

            while (_stack.Count > desiredPoolSize)
            {
                _stack.Pop();
            }

            while (desiredPoolSize > _stack.Count)
            {
                _stack.Push(Alloc());
            }

            Assert.IsEqual(_stack.Count, desiredPoolSize);
        }

        public void Dispose()
        {
            StaticMemoryPoolRegistry.Remove(this);
        }

        public void Clear()
        {
            Resize(0);
        }

        public void ShrinkBy(int numToRemove)
        {
            Resize(_stack.Count - numToRemove);
        }

        public void ExpandBy(int numToAdd)
        {
            Resize(_stack.Count + numToAdd);
        }

        TValue Alloc()
        {
            return new TValue();
        }

        protected TValue SpawnInternal()
        {
            TValue element;

            if (_stack.Count == 0)
            {
                element = Alloc();
            }
            else
            {
                element = _stack.Pop();
            }

            _activeCount++;
            return element;
        }

        void IMemoryPool.Despawn(object item)
        {
            Despawn((TValue)item);
        }

        public void Despawn(TValue element)
        {
            if (_onDespawnedMethod != null)
            {
                _onDespawnedMethod(element);
            }

            if (_stack.Count > 0 && ReferenceEquals(_stack.Peek(), element))
            {
                ModestTree.Log.Error("Despawn error. Trying to destroy object that is already released to pool.");
            }

            Assert.That(!_stack.Contains(element), "Attempted to despawn element twice!");

            _activeCount--;
            _stack.Push(element);
        }
    }

    // Zero parameters

    public class StaticMemoryPool<TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TValue>
        where TValue : class, new()
    {
        Action<TValue> _onSpawnMethod;

        public StaticMemoryPool(
            Action<TValue> onSpawnMethod = null, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn()
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(item);
            }

            return item;
        }
    }

    // One parameter

    public class StaticMemoryPool<TParam1, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TValue>
        where TValue : class, new()
    {
        Action<TParam1, TValue> _onSpawnMethod;

        public StaticMemoryPool(
            Action<TParam1, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn(TParam1 param)
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(param, item);
            }

            return item;
        }
    }

    // Two parameter

    public class StaticMemoryPool<TParam1, TParam2, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TValue>
        where TValue : class, new()
    {
        Action<TParam1, TParam2, TValue> _onSpawnMethod;

        public StaticMemoryPool(
            Action<TParam1, TParam2, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn(TParam1 p1, TParam2 p2)
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(p1, p2, item);
            }

            return item;
        }
    }

    // Three parameters

    public class StaticMemoryPool<TParam1, TParam2, TParam3, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TValue>
        where TValue : class, new()
    {
        Action<TParam1, TParam2, TParam3, TValue> _onSpawnMethod;

        public StaticMemoryPool(
            Action<TParam1, TParam2, TParam3, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TParam3, TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3)
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(p1, p2, p3, item);
            }

            return item;
        }
    }

    // Four parameters

    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : class, new()
    {
#if !NET_4_6
        ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TValue> _onSpawnMethod;

        public StaticMemoryPool(
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(p1, p2, p3, p4, item);
            }

            return item;
        }
    }

    // Five parameters

    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : class, new()
    {
#if !NET_4_6
        ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> _onSpawnMethod;

        public StaticMemoryPool(
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(p1, p2, p3, p4, p5, item);
            }

            return item;
        }
    }

    // Six parameters

    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        where TValue : class, new()
    {
#if !NET_4_6
        ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> _onSpawnMethod;

        public StaticMemoryPool(
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6)
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(p1, p2, p3, p4, p5, p6, item);
            }

            return item;
        }
    }

    // Seven parameters

    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>
        where TValue : class, new()
    {
#if !NET_4_6
        ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> _onSpawnMethod;

        public StaticMemoryPool(
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public
#if !NET_4_6
            ModestTree.Util.
#endif
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> OnSpawnMethod
        {
            set { _onSpawnMethod = value; }
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)
        {
            var item = SpawnInternal();

            if (_onSpawnMethod != null)
            {
                _onSpawnMethod(p1, p2, p3, p4, p5, p6, p7, item);
            }

            return item;
        }
    }
}
