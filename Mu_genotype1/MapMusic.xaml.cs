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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Xml;
using System.Net.Http;
using Bing.Maps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Mu_genotype1
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MapMusic : Mu_genotype1.Common.LayoutAwarePage
    {
        public MapMusic()
        {
            this.InitializeComponent();
            myMap.Center = new Location(21.7679, 78.8718);
            myMap.ZoomLevel = 4;
            myMap.MapType = MapType.Birdseye;
            
        }

        private async void myMap_Loaded_1(object sender, RoutedEventArgs e)
        {
            progbar.Visibility = Visibility.Visible;
            //TODO: save response once in a file and avoid further http requests
            string resp = await Lastfm.geo_Metros();
            List<string> countries = new List<string>();
            using (XmlReader rd = XmlReader.Create(new StringReader(resp)))
            {
                try
                {
                    while (true)
                    {
                        //Metropolis m = new Metropolis();
                        rd.ReadToFollowing("metro");
                        rd.ReadToDescendant("name");
                        //m.name = rd.ReadElementContentAsString();
                        rd.ReadToNextSibling("country");
                        //m.country = rd.ReadElementContentAsString();
                        countries.Add(rd.ReadElementContentAsString());
                    }
                }
                catch(Exception) { }

                }
            IEnumerable<string> nodup = countries.Distinct();
            List<string> nodups = nodup.ToList();
            foreach (string m in nodups)
            {
                Metropolis c = new Metropolis();
                c.country = m;
                Globalv.AllMetros.Add(c);
            }
            HttpClient cli = new HttpClient();

            foreach (Metropolis c in Globalv.AllMetros)
            {
                try
                {
                    var geocoder = await cli.GetAsync(@"https://maps.googleapis.com/maps/api/geocode/json?address=" + c.country + "&sensor=false");
                    string geo_resp = await geocoder.Content.ReadAsStringAsync();
                    JObject jo = JObject.Parse(geo_resp);

                    Location l = new Location();
                    l.Latitude = (double)jo["results"][0]["geometry"]["location"]["lat"];
                    l.Longitude = (double)jo["results"][0]["geometry"]["location"]["lng"];
                    c.latlng = l;
                }
                catch (Exception) { }
                
            }

            //for adding push pins to countries
            foreach (Metropolis m in Globalv.AllMetros)
            {
                Pushpin pin = new Pushpin();
                pin.Text = m.country;
                MapLayer.SetPosition(pin, m.latlng);
                myMap.Children.Add(pin);
                pin.Tapped += pin_Tapped;
            }

            progbar.Visibility = Visibility.Collapsed;
        }

        async void pin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            progbar.Visibility = Visibility.Visible;
            Pushpin pin = (Pushpin)sender;

            string resp = await Lastfm.geo_topTrack(pin.Text);
            Globalv.CountryTrends.Clear();
            using (XmlReader rd = XmlReader.Create(new StringReader(resp)))
            {
                for (int i = 0; i < 12; i++)
                {
                    Song s2 = new Song();
                    rd.ReadToFollowing("name");
                    s2.Title = rd.ReadElementContentAsString();
                    rd.ReadToFollowing("artist");
                    rd.ReadToDescendant("name");
                    s2.Artist = rd.ReadElementContentAsString();
                    //s2.content = "Artist: " + s2.Artist + "\nTrack heard over " + pclist.ToString() + " times by " + listenerslist.ToString() + " listeners worldwide.";
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
                    Globalv.CountryTrends.Add(s2);
                }
                itemsGridView.ItemsSource = Globalv.CountryTrends;
                itemsGridView.UpdateLayout();

            }
            progbar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
