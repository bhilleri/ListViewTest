using CommunityToolkit.Mvvm.ComponentModel;

namespace ListViewManagedByViewModel.ViewModel
{
    public class ItemViewModel : ObservableObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        private ElementType type;

        public ElementType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        public ItemViewModel(string name, bool isSelected, string description, DateTime createdAt, ElementType type)
        {
            _name = name;
            _isSelected = isSelected;
            _description = description;
            _createdAt = createdAt;
            this.type = type;
        }

        public static ItemViewModel GenerateRandom(Random rand = null)
        {
            rand ??= new Random();

            string name = GenerateUniqueStringId();
            string description = $"Description {name}";
            DateTime createdAt = new DateTime(2025,rand.Next(1,12), rand.Next(1,27));
            ElementType type = (ElementType)rand.Next(0, 3);

            return new ItemViewModel(name, false, description, createdAt, type);
        }

        private static long GlobalId = 0;

        public static string GenerateUniqueStringId()
        {
            long value = Interlocked.Increment(ref GlobalId);
            byte[] bytes = BitConverter.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

    }
}
