using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace EcsRx.Net
{
    public interface IFileProtocol : IProtocol
    {
        IObservable<WWW> Load(string path);
    }
}
