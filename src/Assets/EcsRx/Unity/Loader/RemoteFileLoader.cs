using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.WSA;
using Zenject;

namespace EcsRx.Unity.Loader
{
    public class RemoteFileLoader : IFileLoader
    {
     
        public virtual WWW Load(string path)
        {
            var result = ObservableWWW.GetWWW(path);
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
