using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Twitterizer;

namespace Mu_genotype1
{
    static class Globalv
    {
        public static string session_key = null;
        public static string lfm_api_key = "4c4fdf4fe3bab8b93865409ec9e35df1";
        public static List<Artist> RecommendedArtists = new List<Artist>();
        public static List<Song> RecommendedTracks = new List<Song>();
        public static List<Song> GlobalTopTracks = new List<Song>();
        public static List<Song> CountryTrends = new List<Song>();
        public static List<Metropolis> AllMetros = new List<Metropolis>();
        public static OAuthTokenResponse TwitterAccessToken = new OAuthTokenResponse();
        public static string ConsumerKey = "jvookLg4Icyl4757z4swSA";
        public static string ConsumerSecret = "6yUT7YIeqSg3dmUJ3p39oeWNkZTEM4wAjExdlOcuC4";
        public static string TwitterUserId;

        public static long unix_timestamp()
        {
            TimeSpan unix_time = (System.DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)unix_time.TotalSeconds;
        }

    }
}
