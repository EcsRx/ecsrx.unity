using System;

namespace Zenject.TestFramework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WaitTimeSecondsAttribute : Attribute
    {
        public WaitTimeSecondsAttribute(float seconds)
        {
            Seconds = seconds;
        }

        public float Seconds
        {
            get;
            private set;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExpectedExceptionAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExpectedValidationExceptionAttribute : Attribute
    {
    }
}

