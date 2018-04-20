using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.ErrorHandle;
using EcsRx.Network;
using EcsRx.Unity.Exception;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Network
{
    public class HttpRequest
    {
        public Dictionary<string, string> Header { get; set; }
        public string Url { get; set; }

        private IHttpProtocol protocol;
        private IHttpErrorHandle errorHandle;

        public HttpRequest(IHttpProtocol protocol, IHttpErrorHandle errorHandle)
        {
            this.protocol = protocol;
            this.errorHandle = errorHandle;
            Header["Content-Type"] = "application/json";
        }

        public IObservable<TOut> Post<TIn, TOut, TResponse>(HttpRequestMessage<TIn> message) where TIn : struct where TOut : struct where TResponse : HttpResponseMessage<TOut>, new()
        {
            string request = protocol.EncodeMessage(message);
            Debug.Log("Http Path: " + Url + message.Path);
            Debug.Log("HttpRequest Request: " + request);

            var subject = new Subject<TOut>();
            ObservableWWW.Post(Url + message.Path, Encoding.UTF8.GetBytes(request), Header).CatchIgnore(
                (WWWErrorException ex) =>
                {
                    Debug.Log(ex.RawErrorMessage);
                    errorHandle.Handel(new HttpException(ex.RawErrorMessage,
                        -1));
                }).Subscribe(data =>
                {
                    var response = protocol.DecodeMessage<TOut, TResponse>(data);
                    if (response.IsOK)
                    {
                        Debug.Log("HttpRequest Response: " + data);
                        subject.OnNext(response.Data);
                    }
                    else
                    {
                        Debug.LogError("HttpRequest Response: " + data);
                        errorHandle.Handel(new HttpException(response.ErrorMessage,
                            Convert.ToInt32(response.ErrorCode)));
                    }
                    subject.OnNext(response.Data);
                    subject.OnCompleted();
                }
            );

            return subject;
        }
    }
}
