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
using Windows.UI.Popups;
using Windows.Media;

using System.Xml;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Mu_genotype1
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class RecoArtists : Mu_genotype1.Common.LayoutAwarePage
    {
        public RecoArtists()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the collection of items to be displayed.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progbar.Visibility = Visibility.Visible;
            string resp = await Lastfm.user_getRecommendedArtists();
            Globalv.RecommendedArtists.Clear();
            bool success = false;
            try
            {
                using (XmlReader rd = XmlReader.Create(new StringReader(resp)))
                {
                    rd.ReadToFollowing("recommendations");
                    rd.MoveToAttribute("perPage");
                    int size = 34;

                    for (int i = 0; i < size; i++)
                    {
                        Artist ar = new Artist();
                        rd.ReadToFollowing("artist");
                        rd.ReadToDescendant("name");
                        ar.name = rd.ReadElementContentAsString();
                        rd.ReadToNextSibling("mbid");
                        ar.mbid = rd.ReadElementContentAsString();
                        rd.ReadToNextSibling("image");
                        rd.ReadToNextSibling("image");
                        rd.ReadToNextSibling("image");
                        ar.image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(rd.ReadElementContentAsString(), UriKind.Absolute));
                        Globalv.RecommendedArtists.Add(ar);
                    }
                    
                    List<Artist> dps = Globalv.RecommendedArtists.Distinct().ToList();
                    itemGridView.ItemsSource = dps;
                    success = true;
                }
            }
            catch (Exception) 
            {
                success = false;
            }
            if (!success)
            {
                MessageDialog m = new MessageDialog("There was some error in fetching content. Please try after sometime.", "Oops!");
                await m.ShowAsync();
            }

            progbar.Visibility = Visibility.Collapsed;
        }

        private void itemGridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtistDetails), e.ClickedItem);
        }


    }
}
