using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmBinaryModel
{
    public class Node : IComparable, INotifyPropertyChanged
    {
        #region properties
        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                if (_label == value) return;
                _label = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Label"));
            }
        }
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsExpanded"));
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        private readonly ObservableCollection<Node> _children = new ObservableCollection<Node>();
        public ObservableCollection<Node> Children
        {
            get
            {
                return _children;
            }
        }
        private Node _leftNode;
        public Node LeftNode
        {
            get { return _leftNode; }
            set
            {
                if (_leftNode == value) return;
                if (_children.Contains(_leftNode))
                {
                    _children.Remove(_leftNode);
                    if (_leftNode != null)
                    {
                        _leftNode.Label = string.Empty;
                    }
                }

                _leftNode = value;
                if (_leftNode != null)
                {
                    _children.Add(_leftNode);
                    _leftNode.Label = "Left";
                }
                //add in appropriate place
                PropertyChanged(this, new PropertyChangedEventArgs("LeftNode"));
                PropertyChanged(this, new PropertyChangedEventArgs("Children"));
            }
        }

        private Node _rightNode;
        public Node RightNode
        {
            get { return _rightNode; }
            set
            {
                if (_rightNode == value) return;
                if (_children.Contains(_rightNode))
                {
                    _children.Remove(_rightNode);
                    if (_rightNode != null)
                    {
                        _rightNode.Label = string.Empty;
                    }
                }
                _rightNode = value;
                if (_rightNode != null)
                {
                    _children.Insert(0, _rightNode);
                    _rightNode.Label = "Right";
                }

                PropertyChanged(this, new PropertyChangedEventArgs("RightNode"));
                PropertyChanged(this, new PropertyChangedEventArgs("Children"));
            }
        }
        #endregion

        #region functions
        public void AddBinarySearchNode(Node node)
        {
            if (Value <= node.Value)
            {
                if (RightNode == null)
                {
                    RightNode = node;
                }
                else
                {
                    RightNode.AddBinarySearchNode(node);
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
                    LeftNode.AddBinarySearchNode(node);
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
            if (Value > castedObject.Value)
            {
                return 1;
            }
            if (Value == castedObject.Value)
            {
                return 0;
            }
            return -1;
        }
        #endregion

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
    }
}
