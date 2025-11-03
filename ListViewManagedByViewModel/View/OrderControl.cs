using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListViewManagedByViewModel.View
{
    public class OrderControl : Control
    {
        static OrderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OrderControl), new FrameworkPropertyMetadata(typeof(OrderControl)));
        }

        public static DependencyProperty IsDescendingProperty = DependencyProperty.Register(
            nameof(IsDescending),
            typeof(bool),
            typeof(OrderControl),
            new PropertyMetadata(false)
        );

        public bool IsDescending
        {
            get { return (bool)GetValue(IsDescendingProperty); }
            set { SetValue(IsDescendingProperty, value); }
        }

        public static DependencyProperty IsActiveProperty = DependencyProperty.Register(
            nameof(IsActive),
            typeof(bool),
            typeof(OrderControl),
            new PropertyMetadata(false)
        );

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static DependencyProperty IsMouseOverComponentProperty = DependencyProperty.Register(
            nameof(IsMouseOverComponent),
            typeof(bool),
            typeof(OrderControl),
            new PropertyMetadata(false)
        );

        public bool IsMouseOverComponent
        {
            get { return (bool)GetValue(IsMouseOverComponentProperty); }
            set { SetValue(IsMouseOverComponentProperty, value); }
        }
    }
}
