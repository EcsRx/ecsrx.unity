using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Loader
{
    public class LocalPriorLoadStrategy : ILoadStrategy
    {

        public string Folder { get; set; }
        private LocalFileLoader localFileLoader;
        private RemoteFileLoader remoteFileLoader;

        public string path
        {
            get
            {
                //pc,ios //android :jar:file//  
                return Application.persistentDataPath + Folder;

            }
        }

        public LocalPriorLoadStrategy(LocalFileLoader localFileLoader, RemoteFileLoader remoteFileLoade)
        {
            this.localFileLoader = localFileLoader;
            this.remoteFileLoader = remoteFileLoade;
            Folder = "/Cache/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public WWW Load(string url)
        {

            if (!File.Exists(path + url.GetHashCode()))
            {
                //如果之前不存在缓存文件
                var file = remoteFileLoader.Load(url);
                if (file != null)
                {
                    File.WriteAllBytes(path + url.GetHashCode(), file.bytes);
                }
                return file;
            }
            else
            {
                return localFileLoader.Load(url);
            }
        }
    }
}
