using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Crypto;
using EcsRx.ErrorHandle;
using EcsRx.Json;
using EcsRx.Serialize;
using EcsRx.Unity.DataSource;
using EcsRx.Unity.Exception;
using UniRx;
using UnityEngine;

namespace EcsRx.Net
{
    public abstract class HttpProtocol : IHttpProtocol
    {
        private IHttpErrorHandle errorHandle;
        public ISerialize Serialize { get; set; }
        public IDeserialize Deserialize { get; set; }
        public ICrypto Crypto { get; set; }
        public Dictionary<string, string> Header { get; set; } 

        public HttpProtocol(IJsonSerialize serialize, IJsonDeserialize deserialize, ICrypto crypto, IHttpErrorHandle errorHandle)
        {
            Serialize = serialize;
            Deserialize = deserialize;
            Crypto = crypto;
            this.errorHandle = errorHandle;
            Header = new Dictionary<string, string>();
        }


        protected abstract string EncodeMessage<TIn>(HttpRequestMessage<TIn> message) where TIn : struct;
        protected abstract HttpResponseMessage<TOut> DecodeMessage<TOut>(string data) where TOut : struct;

        public IObservable<TOut> Post<TIn, TOut>(HttpRequestMessage<TIn> message) where TIn : struct where TOut : struct 
        {
            string request = EncodeMessage(message);
            Debug.Log("Http Path: " + message.Url + message.Path);
            Debug.Log("HttpRequest Request: " + request);

            var subject = new Subject<TOut>();
            ObservableWWW.Post(message.Url + message.Path, Encoding.UTF8.GetBytes(request), Header).CatchIgnore(
                (WWWErrorException ex) =>
                {
                    Debug.Log(ex.RawErrorMessage);
                    errorHandle.Handel(new HttpException(ex.RawErrorMessage,
                        -1));
                }).Subscribe(data =>
            {
                var response = DecodeMessage<TOut>(data);
                if (response.IsOK)
                {
                    Debug.Log("HttpRequest Response: " + data);
                    subject.OnNext(response.Data);
                }
                else
                {
                    Debug.LogError("HttpRequest Response: " + data);
                    errorHandle.Handel(new HttpException(response.ResultMessage,
                        Convert.ToInt32(response.ResultCode)));
                }
                subject.OnNext(response.Data);
                subject.OnCompleted();
            }
            );

            return subject;
        }
    }
}
