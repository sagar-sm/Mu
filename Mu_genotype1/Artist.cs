using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Mu_genotype1
{
    class Artist : IEquatable<Artist>
    {
        public string name { get; set; }
        public string mbid { get; set; }
        public BitmapImage image { get; set; }
        public string content { get; set; }

        public bool Equals(Artist right)
        {
            if ((object)this == null && (object)right == null)
            {
                return true;
            }
            if ((object)this == null || (object)right == null)
            {
                return false;
            }
            return this.name == right.name;
        }

        public override int GetHashCode()
        {
            int hashname = this.name == null ? 0 : this.name.GetHashCode();
            return hashname;
        }

    }
}
