using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmBinaryModel;

namespace Tests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void CtorTest()
        {
            //arrange
            //act
            var node = new Node {Value = 5};
            
            //assert
            Assert.AreEqual(5, node.Value);
        }
    }
}
