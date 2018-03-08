using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.ErrorHandle
{
    public interface IErrorHandle
    {
        void Handel(Exception e);
    }
}
