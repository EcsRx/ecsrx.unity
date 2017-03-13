using System;
using Assets.Tests.Editor.Helpers;
using NUnit.Framework;
using Tests.Editor.Helpers.Mapping;

namespace Assets.Tests.Editor.Persistence
{
    [TestFixture]
    public class PesistenceSanityTest
    {
        private ComponentDescriptorHelper helper = new ComponentDescriptorHelper();
        private SuperHelper superHelper = new SuperHelper();

        private A GenerateDummyData()
        {
            var a = new A();
            a.TestValue = "Wow";
            a.NestedValue = new B();
            a.NestedValue.IntValue = 10;
            a.NestedValue.StringValue = "Hello";
            a.NestedValue.NestedArray = new[] { new C { FloatValue = 2.43f } };
            a.NestedArray = new B[2];
            a.NestedArray[0] = new B
            {
                IntValue = 20,
                StringValue = "There",
                NestedArray = new[] { new C { FloatValue = 3.5f } }
            };
            a.NestedArray[1] = new B
            {
                IntValue = 30,
                StringValue = "Sir",
                NestedArray = new[]
                {
                     new C { FloatValue = 4.1f },
                     new C { FloatValue = 5.2f }
                }
            };

            return a;
        }

        private string GenerateDummyJsonData()
        {
            return @"{
               ""TestValue"":""Wow"",
               ""NestedValue"":{
                  ""StringValue"":""Hello"",
                  ""IntValue"":10,
                  ""NestedArray"":[
                     {
                        ""FloatValue"":""2.43""
                     }
                  ]
               },
               ""NestedArray"":[
                  {
                     ""StringValue"":""There"",
                     ""IntValue"":20,
                     ""NestedArray"":[
                        {
                           ""FloatValue"":""3.5""
                        }
                     ]
                  },
                  {
                     ""StringValue"":""Sir"",
                     ""IntValue"":30,
                     ""NestedArray"":[
                        {
                           ""FloatValue"":""4.1""
                        },
                        {
                           ""FloatValue"":""5.2""
                        }
                     ]
                  }
               ]
            }";
        }

        [Test]
        public void should_serialize_with_debug_serializer()
        {
            var a = GenerateDummyData();
            var typeStuff = superHelper.GetTypeMappingsFor(typeof(A));

            var output = typeStuff.SerializeData(a);
            Console.WriteLine(output);
            return;
        }

        [Test]
        public void should_correctly_serialize_to_json()
        {
            var a = GenerateDummyData();

            var typeStuff = superHelper.GetTypeMappingsFor(typeof(A));
            var serializer = new JsonSerializer();
            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine(output.ToString());
            return;
        }

        [Test]
        public void should_correctly_deserialize_from_json()
        {
            var a = GenerateDummyJsonData();
            var typeStuff = superHelper.GetTypeMappingsFor(typeof(A));
            var deserializer = new JsonDeserializer();

            var output = deserializer.DeserializeData<A>(typeStuff, a);
            return;
        }
    }
}