using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Processing.BackgroundRemove.Tests
{
    [TestFixture]
    class CheckRemove
    {
        [Test]
        public void TestRB()
        {
            var res =  UnsafeNativeMethods.Sample_Call(10);
            Assert.AreEqual(100, res);
        }
    }
}
