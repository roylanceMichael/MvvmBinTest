using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmBinaryModel
{
    public class Node : IComparable
    {
        public int Value { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }

        public void AddNode(Node node)
        {
            if (Value <= node.Value)
            {
                if (RightNode == null)
                {
                    RightNode = node;
                }
                else
                {
                    RightNode.AddNode(node);
                }
            }
            else
            {
                if (LeftNode == null)
                {
                    LeftNode = node;
                }
                else
                {
                    LeftNode.AddNode(node);
                }
            }
        }

        public int CompareTo(object obj)
        {
            var castedObject = obj as Node;
            if (castedObject == null)
            {
                return 1;
            }
            if(Value > castedObject.Value)
            {
                return 1;
            }
            if(Value == castedObject.Value)
            {
                return 0;
            }
            return -1;
        }
    }
}
