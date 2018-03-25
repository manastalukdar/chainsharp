using System;
using chainsharp.lib;
using Xunit;

namespace chainsharp.tests.unit.chainsharp.lib
{
    public static class UnitTest1
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
