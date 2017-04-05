using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcsRx.Persistence.Database
{
    [Serializable]
    public class ApplicationDatabase
    {
        [SerializeField]
        private IList<ApplicationEntityLink> _entityData;

        [SerializeField]
        private string _version = "1.0.0";

        public IList<ApplicationEntityLink> EntityData
        {
            get { return _entityData; }
            set { _entityData = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public ApplicationDatabase()
        {
            _entityData = new List<ApplicationEntityLink>();
        }
    }
}