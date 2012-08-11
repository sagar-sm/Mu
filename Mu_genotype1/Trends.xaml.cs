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
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Xml;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace Mu_genotype1
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class Trends : Mu_genotype1.Common.LayoutAwarePage
    {
        public Trends()
        {
            this.InitializeComponent();
            TopTracks = new List<Song>();
        }
        public BitmapImage GroupImage { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public List<Song> TopTracks;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the group to be displayed.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Globalv.GlobalTopTracks.Count == 0)
            {
                progbar.Visibility = Visibility.Visible;
                bool success = false;
                try
                {
                    string resp = await Lastfm.chart_topTracks();

                    using (XmlReader rd = XmlReader.Create(new StringReader(resp)))
                    {
                        //for headliner
                        rd.ReadToFollowing("name");
                        GroupName = rd.ReadElementContentAsString();
                        rd.ReadToFollowing("playcount");
                        long pc = rd.ReadElementContentAsLong();
                        rd.ReadToFollowing("listeners");
                        long listeners = rd.ReadElementContentAsLong();
                        rd.ReadToFollowing("artist");
                        rd.ReadToDescendant("name");
                        string artist = rd.ReadElementContentAsString();
                        GroupDescription = "Artist: " + artist + "\nTrack heard over " + pc.ToString() + " times by " + listeners.ToString() + " listeners worldwide.";
                        Song s = new Song();
                        s.Artist = artist;
                        s.Title = GroupName;
                        string resp2 = await Lastfm.track_getInfo(s);
                        using (XmlReader rd2 = XmlReader.Create(new StringReader(resp2)))
                        {
                            rd2.ReadToFollowing("album");
                            rd2.ReadToDescendant("image");
                            rd2.ReadToNextSibling("image");
                            rd2.ReadToNextSibling("image");

                            GroupImage = new BitmapImage(new Uri(rd2.ReadElementContentAsString(), UriKind.Absolute));
                        }
                        GNameTb.Text = GroupName;
                        GImage.Source = GroupImage;
                        GDesc.Text = GroupDescription;

                        //for other items
                        for (int i = 0; i < 19; i++)
                        {
                            //for headliner
                            Song s2 = new Song();
                            rd.ReadToFollowing("name");
                            s2.Title = rd.ReadElementContentAsString();
                            rd.ReadToFollowing("playcount");
                            long pclist = rd.ReadElementContentAsLong();
                            rd.ReadToFollowing("listeners");
                            long listenerslist = rd.ReadElementContentAsLong();
                            rd.ReadToFollowing("artist");
                            rd.ReadToDescendant("name");
                            s2.Artist = rd.ReadElementContentAsString();
                            s2.content = "Artist: " + s2.Artist + "\nTrack heard over " + pclist.ToString() + " times by " + listenerslist.ToString() + " listeners worldwide.";
                            string resp22 = await Lastfm.track_getInfo(s2);

                            try
                            {
                                using (XmlReader rd2 = XmlReader.Create(new StringReader(resp22)))
                                {
                                    rd2.ReadToFollowing("album");
                                    rd2.ReadToFollowing("image");
                                    rd2.ReadToNextSibling("image");
                                    rd2.ReadToNextSibling("image");
                                    s2.image = new BitmapImage(new Uri(rd2.ReadElementContentAsString(), UriKind.Absolute));
                                }
                            }
                            catch (Exception) { }
                            TopTracks.Add(s2);
                        }
                        itemsGridView.ItemsSource = TopTracks;
                        Globalv.GlobalTopTracks = TopTracks;
                    }
                    success = true;
                }
                catch (Exception)
                { success = false; }

                if (!success)
                {
                    MessageDialog m = new MessageDialog("This feature requires you to be connected to the internet. Connect to the internet and try again", "You're offline");
                    await m.ShowAsync();
                }
                progbar.Visibility = Visibility.Collapsed;
            }
            else 
            {
                itemsGridView.ItemsSource = TopTracks;
            }
        }

        private void itemsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Frame.Navigate(typeof(TrendsDetails));
        }

        private void itemsGridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TrendsDetails));
        }
    }
}
