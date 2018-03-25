using System;
using main.lib.chainsharp;
using Xunit;

namespace tests.unit.lib.chainsharp
{
    public class UnitTest1
    {
        [Fact]
        public static void Test1()
        {
            //=====Arrange=====

            //=====Act=====
            var cwd = Class1.dummy();
            var cwdThis = Environment.CurrentDirectory;

            //=====Assert=====
            Assert.Equal(cwd, cwdThis);
        }
    }
}
