using System;
using Persistity.Serialization;
using UnityEngine;

namespace Persistity.Endpoints.Unity
{
    public class WritePlayerPrefs : ISendDataEndpoint
    {
        public string KeyName { get; set; }

        public WritePlayerPrefs(string keyName)
        {
            KeyName = keyName;
        }

        public void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError)
        {
            try
            { PlayerPrefs.SetString(KeyName, data.AsString); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }
            onSuccess(null);
        }
    }
}