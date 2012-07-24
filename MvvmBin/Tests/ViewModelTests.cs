using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmBinaryModel;
using MvvmBinaryViewModel;

namespace Tests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestClass]
        public class AddNode
        {
            [TestMethod]
            public void NotNullWhenAddNode()
            {
                var viewModel = new ViewModel();
                viewModel.AddNode(new Node
                {
                    Value = 5
                });

                Assert.IsNotNull(viewModel.HeadNode);
            }
            [TestMethod]
            public void CorrectOrderWhenTwoEntered()
            {
                var viewModel = new ViewModel();
                viewModel.AddNode(new Node
                {
                    Value = 5
                });

                viewModel.AddNode(new Node
                {
                    Value = 10
                });

                Assert.IsNotNull(viewModel.HeadNode);
                Assert.IsNotNull(viewModel.HeadNode.RightNode);
            }
            [TestMethod]
            public void CorrectOrderWhenTwoEnteredLargeSmall()
            {
                var viewModel = new ViewModel();
                viewModel.AddNode(new Node
                {
                    Value = 10
                });

                viewModel.AddNode(new Node
                {
                    Value = 5
                });

                Assert.IsNotNull(viewModel.HeadNode);
                Assert.IsNotNull(viewModel.HeadNode.LeftNode);
            }
        }

        [TestClass]
        public class RemoveNode
        {
            [TestMethod]
            public void CorrectOrderWhenFiveEntered()
            {
                //arrange
                /* before
                     10 
                  5
                 4  6 
                2  
                 */
                //remove 5
                /* after
                    10 
                 4
                2  6 
                */
                var viewModel = new ViewModel();
                
                viewModel.AddNode(new Node
                {
                    Value = 10
                });

                var node = new Node
                    {
                        Value = 5
                    };

                viewModel.AddNode(node);

                viewModel.AddNode(new Node
                {
                    Value = 4
                });

                viewModel.AddNode(new Node
                {
                    Value = 6
                });
                
                viewModel.AddNode(new Node
                {
                    Value = 2
                });
                //state check
                Assert.IsTrue(viewModel.Contains(node));
                //act
                viewModel.RemoveNode(node);

                //assert
                Assert.IsFalse(viewModel.Contains(node));
            }
        }
    }
}
