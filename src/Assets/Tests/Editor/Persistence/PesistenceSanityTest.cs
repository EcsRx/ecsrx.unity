using System;
using Assets.Tests.Editor.Helpers;
using NUnit.Framework;

namespace Assets.Tests.Editor.Persistence
{
    [TestFixture]
    public class PesistenceSanityTest
    {
        private ComponentDescriptorHelper helper = new ComponentDescriptorHelper();
        private SuperHelper superHelper = new SuperHelper();

        [Test]
        public void should_not_hurt_my_brain()
        {
            var a = new A();
            a.TestValue = "Wow";
            a.NestedValue = new B();
            a.NestedValue.IntValue = 10;
            a.NestedValue.StringValue = "Hello";
            a.NestedValue.NestedArray = new [] { new C { FloatValue = 2.0f } };
            a.NestedArray = new B[2];
            a.NestedArray[0] = new B
            {
                IntValue = 20,
                StringValue = "There",
                NestedArray = new[] { new C { FloatValue = 3.0f } }
            };
            a.NestedArray[1] = new B
            {
                IntValue = 30,
                StringValue = "Sir",
                NestedArray = new []
                {
                     new C { FloatValue = 4.0f },
                     new C { FloatValue = 5.0f }
                }
            };

            var typeStuff = superHelper.GetTypeMappingsFor(typeof(A));

            var output = typeStuff.SerializeData(a);
            Console.WriteLine(output);
            return;
        }
    }
}