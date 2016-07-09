using System.Collections.Generic;
using UnityEngine;

namespace Assets.EcsRx.Unity.ViewPooling
{
    public interface IViewPool
    {
        int IncrementSize { get; }
        void PreAllocate(int allocationCount);

        GameObject AllocateInstance();
        void ReleaseInstance(GameObject instance);
        void EmptyPool();
    }
}