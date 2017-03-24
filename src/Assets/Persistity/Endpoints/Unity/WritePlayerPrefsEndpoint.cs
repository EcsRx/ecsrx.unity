using System;
using System.Text;
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

        public void Execute(byte[] data, Action<object> onSuccess, Action<Exception> onError)
        {
            var stringData = Encoding.Default.GetString(data);

            try
            { PlayerPrefs.SetString(KeyName, stringData); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }
            onSuccess(null);
        }
    }
}