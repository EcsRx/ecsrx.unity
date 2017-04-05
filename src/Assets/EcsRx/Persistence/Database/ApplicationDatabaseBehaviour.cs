using System;
using UnityEngine;

namespace EcsRx.Persistence.Database
{
    [Serializable]
    public class ApplicationDatabaseBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ApplicationDatabase _applicationDatabase = new ApplicationDatabase();

        public ApplicationDatabase ApplicationData
        {
            get { return _applicationDatabase; }
        }
    }
}