using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Mu_genotype1
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string mbid { get; set; }
        public BitmapImage image { get; set; }
        public string content { get; set; }
    }
}
