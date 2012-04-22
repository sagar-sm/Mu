using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Windows.Storage.FileProperties;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;

using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;

namespace Mu_genotype1
{
    static class Lastfm
    {
        public static async void track_updateNowPlaying(MusicProperties id3)
        {
            //try
            //{
                string updtrack_sig = "album" + id3.Album + "api_key" + Globalv.lfm_api_key + "artist" + id3.Artist + "methodtrack.updateNowPlaying" + "sk" + Globalv.session_key + "track" + id3.Title + "0e6e780c3cfa3faedf0c58d5aa6de92f";

                HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm("MD5");
                CryptographicHash objHash = objAlgProv.CreateHash();
                IBuffer buffSig = CryptographicBuffer.ConvertStringToBinary(updtrack_sig, BinaryStringEncoding.Utf8);
                objHash.Append(buffSig);
                IBuffer buffSighash = objHash.GetValueAndReset();

                updtrack_sig = CryptographicBuffer.EncodeToHexString(buffSighash);

                HttpClient cli = new HttpClient();
                cli.DefaultRequestHeaders.ExpectContinue = false; //important
                string track_updateNowPlaying = @"method=track.updateNowPlaying&track=" + id3.Title + @"&artist=" + id3.Artist + @"&album=" + id3.Album + @"&api_key=" + Globalv.lfm_api_key + @"&api_sig=" + updtrack_sig + @"&sk=" + Globalv.session_key;

                cli.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpContent tunp = new StringContent(track_updateNowPlaying);

                tunp.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var upd_now_playing = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0", UriKind.Absolute), tunp);
            //}
           // catch (Exception) { };
        }


        public static async Task<string> track_scrobble(MusicProperties id3)
        {
            long epoch = Globalv.unix_timestamp();
            string scrb_track_sig = "album" + id3.Album + "api_key" + Globalv.lfm_api_key + "artist" + id3.Artist + "methodtrack.scrobble" + "sk" + Globalv.session_key + "timestamp" + epoch.ToString() + "track" + id3.Title + "0e6e780c3cfa3faedf0c58d5aa6de92f";
            
            //UTF8Encoding utf8e = new System.Text.UTF8Encoding();
            //scrb_track_sig = utf8e.GetString(utf8e.GetBytes(scrb_track_sig));

            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm("MD5");
            CryptographicHash objHash = objAlgProv.CreateHash();
            IBuffer buffSig = CryptographicBuffer.ConvertStringToBinary(scrb_track_sig, BinaryStringEncoding.Utf8);
            objHash.Append(buffSig);
            IBuffer buffSighash = objHash.GetValueAndReset();

            scrb_track_sig = CryptographicBuffer.EncodeToHexString(buffSighash);

            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.ExpectContinue = false; //important
            string track_scrobble = @"method=track.scrobble&timestamp=" + epoch.ToString() + "&track=" + id3.Title + @"&artist=" + id3.Artist + @"&album=" + id3.Album + @"&api_key=" + Globalv.lfm_api_key + @"&api_sig=" + scrb_track_sig + @"&sk=" + Globalv.session_key;

            cli.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            HttpContent tscr = new StringContent(track_scrobble);

            tscr.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var scrobbled_resp = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0", UriKind.Absolute), tscr);
            return await scrobbled_resp.Content.ReadAsStringAsync();
        }

        public static async Task<string> track_love(MusicProperties id3)
        {
            try
            {
                string scrb_track_sig = "api_key" + Globalv.lfm_api_key + "artist" + id3.Artist + "methodtrack.love" + "sk" + Globalv.session_key + "track" + id3.Title + "0e6e780c3cfa3faedf0c58d5aa6de92f";

                //UTF8Encoding utf8e = new System.Text.UTF8Encoding();
                //scrb_track_sig = utf8e.GetString(utf8e.GetBytes(scrb_track_sig));

                HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm("MD5");
                CryptographicHash objHash = objAlgProv.CreateHash();
                IBuffer buffSig = CryptographicBuffer.ConvertStringToBinary(scrb_track_sig, BinaryStringEncoding.Utf8);
                objHash.Append(buffSig);
                IBuffer buffSighash = objHash.GetValueAndReset();

                scrb_track_sig = CryptographicBuffer.EncodeToHexString(buffSighash);

                HttpClient cli = new HttpClient();
                cli.DefaultRequestHeaders.ExpectContinue = false; //important
                string track_love = @"method=track.love&track=" + id3.Title + @"&artist=" + id3.Artist + @"&api_key=" + Globalv.lfm_api_key + @"&api_sig=" + scrb_track_sig + @"&sk=" + Globalv.session_key;

                cli.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpContent tscr = new StringContent(track_love);

                tscr.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var loved_resp = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0", UriKind.Absolute), tscr);
                return await loved_resp.Content.ReadAsStringAsync();
            }
            catch (Exception) { return null; }
        }

        public static async Task<string> track_ban(MusicProperties id3)
        {
            try
            {
                string scrb_track_sig = "api_key" + Globalv.lfm_api_key + "artist" + id3.Artist + "methodtrack.ban" + "sk" + Globalv.session_key + "track" + id3.Title + "0e6e780c3cfa3faedf0c58d5aa6de92f";

                //UTF8Encoding utf8e = new System.Text.UTF8Encoding();
                //scrb_track_sig = utf8e.GetString(utf8e.GetBytes(scrb_track_sig));

                HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm("MD5");
                CryptographicHash objHash = objAlgProv.CreateHash();
                IBuffer buffSig = CryptographicBuffer.ConvertStringToBinary(scrb_track_sig, BinaryStringEncoding.Utf8);
                objHash.Append(buffSig);
                IBuffer buffSighash = objHash.GetValueAndReset();

                scrb_track_sig = CryptographicBuffer.EncodeToHexString(buffSighash);

                HttpClient cli = new HttpClient();
                cli.DefaultRequestHeaders.ExpectContinue = false; //important
                string track_ban = @"method=track.ban&track=" + id3.Title + @"&artist=" + id3.Artist + @"&api_key=" + Globalv.lfm_api_key + @"&api_sig=" + scrb_track_sig + @"&sk=" + Globalv.session_key;

                cli.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpContent tscr = new StringContent(track_ban);

                tscr.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var banned_resp = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0", UriKind.Absolute), tscr);
                return await banned_resp.Content.ReadAsStringAsync();
            }
            catch (Exception) { return null; }
        }

        public static async Task<string> track_getInfo(MusicProperties id3)
        {
            try
            {
                HttpClient cli = new HttpClient();
                string get_track_info = @"http://ws.audioscrobbler.com/2.0/?method=track.getinfo&api_key=" + Globalv.lfm_api_key + @"&artist=" + id3.Artist + @"&track=" + id3.Title + @"&autocorrect=1";
                HttpResponseMessage track_info = await cli.GetAsync(get_track_info);
                return await track_info.Content.ReadAsStringAsync();
            }
            catch (Exception) { return null; }
        }

        public static async Task<string> track_getInfo(Song id3)
        {
            try
            {
                HttpClient cli = new HttpClient();
                string get_track_info = @"http://ws.audioscrobbler.com/2.0/?method=track.getinfo&api_key=" + Globalv.lfm_api_key + @"&artist=" + id3.Artist + @"&track=" + id3.Title + @"&autocorrect=1";
                HttpResponseMessage track_info = await cli.GetAsync(get_track_info);
                return await track_info.Content.ReadAsStringAsync();
            }
            catch (Exception) { return null; }
        }


        public static async Task<string> user_getRecommendedArtists() 
        {
            try
            {
                HttpClient cli = new HttpClient();
                cli.MaxResponseContentBufferSize = 196608;
                string reco_sig = "api_key" + Globalv.lfm_api_key + "limit40methoduser.getRecommendedArtistssk" + Globalv.session_key + "0e6e780c3cfa3faedf0c58d5aa6de92f";

                HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm("MD5");
                CryptographicHash objHash = objAlgProv.CreateHash();
                IBuffer buffSig = CryptographicBuffer.ConvertStringToBinary(reco_sig, BinaryStringEncoding.Utf8);
                objHash.Append(buffSig);
                IBuffer buffSighash = objHash.GetValueAndReset();

                reco_sig = CryptographicBuffer.EncodeToHexString(buffSighash);
                string get_reco_artists = @"http://ws.audioscrobbler.com/2.0/?method=user.getRecommendedArtists&limit=40&api_key=" + Globalv.lfm_api_key + @"&api_sig=" + reco_sig +"&sk=" + Globalv.session_key;
                HttpResponseMessage recoart = await cli.GetAsync(get_reco_artists);
                return await recoart.Content.ReadAsStringAsync();
            }
            catch (Exception) { return null; }

        }

        public static async Task<string> artist_getInfo(string artist)
        {
            try
            {
                HttpClient cli = new HttpClient();
                string get_artist_info = @"http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&api_key=" + Globalv.lfm_api_key + @"&artist=" + artist + @"&autocorrect=1";
                HttpResponseMessage art_info = await cli.GetAsync(get_artist_info);
                return await art_info.Content.ReadAsStringAsync();
            }
            catch(Exception) { return null; }
        }
                        
        public static async Task<string> track_getSimilar(Song id3)
        {
            try
            {
                HttpClient cli = new HttpClient();
                //Playlist.NowPlaying.Add()
                string get_track_reco = @"http://ws.audioscrobbler.com/2.0/?method=track.getsimilar&artist=" + id3.Artist + @"&track=" + id3.Title + @"&limit=14&api_key=" + Globalv.lfm_api_key;
                HttpResponseMessage reco = await cli.GetAsync(get_track_reco);
                return await reco.Content.ReadAsStringAsync();
            }
            catch (Exception) { return null; }
        }

        public static async Task<string> chart_topTracks()
        {
            HttpClient cli = new HttpClient();
            string get_top_tracks = @"http://ws.audioscrobbler.com/2.0/?method=chart.gettoptracks&api_key=" + Globalv.lfm_api_key + @"&page=1&limit=20";
            HttpResponseMessage top_trc = await cli.GetAsync(get_top_tracks);
            return await top_trc.Content.ReadAsStringAsync();
        }
        public static async Task<string> geo_Metros()
        {
            HttpClient cli = new HttpClient();
            string get_metros = @"http://ws.audioscrobbler.com/2.0/?method=geo.getmetros&api_key=b25b959554ed76058ac220b7b2e0a026";
            HttpResponseMessage metros = await cli.GetAsync(get_metros);
            return await metros.Content.ReadAsStringAsync();
        }

        public static async Task<string> geo_topTrack(string country)
        {
            HttpClient cli = new HttpClient();
            string get_top_track_country = @"http://ws.audioscrobbler.com/2.0/?method=geo.gettoptracks&country=" + country + @"&api_key=" + Globalv.lfm_api_key + @"&limit=20";
            HttpResponseMessage top_trc_cntry = await cli.GetAsync(get_top_track_country);
            return await top_trc_cntry.Content.ReadAsStringAsync();
        }


        /*
        public static async void album_info()
        {
            HttpClient cli = new HttpClient();
            string get_album_info = @"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=" +  Globalv.lfm_api_key + @"&artist=" + id3.Artist + @"&album=" + id3.Album + @"&autocorrect=1";
            HttpResponseMessage alb_info = await cli.GetAsync(get_album_info);
        }


        public static async void artist_similar()
        {
            HttpClient cli = new HttpClient();
            string get_similar_artist = @"http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar&artist=" + id3.Artist + @"&api_key=" + Globalv.lfm_api_key +@"&autocorrect=1";
            HttpResponseMessage art_sim = await cli.GetAsync(get_similar_artist);
        }

        public static async void artist_topTracks()
        {
            HttpClient cli = new HttpClient();
            string get_art_top_tracks = @"http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist=" + id3.Artist + @"&api_key=" + Globalv.lfm_api_key + @"&limit=5&autocorrect=1";
            HttpResponseMessage art_top_trc = await cli.GetAsync(get_art_top_tracks); 
        }


        public static async void top_artist_country()
        {
            HttpClient cli = new HttpClient();
            string get_top_artist_country = @"http://ws.audioscrobbler.com/2.0/?method=geo.gettopartists&country=" + user.country + @"&api_key=" + Globalv.lfm_api_key + @"&limit=10";
            HttpResponseMessage top_art_cntry = await cli.GetAsync(get_top_artist_country);
        }

        public static async void top_track_country()
        {
            HttpClient cli = new HttpClient();
            string get_top_track_country = @"http://ws.audioscrobbler.com/2.0/?method=geo.gettoptracks&country=" + user.country + @"&api_key=" + Globalv.lfm_api_key + @"&limit=10";
            HttpResponseMessage top_trc_cntry = await cli.GetAsync(get_top_track_country);
        }

        public static async void tasteometer()
        {
            HttpClient cli = new HttpClient();
            string compare_taste_users = @"http://ws.audioscrobbler.com/2.0/?method=tasteometer.compare&type1=user&type2=user&value1=" + user1 + @"&value2=" + user2 + @"&api_key=" + Globalv.lfm_api_key;
            HttpResponseMessage user_taste = await cli.GetAsync(compare_taste_users);
        }


        public static async void similar_track() //actual recommendation function
        {
            HttpClient cli = new HttpClient();
            string get_similar_track = @"http://ws.audioscrobbler.com/2.0/?method=track.getsimilar&artist=" + id3.artist + @"&track=" + id3.title + @"&api_key=" + Globalv.lfm_api_key + @"&autocorrect=1&limit=5";
            HttpResponseMessage similar_track = await cli.GetAsync(get_similar_track);
        }

        public static async void user_info()
        {
            HttpClient cli = new HttpClient();
            string get_user_info = @"http://ws.audioscrobbler.com/2.0/?method=user.getinfo&user=" + user.name + @"&api_key=" + Globalv.lfm_api_key;
            HttpResponseMessage u_info = await cli.GetAsync(get_user_info);
        }

        public static async void user_recent_tracks()
        {
            HttpClient cli = new HttpClient();
            string recent_tracks = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=" + user.name + "&api_key=" + Globalv.lfm_api_key + @"&limit=10";
            HttpResponseMessage u_recent_track = await cli.GetAsync(recent_tracks);
        }

        public static async void user_top_artist()  //time = overall | 7day | 3month | 6month | 12month - The time period over which to retrieve top tracks for.
        {
            HttpClient cli = new HttpClient();
            string top_user_artist = @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=" + user.name + "&api_key=" + Globalv.lfm_api_key + @"&limit=5&period=" + time";
            HttpResponseMessage top_u_artist = await cli.GetAsync(top_user_artist);
        }

        public static async void user_top_track()  //time = overall | 7day | 3month | 6month | 12month - The time period over which to retrieve top tracks for.
        {
            HttpClient cli = new HttpClient();
            string top_user_track = @"http://ws.audioscrobbler.com/2.0/?method=user.gettoptracks&user=" + user.name + "&api_key=" + Globalv.lfm_api_key + @"&limit=5&period=" + time";
            HttpResponseMessage top_u_track = await cli.GetAsync(top_user_track);
        }

        public static async void query_library() //search for tracks with nowplaying artist in last.fm user library
        {
            HttpClient cli = new HttpClient();
            string search_library = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key=" + Globalv.lfm_api_key + @"&user=" + username + @"&artist=" + id3.artist";
            HttpResponseMessage in_library = await cli.GetAsync(search_library);
        }

        */

    }
}
