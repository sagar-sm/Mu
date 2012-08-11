using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using System.Security;

namespace Mu_genotype1
{
    static class Twitter
    {
        public async static Task<OAuthToken> Rcv_Request_token()
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>();
            string oauth_nonce = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString()));
            string utc_time = Globalv.unix_timestamp().ToString();
            sd.Add("oauth_version", "1.0");
            sd.Add("oauth_callback", "oob");
            sd.Add("oauth_consumer_key", Globalv.ConsumerKey);
            sd.Add("oauth_nonce", oauth_nonce);
            sd.Add("oauth_signature_method", "HMAC-SHA1");
            sd.Add("oauth_timestamp", utc_time);

            string baseString = String.Empty;
            baseString += "POST" + "&";
            baseString += Uri.EscapeDataString(
                "https://api.twitter.com/oauth/request_token")
                + "&";

            foreach (KeyValuePair<string, string> entry in sd)
            {
                baseString += Uri.EscapeDataString(entry.Key +
                    "=" + entry.Value + "&");
            }

            baseString =
                baseString.Substring(0, baseString.Length - 3);

            string consumerSecret = Globalv.ConsumerSecret;


            string signingKey = Uri.EscapeDataString(consumerSecret) + "&";

            // Create a MacAlgorithmProvider object for the specified algorithm.
            MacAlgorithmProvider objMacProv = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");

            // Demonstrate how to retrieve the name of the algorithm used.
            String strNameUsed = objMacProv.AlgorithmName;

            // Create a buffer that contains the the message to be signed.
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(baseString, encoding);

            // Create a key to be signed with the message.
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);
            CryptographicKey hmacKey = objMacProv.CreateKey(keyMaterial);

            // Sign the key and message together.
            IBuffer buffHMAC = CryptographicEngine.Sign(hmacKey, buffMsg);

            // Verify that the HMAC length is correct for the selected algorithm
            if (buffHMAC.Length != objMacProv.MacLength)
            {
                throw new Exception("Error computing digest");
            }

            string signatureString = CryptographicBuffer.EncodeToBase64String(buffHMAC);

            //prepare for sending
            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.ExpectContinue = false;
            cli.DefaultRequestHeaders.ConnectionClose = true;

            //auth headers
            string authorizationHeaderParams = String.Empty;
            //authorizationHeaderParams += "OAuth ";
            authorizationHeaderParams += "oauth_nonce=" + "\"" + Uri.EscapeDataString(oauth_nonce) + "\",";
            authorizationHeaderParams += "oauth_callback=" + "\"" + Uri.EscapeDataString("oob") + "\",";
            authorizationHeaderParams += "oauth_signature_method=" + "\"" + Uri.EscapeDataString("HMAC-SHA1") + "\",";
            authorizationHeaderParams += "oauth_timestamp=" + "\"" + Uri.EscapeDataString(utc_time) + "\",";
            authorizationHeaderParams += "oauth_consumer_key=" + "\"" + Uri.EscapeDataString(Globalv.ConsumerKey) + "\",";
            authorizationHeaderParams += "oauth_signature=" + "\"" + Uri.EscapeDataString(signatureString) + "\",";
            authorizationHeaderParams += "oauth_version=" + "\"" + Uri.EscapeDataString("1.0") + "\"";
            cli.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", authorizationHeaderParams);



            var MuTweetResponse = await cli.PostAsync(new Uri(@"https://api.twitter.com/oauth/request_token", UriKind.Absolute), null);
            string resp = await MuTweetResponse.Content.ReadAsStringAsync();

            string[] resp_split = resp.Split('&');
            string requesttoken = resp_split[0].Substring(12);
            string requesttokensecret = resp_split[1].Substring(19);

            OAuthToken ret = new OAuthToken();
            ret.Token = requesttoken;
            ret.TokenSecret = requesttokensecret;

            return ret;
        }

        public async static Task<OAuthToken> ExchangeRequestWithAccessToken(OAuthToken RequestToken, string pin)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>();
            string oauth_nonce = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString()));
            string utc_time = Globalv.unix_timestamp().ToString();
            sd.Add("oauth_version", "1.0");
            sd.Add("oauth_callback", "oob");
            sd.Add("oauth_consumer_key", Globalv.ConsumerKey);
            sd.Add("oauth_nonce", oauth_nonce);
            sd.Add("oauth_signature_method", "HMAC-SHA1");
            sd.Add("oauth_timestamp", utc_time);
            sd.Add("oauth_token", RequestToken.Token);
            sd.Add("oauth_verifier", pin);

            string baseString = String.Empty;
            baseString += "POST" + "&";
            baseString += Uri.EscapeDataString(
                "https://api.twitter.com/oauth/access_token ")
                + "&";

            foreach (KeyValuePair<string, string> entry in sd)
            {
                baseString += Uri.EscapeDataString(entry.Key +
                    "=" + entry.Value + "&");
            }

            baseString =
                baseString.Substring(0, baseString.Length - 3);

            string consumerSecret = Globalv.ConsumerSecret;
            string oauth_token_secret = RequestToken.TokenSecret;

            string signingKey = Uri.EscapeDataString(consumerSecret) + "&" + Uri.EscapeDataString(oauth_token_secret);

            // Create a MacAlgorithmProvider object for the specified algorithm.
            MacAlgorithmProvider objMacProv = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");

            // Demonstrate how to retrieve the name of the algorithm used.
            String strNameUsed = objMacProv.AlgorithmName;

            // Create a buffer that contains the the message to be signed.
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(baseString, encoding);

            // Create a key to be signed with the message.
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);
            CryptographicKey hmacKey = objMacProv.CreateKey(keyMaterial);

            // Sign the key and message together.
            IBuffer buffHMAC = CryptographicEngine.Sign(hmacKey, buffMsg);

            // Verify that the HMAC length is correct for the selected algorithm
            if (buffHMAC.Length != objMacProv.MacLength)
            {
                throw new Exception("Error computing digest");
            }

            string signatureString = CryptographicBuffer.EncodeToBase64String(buffHMAC);

            //prepare for sending
            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.ExpectContinue = false;
            cli.DefaultRequestHeaders.ConnectionClose = true;

            //auth headers
            string authorizationHeaderParams = String.Empty;
            //authorizationHeaderParams += "OAuth ";
            authorizationHeaderParams += "oauth_nonce=" + "\"" + Uri.EscapeDataString(oauth_nonce) + "\",";
            authorizationHeaderParams += "oauth_callback=" + "\"" + Uri.EscapeDataString("oob") + "\",";
            authorizationHeaderParams += "oauth_signature_method=" + "\"" + Uri.EscapeDataString("HMAC-SHA1") + "\",";
            authorizationHeaderParams += "oauth_timestamp=" + "\"" + Uri.EscapeDataString(utc_time) + "\",";
            authorizationHeaderParams += "oauth_consumer_key=" + "\"" + Uri.EscapeDataString(Globalv.ConsumerKey) + "\",";
            authorizationHeaderParams += "oauth_token=" + "\"" + Uri.EscapeDataString(RequestToken.Token) + "\",";
            authorizationHeaderParams += "oauth_signature=" + "\"" + Uri.EscapeDataString(signatureString) + "\",";
            authorizationHeaderParams += "oauth_verifier=" + "\"" + pin + "\",";
            authorizationHeaderParams += "oauth_version=" + "\"" + Uri.EscapeDataString("1.0") + "\"";
            cli.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", authorizationHeaderParams);

            var MuTweetResponse = await cli.PostAsync(new Uri(@"https://api.twitter.com/oauth/access_token ", UriKind.Absolute), null);
            string resp = await MuTweetResponse.Content.ReadAsStringAsync();

            string[] resp_split = resp.Split('&');
            string requesttoken = resp_split[0].Substring(12);
            string requesttokensecret = resp_split[1].Substring(19);

            OAuthToken ret = new OAuthToken();
            ret.Token = requesttoken;
            ret.TokenSecret = requesttokensecret;

            return ret;
        }

        public async static Task<string> Post_status(string status)
        {            
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>();
            string oauth_nonce = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString()));
            string utc_time = Globalv.unix_timestamp().ToString();
            sd.Add("status", Uri.EscapeDataString(status));
            sd.Add("include_entities", "true");
            sd.Add("oauth_version", "1.0");
            sd.Add("oauth_consumer_key", Globalv.ConsumerKey);
            sd.Add("oauth_nonce", oauth_nonce);
            sd.Add("oauth_signature_method", "HMAC-SHA1");
            sd.Add("oauth_timestamp", utc_time);
            sd.Add("oauth_token", Globalv.TwitterAccessToken.Token);

            string baseString = String.Empty;
            baseString += "POST" + "&";
            baseString += Uri.EscapeDataString(
                "http://api.twitter.com/1/statuses/update.json")
                + "&";

            foreach (KeyValuePair<string, string> entry in sd)
            {
                baseString += Uri.EscapeDataString(entry.Key +
                    "=" + entry.Value + "&");
            }

            baseString =
                baseString.Substring(0, baseString.Length - 3);

            //shh... secrets here!
            string consumerSecret = Globalv.ConsumerSecret;

            string oauth_token_secret = Globalv.TwitterAccessToken.TokenSecret;

            string signingKey = Uri.EscapeDataString(consumerSecret) + "&" + Uri.EscapeDataString(oauth_token_secret);

            // Create a MacAlgorithmProvider object for the specified algorithm.
            MacAlgorithmProvider objMacProv = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");

            // Demonstrate how to retrieve the name of the algorithm used.
            String strNameUsed = objMacProv.AlgorithmName;

            // Create a buffer that contains the the message to be signed.
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(baseString, encoding);

            // Create a key to be signed with the message.
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);
            CryptographicKey hmacKey = objMacProv.CreateKey(keyMaterial);

            // Sign the key and message together.
            IBuffer buffHMAC = CryptographicEngine.Sign(hmacKey, buffMsg);

            // Verify that the HMAC length is correct for the selected algorithm
            if (buffHMAC.Length != objMacProv.MacLength)
            {
                throw new Exception("Error computing digest");
            }

            string signatureString = CryptographicBuffer.EncodeToBase64String(buffHMAC);

            //prepare for sending
            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.ExpectContinue = false;
            cli.DefaultRequestHeaders.ConnectionClose = true;

            //auth headers
            string authorizationHeaderParams = String.Empty;
            //authorizationHeaderParams += "OAuth ";
            authorizationHeaderParams += "oauth_nonce=" + "\"" + Uri.EscapeDataString(oauth_nonce) + "\",";
            authorizationHeaderParams += "oauth_signature_method=" + "\"" + Uri.EscapeDataString("HMAC-SHA1") + "\",";
            authorizationHeaderParams += "oauth_timestamp=" + "\"" + Uri.EscapeDataString(utc_time) + "\",";
            authorizationHeaderParams += "oauth_consumer_key=" + "\"" + Uri.EscapeDataString(Globalv.ConsumerKey) + "\",";
            authorizationHeaderParams += "oauth_token=" + "\"" + Uri.EscapeDataString(Globalv.TwitterAccessToken.Token) + "\",";
            authorizationHeaderParams += "oauth_signature=" + "\"" + Uri.EscapeDataString(signatureString) + "\",";
            authorizationHeaderParams += "oauth_version=" + "\"" + Uri.EscapeDataString("1.0") + "\"";
            cli.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", authorizationHeaderParams);

            string postBody = "status=" + Uri.EscapeDataString(status) + "&include_entities=true";
            HttpContent tweet = new StringContent(postBody);
            tweet.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var MuTweetResponse = await cli.PostAsync(new Uri(@"http://api.twitter.com/1/statuses/update.json", UriKind.Absolute), tweet);
            return await MuTweetResponse.Content.ReadAsStringAsync();                                    
        }

        public async static Task<string> Get_user_timeline(string userid)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>();
            string oauth_nonce = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString()));
            string utc_time = Globalv.unix_timestamp().ToString();
            sd.Add("include_rts", "true");
            sd.Add("include_entities", "true");
            sd.Add("user_id", userid);
            sd.Add("count", "20");
            sd.Add("oauth_version", "1.0");
            sd.Add("oauth_consumer_key", Globalv.ConsumerKey);
            sd.Add("oauth_nonce", oauth_nonce);
            sd.Add("oauth_signature_method", "HMAC-SHA1");
            sd.Add("oauth_timestamp", utc_time);
            sd.Add("oauth_token", Globalv.TwitterAccessToken.Token);

            string baseString = String.Empty;
            baseString += "GET" + "&";
            baseString += Uri.EscapeDataString(
                "https://api.twitter.com/1/statuses/user_timeline.json?")
                + "&";

            foreach (KeyValuePair<string, string> entry in sd)
            {
                baseString += Uri.EscapeDataString(entry.Key +
                    "=" + entry.Value + "&");
            }

            baseString =
                baseString.Substring(0, baseString.Length - 3);

            //shh... secrets here!
            string consumerSecret = Globalv.ConsumerSecret;

            string oauth_token_secret = Globalv.TwitterAccessToken.TokenSecret;

            string signingKey = Uri.EscapeDataString(consumerSecret) + "&" + Uri.EscapeDataString(oauth_token_secret);

            // Create a MacAlgorithmProvider object for the specified algorithm.
            MacAlgorithmProvider objMacProv = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");

            // Demonstrate how to retrieve the name of the algorithm used.
            String strNameUsed = objMacProv.AlgorithmName;

            // Create a buffer that contains the the message to be signed.
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(baseString, encoding);

            // Create a key to be signed with the message.
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);
            CryptographicKey hmacKey = objMacProv.CreateKey(keyMaterial);

            // Sign the key and message together.
            IBuffer buffHMAC = CryptographicEngine.Sign(hmacKey, buffMsg);

            // Verify that the HMAC length is correct for the selected algorithm
            if (buffHMAC.Length != objMacProv.MacLength)
            {
                throw new Exception("Error computing digest");
            }

            string signatureString = CryptographicBuffer.EncodeToBase64String(buffHMAC);

            //prepare for sending
            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.ExpectContinue = false;
            cli.DefaultRequestHeaders.ConnectionClose = true;

            //auth headers
            string authorizationHeaderParams = String.Empty;
            //authorizationHeaderParams += "OAuth ";
            authorizationHeaderParams += "oauth_nonce=" + "\"" + Uri.EscapeDataString(oauth_nonce) + "\",";
            authorizationHeaderParams += "oauth_signature_method=" + "\"" + Uri.EscapeDataString("HMAC-SHA1") + "\",";
            authorizationHeaderParams += "oauth_timestamp=" + "\"" + Uri.EscapeDataString(utc_time) + "\",";
            authorizationHeaderParams += "oauth_consumer_key=" + "\"" + Uri.EscapeDataString(Globalv.ConsumerKey) + "\",";
            authorizationHeaderParams += "oauth_token=" + "\"" + Uri.EscapeDataString(Globalv.TwitterAccessToken.Token) + "\",";
            authorizationHeaderParams += "oauth_signature=" + "\"" + Uri.EscapeDataString(signatureString) + "\",";
            authorizationHeaderParams += "oauth_version=" + "\"" + Uri.EscapeDataString("1.0") + "\"";
            cli.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", authorizationHeaderParams);

            var resp = await cli.GetAsync(new Uri(@"https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&user_id="+userid+"&count=20", UriKind.Absolute));
            return await resp.Content.ReadAsStringAsync();
        }

        public async static Task<string> Get_tweets(string hashtag)
        {
            HttpClient cli = new HttpClient();
            string get_latest_tweets = @"http://search.twitter.com/search.json?q=" + Uri.EscapeDataString(hashtag) + @"&rpp=20&include_entities=true&lang=en&result_type=mixed";
            HttpResponseMessage get_tweets = await cli.GetAsync(get_latest_tweets);
            return await get_tweets.Content.ReadAsStringAsync();

        }
    }
}
