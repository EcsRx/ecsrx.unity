using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EcsRx.Unity.Loader
{
    public class ImageLoader : Loader<Texture2D>
    {

        public ImageLoader(ILoadStrategy loadStrategy, LocalFileLoader localFileLoader) : base(loadStrategy, localFileLoader)
        {
            loadStrategy.Folder = "/ImageCache/";
        }

        public override Texture2D LoadFromLocal(string url)
        {
            var www = localFileLoader.Load(url);
            return www != null ? www.texture : null;
        }

        public override Texture2D LoadFromRemote(string url)
        {
            var www = loadStrategy.Load(url);
            return  www != null ? www.texture : null;
        }
    }
}
