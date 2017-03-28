using System;
using Persistity.Serialization;
using UnityEngine;

namespace Persistity.Endpoints.Unity
{
    public class ReadPlayerPrefsEndpoint : IReceiveDataEndpoint
    {
        public string KeyName { get; set; }

        public ReadPlayerPrefsEndpoint(string keyName)
        {
            KeyName = keyName;
        }

        public void Execute(Action<DataObject> onSuccess, Action<Exception> onError)
        {
            try
            {
                var data = PlayerPrefs.GetString(KeyName);
                onSuccess(new DataObject(data));
            }
            catch (Exception ex)
            {
                onError(ex);
            }
        }
    }
}