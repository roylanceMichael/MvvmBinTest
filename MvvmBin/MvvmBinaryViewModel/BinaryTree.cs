using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmBinaryModel;

namespace MvvmBinaryViewModel
{
    public class BinaryTree : INotifyPropertyChanged
    {
        #region static properties
        public static List<Node> NodeList
        {
            get
            {
                return new List<Node>
                {
                    new Node{Value = 4, IsExpanded = true, IsSelected = true},
                    new Node{Value = 2, IsExpanded = true},
                    new Node{Value = 1, IsExpanded = true},
                    new Node{Value = 3, IsExpanded = true},
                    new Node{Value = 8, IsExpanded = true},
                    new Node{Value = 6, IsExpanded = true},
                    new Node{Value = 5, IsExpanded = true},
                    new Node{Value = 7, IsExpanded = true},
                };
            }
        }

        #endregion

        #region properties
        private DelegateCommand _addNodeCommand;
        public ICommand AddNodeCommand
        {
            get { return _addNodeCommand ?? (_addNodeCommand = new DelegateCommand(AddNodeEntrance)); }
        }

        private DelegateCommand _removeNodeCommand;
        public ICommand RemoveNodeCommand
        {
            get { return _removeNodeCommand ?? (_removeNodeCommand = new DelegateCommand(RemoveNodeEntrance)); }
        }

        private DelegateCommand _addDefaultTreeCommand;
        public ICommand AddDefaultTreeCommand
        {
            get { return _addDefaultTreeCommand ?? (_addDefaultTreeCommand = new DelegateCommand(AddDefaultTreeEntrance)); }
        }

        private int? _currentValue;
        public int? CurrentValue
        {
            get { return _currentValue; }
            set
            {
                if (_currentValue == value) return;
                _currentValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentValue"));
            }
        }

        private readonly ObservableCollection<Node> _headNodeList = new ObservableCollection<Node>();
        public ObservableCollection<Node> HeadNodeList
        {
            get { return _headNodeList; }
        }

        private Node _headNode;
        public Node HeadNode
        {
            get { return _headNode; }
            set
            {
                if (_headNode == value) return;
                if (_headNodeList.Contains(_headNode))
                {
                    _headNodeList.Remove(_headNode);
                    if (_headNode != null)
                    {
                        _headNode.Label = string.Empty;
                    }
                }
                _headNode = value;
                if (_headNode != null)
                {
                    _headNode.Label = "Head";
                }
                _headNodeList.Add(_headNode);
                PropertyChanged(this, new PropertyChangedEventArgs("HeadNode"));
            }
        }
        #endregion

        #region functions
        public void AddDefaultTreeEntrance(object param)
        {
            //null out current header
            HeadNode = null;
            //clean it up, force it. 
            //write clean method later
            GC.Collect();

            foreach (var node in NodeList)
            {
                AddNode(node);
            }
        }

        public void RemoveNodeEntrance(object param)
        {
            var node = param as Node;
            if (node != null)
            {
                RemoveNode(node);
            }
        }

        public void AddNodeEntrance(object param)
        {
            var newNode = new Node
                {
                    Value = CurrentValue ?? 5,
                    IsExpanded = true,
                    IsSelected = true
                };
            AddNode(newNode);
        }

        public bool Contains(Node node)
        {
            var result = FindNodeAndParent(node);
            return result.Item1 != null;
        }

        public bool RemoveNode(Node node)
        {
            var removeChildParentTuple = FindNodeAndParent(node);
            if (removeChildParentTuple.Item1 == null)
            {
                return false;
            }

            var currentNode = removeChildParentTuple.Item1;
            var parentNode = removeChildParentTuple.Item2;
            //Node we're removing has a null right 
            if (currentNode.RightNode == null)
            {
                if (parentNode == null)
                {
                    HeadNode = currentNode.LeftNode;
                }
                else
                {
                    var result = parentNode.CompareTo(currentNode);
                    if (result > 0)
                    {
                        parentNode.LeftNode = currentNode.LeftNode;
                    }
                    else if (result < 0)
                    {
                        parentNode.RightNode = currentNode.LeftNode;
                    }
                }
            }
            else if (currentNode.RightNode.LeftNode == null)
            {
                currentNode.RightNode.LeftNode = currentNode.LeftNode;

                if (parentNode == null)
                {
                    HeadNode = currentNode.RightNode;
                }
                else
                {
                    var result = parentNode.CompareTo(currentNode);
                    if (result > 0)
                    {
                        parentNode.LeftNode = currentNode.RightNode;
                    }
                    else if (result < 0)
                    {
                        parentNode.RightNode = currentNode.RightNode;
                    }
                }
            }
            else
            {
                var leftMost = currentNode.RightNode.LeftNode;
                var leftMostParent = currentNode.RightNode;

                while (leftMost.LeftNode != null)
                {
                    leftMostParent = leftMost;
                    leftMost = leftMost.LeftNode;
                }

                leftMostParent.LeftNode = leftMost.RightNode;

                leftMost.LeftNode = currentNode.LeftNode;
                leftMost.RightNode = currentNode.RightNode;

                if (parentNode == null)
                {
                    HeadNode = leftMost;
                }
                else
                {
                    var result = parentNode.CompareTo(currentNode);
                    if (result > 0)
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
                if (result > 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.LeftNode;
                }
                else if (result < 0)
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

        #endregion

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
    }
}
