using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmBinaryModel;

namespace MvvmBinaryViewModel
{
    public static class ViewModelFactory
    {
        public static ViewModel TestViewModels()
        {
            var vm = new ViewModel
                {
                    HeadNode = new Node
                        {
                            Value = 5
                        }
                };
            return vm;
        }
    }
}
