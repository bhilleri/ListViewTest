using System.Collections;
using System.Runtime.CompilerServices;

namespace ListViewManagedByViewModel.ViewModel
{
    public class Sorter : IComparer<ItemViewModel>, IComparer
    {
        private SortType ActiveSort = SortType.Default;
        private bool Descending = true;

        public void SetSort(SortType sortType, bool descending)
        {
            ActiveSort = sortType;
            Descending = descending;
        }

        public int Compare(ItemViewModel? itemA, ItemViewModel? itemB)
        {
            if (itemA == null && itemB == null)
            {
                return 0;   // equal
            }
            else if (itemA == null)
            {
                return 1;  // A is after B
            }
            else if (itemB == null)
            {
                return -1;   // A is before B
            }
            int result = 0;
            switch (ActiveSort)
            {
                case SortType.Name:
                    result = ManageNameOrder(itemA, itemB);
                    break;
                case SortType.CreatedAt:
                    result = ManageCreatedAtOrder(itemA, itemB);
                    break;
                case SortType.Type:
                    result = ManageTypeOrder(itemA, itemB);
                    break;
                case SortType.Description:
                    result = ManageDescriptionOrder(itemA, itemB);
                    break;
                case SortType.Default:
                default:
                    result = ManageDefaultOrder(itemA, itemB);
                    break;
            }
            return result;
        }

        private int ManageDefaultOrder(ItemViewModel itemA, ItemViewModel itemB)
        {
            return CompareTwoProperties(itemA.Name, itemB.Name, false, true);
        }

        private int ManageNameOrder(ItemViewModel itemA, ItemViewModel itemB)
        {
            return CompareTwoProperties(itemA.Name, itemB.Name, true, true);
        }

        private int ManageDescriptionOrder(ItemViewModel itemA, ItemViewModel itemB)
        {
            int result = CompareTwoProperties(itemA.Description, itemB.Description, true);
            if (result == 0)
            {
                result = ManageDefaultOrder(itemA, itemB);
            }
            return result;
        }

        private int ManageCreatedAtOrder(ItemViewModel itemA, ItemViewModel itemB)
        {
            int result = CompareTwoProperties(itemA.CreatedAt, itemB.CreatedAt);
            if(result == 0)
            {
                result = ManageDefaultOrder(itemA, itemB);
            }
            return result;
        }

        private int ManageTypeOrder(ItemViewModel itemA, ItemViewModel itemB)
        {
            int result = CompareTwoProperties(itemA.Type, itemB.Type, true);

            if (result == 0)
            {
                result = ManageDefaultOrder(itemA, itemB);
            }
            return result;
        }

        private int CompareTwoProperties<T>(T itemA, T itemB, bool inversedLogic = false, bool dependOfDescending = true) where T : IComparable
        {
            if (itemA == null && itemB == null)
            {
                return 0;   // equal
            }
            else if(itemA == null)
            {
                return 1;  // A is after B
            }
            else if(itemB == null)
            {
                return -1;   // A is before B
            }

            int result = 0;
            if (typeof(T) == typeof(string))
            {
                result = StringComparer.OrdinalIgnoreCase.Compare(itemA as string, itemB as string);
            }
            else
            {
                result = itemA.CompareTo(itemB);
            }
                

            if (dependOfDescending)
            {
                if (Descending)
                {
                    result = -result;
                    // descending
                }
                else
                {
                    
                    // ascending
                }
            }
            else
            {
                result = -result;
                // descending
            }

            if (inversedLogic)
            {
                result = -result;
            }

            return result;
        }

        public int Compare(object? x, object? y)
        {
            if (x is ItemViewModel itemA && y is ItemViewModel itemB)
            {
                return Compare(itemA, itemB);
            }
            else
            {
                return 0;
            }
        }
    }
}
