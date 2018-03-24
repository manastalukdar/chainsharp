using System;
using main.lib.chainsharp;
using Xunit;

namespace tests.unit.lib.chainsharp
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //=====Arrange=====
            Class1 class1 = new Class1();

            //=====Act=====
            class1.dummy();

            //=====Assert=====
        }
    }
}
