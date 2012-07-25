using System;
using System.Collections.Generic;
using System.Linq;
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
            public void NullWhenAddNodeEntranceNull()
            {
                var viewModel = new BinaryTree { CurrentValue = 4 };
                viewModel.AddNodeEntrance(null);

                Assert.IsNotNull(viewModel.HeadNode);
            }

            [TestMethod]
            public void NotNullWhenAddNodeDefault()
            {
                var viewModel = new BinaryTree();
                viewModel.AddNodeCommand.Execute(null);

                Assert.IsNotNull(viewModel.HeadNode);
            }
            [TestMethod]
            public void CorrectOrderWhenTwoEntered()
            {
                var viewModel = new BinaryTree();
                viewModel.AddSearchNode(new Node
                {
                    Value = 5
                });

                viewModel.AddSearchNode(new Node
                {
                    Value = 10
                });

                Assert.IsNotNull(viewModel.HeadNode);
                Assert.IsNotNull(viewModel.HeadNode.RightNode);
            }
            [TestMethod]
            public void CorrectOrderWhenTwoEnteredLargeSmall()
            {
                var viewModel = new BinaryTree();
                viewModel.AddSearchNode(new Node
                {
                    Value = 10
                });

                viewModel.AddSearchNode(new Node
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
                var viewModel = new BinaryTree();

                viewModel.AddSearchNode(new Node
                {
                    Value = 10
                });

                var node = new Node
                    {
                        Value = 5
                    };

                viewModel.AddSearchNode(node);

                viewModel.AddSearchNode(new Node
                {
                    Value = 4
                });

                viewModel.AddSearchNode(new Node
                {
                    Value = 6
                });

                viewModel.AddSearchNode(new Node
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

            [TestMethod]
            public void SuccessWhenRemoveNodeNorightChild()
            {
                //arrange
                var vm = new BinaryTree();
                vm.AddDefaultTreeCommand.Execute(null);
                var eightNode = BinaryTree.NodeList.First(t => t.Value == 8);
                //act
                vm.RemoveNodeEntrance(eightNode);
                //assert
                Assert.IsFalse(vm.Contains(eightNode));
                Assert.IsTrue(vm.HeadNode.RightNode.Value == 6);
                Assert.IsTrue(vm.HeadNode.RightNode.LeftNode.Value == 5);
                Assert.IsTrue(vm.HeadNode.RightNode.RightNode.Value == 7);
            }

            [TestMethod]
            public void SuccessWhenRemoveNodeRightChildNoLeftChild()
            {
                //arrange
                var vm = new BinaryTree();
                var rightChildHasNoLeft = new List<Node>
                {
                    new Node{Value = 4, IsExpanded = true, IsSelected = true},
                    new Node{Value = 6, IsExpanded = true},
                    new Node{Value = 5, IsExpanded = true},
                    new Node{Value = 7, IsExpanded = true},
                    new Node{Value = 8, IsExpanded = true},
                };
                rightChildHasNoLeft.ForEach(vm.AddSearchNode);
                var sixthNode = rightChildHasNoLeft.First(t => t.Value == 6);
                //act
                vm.RemoveNodeEntrance(sixthNode);
                //assert
                Assert.IsFalse(vm.Contains(sixthNode));
                Assert.IsTrue(vm.HeadNode.RightNode.Value == 7);
                Assert.IsTrue(vm.HeadNode.RightNode.LeftNode.Value == 5);
                Assert.IsTrue(vm.HeadNode.RightNode.RightNode.Value == 8);
            }

            [TestMethod]
            public void SuccessWhenRemoveNodeRightChildHasLeftChild()
            {
                //arrange
                var vm = new BinaryTree();
                var rightChildHasNoLeft = new List<Node>
                {
                    new Node{Value = 4, IsExpanded = true, IsSelected = true},
                    new Node{Value = 6, IsExpanded = true},
                    new Node{Value = 5, IsExpanded = true},
                    new Node{Value = 8, IsExpanded = true},
                    new Node{Value = 7, IsExpanded = true},
                };
                rightChildHasNoLeft.ForEach(vm.AddSearchNode);
                var sixthNode = rightChildHasNoLeft.First(t => t.Value == 6);
                //act
                vm.RemoveNodeEntrance(sixthNode);
                //assert
                Assert.IsFalse(vm.Contains(sixthNode));
                Assert.IsTrue(vm.HeadNode.RightNode.Value == 7);
                Assert.IsTrue(vm.HeadNode.RightNode.LeftNode.Value == 5);
                Assert.IsTrue(vm.HeadNode.RightNode.RightNode.Value == 8);
            }
        }

        [TestClass]
        public class AddDefaultTree
        {
            [TestMethod]
            public void CorrectNodesWhenDefaultPathEntered()
            {
                //arrange
                var vm = new BinaryTree();
                //act
                vm.AddDefaultTreeCommand.Execute(null);
                //assert
                Assert.IsTrue(vm.Contains(BinaryTree.NodeList[1]));
            }
        }

        [TestClass]
        public class ValidateSearchTree
        {
            [TestMethod]
            public void SuccessWhenHeadNodeInserted()
            {
                var vm = new BinaryTree();
                var list = BinaryTree.NodeList;
                vm.AddBalancedNode(list[0]);
                Assert.IsTrue(vm.HeadNode == list[0]);
            }

            [TestMethod]
            public void SuccessWhenFiveInserted()
            {
                var vm = new BinaryTree();
                var list = BinaryTree.NodeList;
                for (var i = 0; i < 6; i++)
                {
                    vm.AddBalancedNode(list[i]);
                }

                Assert.IsTrue(vm.HeadNode == list[0]);
                Assert.IsTrue(vm.HeadNode.LeftNode == list[1]);
                Assert.IsTrue(vm.HeadNode.RightNode == list[2]);
                Assert.IsTrue(vm.HeadNode.LeftNode.LeftNode == list[3]);
                Assert.IsTrue(vm.HeadNode.LeftNode.RightNode == list[4]);
            }

            [TestMethod]
            public void FailureWhenNotBinarySearchTree()
            {
                var vm = new BinaryTree();
                var list = BinaryTree.NodeList;
                for (var i = 0; i < 6; i++)
                {
                    vm.AddBalancedNode(list[i]);
                }
                Assert.IsFalse(vm.IsThisBinaryTree());
            }

            [TestMethod]
            public void SuccessWhenBinarySearchTree()
            {
                var vm = new BinaryTree();
                vm.AddDefaultTreeEntrance(null);
                Assert.IsTrue(vm.IsThisBinaryTree());
            }
        }
    }
}
