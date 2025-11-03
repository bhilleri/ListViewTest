using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ListViewManagedByViewModel.ViewModel
{
    public class ListViewModel : ObservableObject
    {

        private static readonly ThreadLocal<Random> threadLocalRand = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
        //private readonly ListCollectionView _view;
        Sorter sorterInstance = new Sorter();

        private List<ItemViewModel> items = new List<ItemViewModel>();

        //private MintPlayer.ObservableCollection.ObservableCollection<ItemViewModel> listItems = new MintPlayer.ObservableCollection.ObservableCollection<ItemViewModel>();
        //public MintPlayer.ObservableCollection.ObservableCollection<ItemViewModel> ListItems
        //{
        //    get => listItems;
        //    set => SetProperty(ref listItems, value);
        //}

        private ObservableCollection<ItemViewModel> listItems = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> ListItems
        {
            get => listItems;
            set => SetProperty(ref listItems, value);
        }

        public ListViewModel()
        {
            sorterInstance = new Sorter();
            commandInViewModel = new AsyncRelayCommand<string?>(ChangeOrder);

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                items.Add(new ItemViewModel("Document.txt", false, "Text Document", DateTime.Now.AddDays(-2), ElementType.File));
                items.Add(new ItemViewModel("Pictures", true, "Image Folder", DateTime.Now.AddMonths(-1), ElementType.Folder));
                items.Add(new ItemViewModel("Shortcut to App", false, "Application Shortcut", DateTime.Now.AddDays(-10), ElementType.Shortcut));
                items.Add(new ItemViewModel("Music.mp3", true, "Audio File", DateTime.Now.AddYears(-1), ElementType.File));
                items.Add(new ItemViewModel("Videos", false, "Video Folder", DateTime.Now.AddMonths(-3), ElementType.Folder));
            }
            else
            {
                for (int i = 0; i < 1000000; i++)
                {
                    items.Add(ItemViewModel.GenerateRandom());
                }
            }

            //ListItems.AddRange(items);
            //_view = (ListCollectionView)CollectionViewSource.GetDefaultView(ListItems);
            //_view.CustomSort = sorterInstance;
            ListItems = new ObservableCollection<ItemViewModel>(items);
        }

        private async Task ResortList()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            List<ItemViewModel> tempList = new List<ItemViewModel>();

            this.UpdateList();

            //_view.Refresh();

            tempList = this.items.AsParallel().AsOrdered().WithDegreeOfParallelism(Environment.ProcessorCount).OrderBy(x => x, sorterInstance).ToList();
            this.ListItems = new ObservableCollection<ItemViewModel>(tempList);
            await System.Windows.Threading.Dispatcher.Yield(System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            var collectionTime = sw.ElapsedMilliseconds;

            Debug.WriteLine($"Tri : {collectionTime} ms");

            //Debug.WriteLine($"Tri: {sortTime} ms, Mise à jour ObservableCollection: {collectionTime} ms");
            //this.ListItems = new ObservableCollection<ItemViewModel>(this.ListItems.OrderBy(x => x, sorterInstance));
        }

        public void UpdateList()
        {
            Random rand = threadLocalRand.Value;
            List<ItemViewModel> newItems = new List<ItemViewModel>();
            List<ItemViewModel> itemsTORemove = new List<ItemViewModel>();
            foreach (ItemViewModel item in items)
            {
                int a= rand.Next(0, 2);
                if(a == 1)
                {
                    newItems.Add(item);
                }
                else
                {
                    itemsTORemove.Add(item);
                }
            }

            foreach (var item in itemsTORemove)
            {
                if (items.Contains(item))
                {
                    items.Remove(item);
                }
            }



            ItemViewModel[] itemsToAddTab = new ItemViewModel[500000];

            Parallel.For(0, 500000, (int i) =>
            {
                Random rand = threadLocalRand.Value;
                itemsToAddTab[i] = ItemViewModel.GenerateRandom(rand);
            });

            newItems.AddRange(itemsToAddTab);
            items.Clear();
            this.items = newItems;

            IEnumerable<ItemViewModel> removedItems = ListItems.Except(items).ToList();
            IEnumerable<ItemViewModel> addedItems = items.Except(ListItems).ToList();

            //this.listItems.RemoveRange(removedItems);
            //this.listItems.AddRange(addedItems);

        }

        public ICommand commandInViewModel { get; private set; }

        private async Task ChangeOrder(string? value)
        {
            if(Enum.TryParse(value, out SortType newSortType) == false)
            {
                newSortType = SortType.Default;
            }
            
            if (currentSortedProperty == newSortType)
            {
                switch (newSortType)
                {
                    case SortType.Name:
                        SortNameIsDescending = !SortNameIsDescending;
                        break;
                    case SortType.CreatedAt:
                        SortCreateAteIsDescending = !SortCreateAteIsDescending;
                        break;
                    case SortType.Type:
                        SortTypeIsDescending = !SortTypeIsDescending;
                        break;
                    case SortType.Description:
                        SortDescriptionIsDescending = !SortDescriptionIsDescending;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                CurrentSortedProperty = newSortType;
            }

            switch(CurrentSortedProperty)
            {
                case SortType.Name:
                    sorterInstance.SetSort(SortType.Name, SortNameIsDescending);
                    break;
                case SortType.CreatedAt:
                    sorterInstance.SetSort(SortType.CreatedAt, SortCreateAteIsDescending);
                    break;
                case SortType.Type:
                    sorterInstance.SetSort(SortType.Type, SortTypeIsDescending);
                    break;
                case SortType.Description:
                    sorterInstance.SetSort(SortType.Description, SortDescriptionIsDescending);
                    break;
                case SortType.Default:
                default:
                    sorterInstance.SetSort(SortType.Default, true);
                    break;
            }

            await ResortList();
        }

        private bool sortNameIsDescending = true;
        public bool SortNameIsDescending
        {
            get => sortNameIsDescending;
            set => SetProperty(ref sortNameIsDescending, value);
        }
        private bool sortCreateAteIsDescending = true;
        public bool SortCreateAteIsDescending
        {
            get => sortCreateAteIsDescending;
            set => SetProperty(ref sortCreateAteIsDescending, value);
        }
        private bool sortDescriptionIsDescending = true;
        public bool SortDescriptionIsDescending
        {
            get => sortDescriptionIsDescending;
            set => SetProperty(ref sortDescriptionIsDescending, value);
        }
        private bool sortTypeIsDescending = true;
        public bool SortTypeIsDescending
        {
            get => sortTypeIsDescending;
            set => SetProperty(ref sortTypeIsDescending, value);
        }

        private SortType currentSortedProperty = SortType.Default;
        public SortType CurrentSortedProperty
        {
            get => currentSortedProperty;
            set => SetProperty(ref currentSortedProperty, value);
        }
    }
}
