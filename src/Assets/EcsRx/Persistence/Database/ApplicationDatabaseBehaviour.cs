using System;
using EcsRx.Persistence.Data;
using UnityEngine;

namespace EcsRx.Persistence.Database
{
    [Serializable]
    public class ApplicationDatabaseBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ApplicationDatabase _applicationDatabase = new Data.ApplicationDatabase();

        public ApplicationDatabase ApplicationData
        {
            get { return _applicationDatabase; }
        }
    }
}