using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmBinaryModel;

namespace MvvmBinaryViewModel
{
    public class ViewModel
    {
        public Node HeadNode { get; set; }

        public bool Contains(Node node)
        {
            var result = FindNodeAndParent(node);
            return result.Item1 != null;
        }

        public bool RemoveNode(Node node)
        {
            var removeChildParentTuple = FindNodeAndParent(node);
            if(removeChildParentTuple.Item1 == null)
            {
                return false;
            }

            var currentNode = removeChildParentTuple.Item1;
            var parentNode = removeChildParentTuple.Item2;
            //Node we're removing has a null right 
            if(currentNode.RightNode == null)
            {
                if(parentNode == null)
                {
                    HeadNode = currentNode.LeftNode;
                }
                else
                {
                    var result = parentNode.CompareTo(currentNode.Value);
                    if(result > 0)
                    {
                        parentNode.LeftNode = currentNode.LeftNode;
                    }
                    else if(result < 0)
                    {
                        parentNode.RightNode = currentNode.LeftNode;
                    }
                }
            }
            else if(currentNode.RightNode.LeftNode == null)
            {
                currentNode.RightNode.LeftNode = currentNode.LeftNode;

                if(parentNode == null)
                {
                    HeadNode = currentNode.RightNode;
                }
                else
                {
                    var result = parentNode.CompareTo(currentNode.Value);
                    if(result > 0)
                    {
                        parentNode.LeftNode = currentNode.RightNode;
                    }
                    else if(result < 0)
                    {
                        parentNode.RightNode = currentNode.RightNode;
                    }
                }
            }
            else
            {
                var leftMost = currentNode.RightNode.LeftNode;
                var leftMostParent = currentNode.RightNode;

                while(leftMost.LeftNode != null)
                {
                    leftMostParent = leftMost;
                    leftMost = leftMost.LeftNode;
                }

                leftMostParent.LeftNode = leftMost.RightNode;

                leftMost.LeftNode = currentNode.LeftNode;
                leftMost.RightNode = currentNode.RightNode;

                if(parentNode == null)
                {
                    HeadNode = leftMost;
                }
                else
                {
                    var result = parentNode.CompareTo(currentNode.Value);
                    if(result > 0)
                    {
                        parentNode.LeftNode = leftMost;
                    }
                    else if (result < 0)
                    {
                        parentNode.RightNode = leftMost;
                    }
                }
            }
            return true;
        }

        public Tuple<Node, Node> FindNodeAndParent(Node node)
        {
            var currentNode = HeadNode;
            Node parent = null;
            while (currentNode != null)
            {
                var result = currentNode.CompareTo(node);
                if(result > 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.LeftNode;
                }
                else if(result < 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.RightNode;
                }
                else
                {
                    break;
                }
            }
            return new Tuple<Node, Node>(currentNode, parent);
        }

        public void AddNode(Node node)
        {
            if (HeadNode == null)
            {
                HeadNode = node;
            }
            else
            {
                HeadNode.AddNode(node);
            }
        }
    }
}
