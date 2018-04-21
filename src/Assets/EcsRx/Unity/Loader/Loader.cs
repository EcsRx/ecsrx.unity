using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EcsRx.Unity.Loader
{
    public abstract class Loader<T> : ILoader
    {
        protected ILoadStrategy loadStrategy;
        protected LocalFileLoader localFileLoader;
        protected Loader(ILoadStrategy loadStrategy, LocalFileLoader localFileLoader)
        {
            this.loadStrategy = loadStrategy;
            this.localFileLoader = localFileLoader;
        }  

        public abstract T LoadFromLocal(string url);
        public abstract T LoadFromRemote(string url);
    }
}
