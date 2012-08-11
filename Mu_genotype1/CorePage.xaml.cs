using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mu_genotype1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CorePage : Page
    {
        public CorePage()
        {
            this.InitializeComponent();
            App.GlobalAudioElement.VerticalAlignment = VerticalAlignment.Top;
            BaseGrid.Children.Add(App.GlobalAudioElement);
            BaseGrid.Children.Reverse();
        }
        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coreFrame.Navigate(typeof(BlankPage));
            //Window.Current.Content = coreFrame;
            //Window.Current.Activate();
        }
    }
}
