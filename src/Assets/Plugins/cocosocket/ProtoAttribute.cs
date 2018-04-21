using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace protocol
{
    [AttributeUsage(AttributeTargets.Class)]
    class ProtoAttribute : Attribute
    {
        public int value { get; set; }
		public string description { get; set; }
    }
}
