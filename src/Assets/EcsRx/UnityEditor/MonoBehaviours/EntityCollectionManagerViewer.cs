using EcsRx.Collections;
using UnityEngine;
using Zenject;

namespace EcsRx.UnityEditor.MonoBehaviours
{
    public class EntityCollectionManagerViewer : MonoBehaviour
    {
         [Inject]
         public IEntityCollectionManager CollectionManager { get; private set; }
    }
}