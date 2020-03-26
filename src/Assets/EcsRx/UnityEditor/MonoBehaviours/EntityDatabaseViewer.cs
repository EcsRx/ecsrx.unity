using EcsRx.Collections.Database;
using UnityEngine;
using Zenject;

namespace EcsRx.UnityEditor.MonoBehaviours
{
    public class EntityDatabaseViewer : MonoBehaviour
    {
         [Inject]
         public IEntityDatabase EntityDatabase { get; private set; }
    }
}