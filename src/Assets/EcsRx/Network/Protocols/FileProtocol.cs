using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Crypto;
using EcsRx.Serialize;
using UniRx;
using UnityEngine;

namespace EcsRx.Network
{
    public class FileProtocol : IFileProtocol
    {
        public ISerialize Serialize { get; set; }
        public IDeserialize Deserialize { get; set; }
        public ICrypto Crypto { get; set; }
        public IObservable<WWW> Load(string path)
        {
            var result = ObservableWWW.GetWWW("file://" + path);

            result.Subscribe(www =>
            {
                Debug.Log("file load Response: ");
            },
            Debug.LogException
            );

            return result;
        }
    }
}
