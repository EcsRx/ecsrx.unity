using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Loader
{
    public class LocalFileLoader : IFileLoader
    {
        public virtual WWW Load(string path)
        {
            var result = ObservableWWW.GetWWW("file://" + path);
            WWW data = null;
            result.Subscribe(www =>
            {
                data = www;
                Debug.Log("file load Response: ");
            },
            Debug.LogException
            );
            return data;
        }
    }
}
