using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;

namespace Plugin.Badge.WPF
{
    /// <summary>
    /// Interaction logic for TabItemTemplate.xaml
    /// </summary>
    public partial class TabItemTemplate
    {
        private Page _tab;

        public TabItemTemplate()
        {
            InitializeComponent();

            this.DataContextChanged += OnDataContextChanged;
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Unloaded -= OnUnloaded;
            if (_tab != null)
            {
                _tab.PropertyChanged -= BindableObjectOnPropertyChanged;
            }
           
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            this.DataContextChanged -= OnDataContextChanged;
            if (this.DataContext is Page page)
            {
                _tab = page.GetPageWithBadge();
                _tab.PropertyChanged -= BindableObjectOnPropertyChanged;
            }
        }

        private void BindableObjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName.StartsWith("Badge"))
            {
                var dataContext = this.DataContext;
                this.DataContext = null;
                this.DataContext = dataContext;
            }
        }
    }
}
