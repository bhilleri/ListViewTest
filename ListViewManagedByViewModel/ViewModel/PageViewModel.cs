using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListViewManagedByViewModel.ViewModel
{
    public class PageViewModel : ObservableObject
    {
        private ListViewModel listViewModel = new ListViewModel();
        public ListViewModel ListViewModel
        {
            get => listViewModel;
            private set => SetProperty(ref listViewModel, value);
        }
    }
}
