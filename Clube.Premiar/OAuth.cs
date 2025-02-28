﻿// OAuth/OAuth.cs
//
// Code to do OAuth stuff, in support of a cropper plugin that sends
// a screen snap to TwitPic.com or the other that sends to Twitter.
//
// There's one main class: OAuth.Manager.  It handles interaction with the OAuth-
// enabled service, for requesting temporary tokens (aka request tokens), as well
// as access tokens. It also provides a convenient way to construct an oauth
// Authorization header for use in any Http transaction.
//
// The code has been tested with Twitter and TwitPic, from a desktop application.
//
// To build it standalone:
//
//     csc.exe /target:library /D:STANDALONE /doc:OAuth.XML OAuth.cs
//
// -------------------------------------------------------
// Dino Chiesa
// Tue, 14 Dec 2010  12:31
//
// -------------------------------------------------------
// Last saved: <2012-May-18 15:36:17>
//

using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace OAuth
{
    /// <summary>
    ///   A class to manage OAuth 1.0A interactions. This works with
    ///   Twitter; not sure about other OAuth-enabled services.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This class holds the relevant oauth parameters, as well as
    ///     state for the oauth authentication dance.  This class also
    ///     exposes methods that communicate with the OAuth provider, or
    ///     generate elaborated quantities (like Authorization header
    ///     strings) based on all the oauth properties.
    ///   </para>
    ///   <para>
    ///     OAuth 1.0A is ostensibly a standard, but as far as I am
    ///     aware, only Twitter implements the standard. Other services
    ///     implement *slightly* different oauth services.  This class
    ///     has been tested to work only with Twitter.
    ///   </para>
    ///   <para>
    ///     See <see href="http://dev.twitpic.com/docs/2/upload/">
    ///     http://dev.twitpic.com/docs/2/upload/</see>
    ///     for an example of the oauth parameters. The parameters include token,
    ///     client_id, timestamp, version, and so on. In the actual HTTP
    ///     message, they all include the oauth_ prefix, so ..  oauth_token,
    ///     oauth_timestamp, and so on. You set these via a string indexer.
    ///     If the instance of the class is called oauth, then to set the
    ///     oauth_token parameter, you use oath["token"] in C#.
    ///   </para>
    ///   <para>
    ///     This class automatically sets many of the required oauth parameters;
    ///     this includes the timestamp, nonce, callback, and version parameters.
    ///     (The callback param is initialized to 'oob'). You can reset any of
    ///     these parameters as you see fit.  In many cases you won't have to.
    ///   </para>
    ///   <para>
    ///     The public methods on the class include:
    ///     AcquireRequestToken, AcquireAccessToken,
    ///     GenerateCredsHeader, and GenerateAuthzHeader.  The
    ///     first two are used only on the first run of an applicaiton,
    ///     or after a user has explicitly de-authorized an application
    ///     for use with OAuth.  Normally, the GenerateXxxHeader methods
    ///     can be used repeatedly, when sending HTTP messages that
    ///     require an OAuth Authorization header.
    ///   </para>
    ///   <para>
    ///     The AcquireRequestToken and AcquireAccessToken methods
    ///     actually send out HTTP messages.
    ///   </para>
    ///   <para>
    ///     The GenerateXxxxHeaders are used when constructing and
    ///     sending your own HTTP messages.
    ///   </para>
    /// </remarks>
    public class Manager
    {
        /// <summary>
        ///   The default public constructor.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This constructor initializes the internal fields in the
        ///     Manager instance to default values.
        ///   </para>
        /// </remarks>
        public Manager()
        {
            _random = new Random();
            _params = new Dictionary<String, String>();
            _params["callback"] = "oob"; // presume "desktop" consumer
            _params["client_id"] = "";
            _params["client_secret"] = "";
            _params["timestamp"] = GenerateTimeStamp();
            _params["nonce"] = GenerateNonce();
            _params["signature_method"] = "HMAC-SHA1";
            _params["signature"] = "";
            _params["token"] = "";
            _params["token_secret"] = "";
            _params["version"] = "1.0";
        }

        /// <summary>
        ///   The constructor to use when using OAuth when you already
        ///   have an OAuth access token.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The parameters for this constructor all have the
        ///     meaning you would expect.  The token and tokenSecret
        ///     are set in oauth_token, and oauth_token_secret.
        ///     These are *Access* tokens, obtained after a call
        ///     to AcquireAccessToken.  The application can store
        ///     those tokens and re-use them on successive runs.
        ///     For twitter at least, the access tokens never expire.
        ///   </para>
        /// </remarks>
        ///
        /// <param name="clientId">The oauth_client_id parameter for
        /// OAuth. Get this, along with the consumer secret value, by manually
        /// registering your app with Twitter at
        /// <see href="http://twitter.com/apps/new">http://twitter.com/apps/new</see>
        /// </param>
        ///
        /// <param name="clientSecret">The oauth_client_secret
        /// parameter for oauth.</param>
        ///
        /// <param name="token">The oauth_token parameter for
        /// oauth. This is sometimes called the Access Token.</param>
        ///
        /// <param name="tokenSecret">The oauth_token_secret parameter for
        /// oauth. This is sometimes called the Access Token Secret.</param>
        public Manager(string clientId,
                       string clientSecret,
                       string token,
                       string tokenSecret) : this()
        {
            _params["client_id"] = clientId;
            _params["client_secret"] = clientSecret;
            _params["token"] = token;
            _params["token_secret"] = tokenSecret;
        }



        /// <summary>
        ///   The constructor to use when using OAuth when you already
        ///   have an OAuth consumer key and sercret, but need to
        ///   acquire an oauth access token.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The parameters for this constructor are the client_id
        ///     and client_secret that you get, manually, by
        ///     registering your application with Twitter.
        ///   </para>
        ///   <para>
        ///     What you need to do after instantiating the Manager
        ///     class with this constructor is set or obtain the access
        ///     key and token. See the examples provided elsewhere
        ///     for an illustration.
        ///   </para>
        /// </remarks>
        ///
        /// <param name="clientId">The oauth_client_id parameter for
        /// oauth. Get this, along with the clientSecret, by manually
        /// registering your app with Twitter at
        /// <see href="http://twitter.com/apps/new">http://twitter.com/apps/new</see>
        /// </param>
        ///
        /// <param name="clientSecret">The oauth_client_secret
        /// parameter for oauth.</param>
        ///
        public Manager(string clientId, string clientSecret) : this()
        {
            _params["client_id"] = clientId;
            _params["client_secret"] = clientSecret;
        }

        /// <summary>
        ///   The string indexer to get or set oauth parameter values.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Use the parameter name *without* the oauth_ prefix.  For
        ///     example, if you want to set the value for the
        ///     oauth_token parameter field in an HTTP message, then use
        ///     oauth["token"].
        ///   </para>
        ///   <para>
        ///     The set of oauth param names known by this indexer includes:
        ///     callback, client_id, client_secret, timestamp, nonce,
        ///     signature_method, signature, token, token_secret, and version.
        ///   </para>
        ///   <para>
        ///     If you try setting a parameter with a name that is not known,
        ///     the setter will throw.  You cannot "add" new oauth parameters
        ///     using the setter on this indexer.
        ///   </para>
        /// </remarks>
        ///
        /// <example>
        ///   This shows how to set the oauth_client_id and
        ///   oauth_client_secret using the indexer. Notice that the string
        ///   values lack the oauth_ prefix.
        ///   <code>
        ///   var oauth = new OAuth.Manager();
        ///   oauth["client_id"] = "~~~client_id~~~~";
        ///   oauth["client_secret"] = "~~~client_secret~~~";
        ///   oauth.AcquireRequestToken();
        ///   </code>
        /// </example>
        public string this[string ix]
        {
            get
            {
                if (_params.ContainsKey(ix))
                    return _params[ix];
                return null;
            }
            set
            {
                if (!_params.ContainsKey(ix))
                    _params.Add(ix, value);
                else
                    _params[ix] = value;
            }
        }


        /// <summary>
        /// Generate the timestamp for the signature.
        /// </summary>
        ///
        /// <returns>The timestamp, in string form.</returns>
        private string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - _epoch;
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        ///   Renews the nonce and timestamp on the oauth parameters.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Each new request should get a new, current timestamp, and a
        ///     nonce. This helper method does both of those things. This gets
        ///     called before generating an authorization header, as for example
        ///     when the user of this class calls <see cref='AcquireRequestToken()'/>.
        ///   </para>
        /// </remarks>
        private void NewRequest()
        {
            _params["nonce"] = GenerateNonce();
            _params["timestamp"] = GenerateTimeStamp();
        }

        /// <summary>
        /// Generate an oauth nonce.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     According to <see
        ///     href="http://tools.ietf.org/html/rfc5849">RFC 5849</see>, A
        ///     nonce is a random string, uniquely generated by the client to
        ///     allow the server to verify that a request has never been made
        ///     before and helps prevent replay attacks when requests are made
        ///     over a non-secure channel.  The nonce value MUST be unique
        ///     across all requests with the same timestamp, client
        ///     credentials, and token combinations.
        ///   </para>
        ///   <para>
        ///     One way to implement the nonce is just to use a
        ///     monotonically-increasing integer value.  It starts at zero and
        ///     increases by 1 for each new request or signature generated.
        ///     Keep in mind the nonce needs to be unique only for a given
        ///     timestamp!  So if your app makes less than one request per
        ///     second, then using a static nonce of "0" will work.
        ///   </para>
        ///   <para>
        ///     Most oauth nonce generation routines are waaaaay over-engineered,
        ///     and this one is no exception.
        ///   </para>
        /// </remarks>
        /// <returns>the nonce</returns>
        private string GenerateNonce()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                int g = _random.Next(3);
                switch (g)
                {
                    case 0:
                        // lowercase alpha
                        sb.Append((char)(_random.Next(26) + 97), 1);
                        break;
                    default:
                        // numeric digits
                        sb.Append((char)(_random.Next(10) + 48), 1);
                        break;
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// Internal function to extract from a URL all query string
        /// parameters that are not related to oauth - in other words all
        /// parameters not begining with "oauth_".
        /// </summary>
        ///
        /// <remarks>
        ///   <para>
        ///     For example, given a url like http://foo?a=7&amp;guff, the
        ///     returned value will be a Dictionary of string-to-string
        ///     relations.  There will be 2 entries in the Dictionary: "a"=>7,
        ///     and "guff"=>"".
        ///   </para>
        /// </remarks>
        ///
        /// <param name="queryString">The query string part of the Url</param>
        ///
        /// <returns>A Dictionary containing the set of
        /// parameter names and associated values</returns>
        private Dictionary<String, String> ExtractQueryParameters(string queryString)
        {
            if (queryString.StartsWith("?"))
                queryString = queryString.Remove(0, 1);

            var result = new Dictionary<String, String>();

            if (string.IsNullOrEmpty(queryString))
                return result;

            foreach (string s in queryString.Split('&'))
            {
                if (!string.IsNullOrEmpty(s) && !s.StartsWith("oauth_"))
                {
                    if (s.IndexOf('=') > -1)
                    {
                        string[] temp = s.Split('=');
                        result.Add(temp[0], temp[1]);
                    }
                    else
                        result.Add(s, string.Empty);
                }
            }

            return result;
        }



        /// <summary>
        ///   This method performs oauth-compliant Url Encoding.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This class provides an OAuth-friendly URL encoder.  .NET includes
        ///     a Url encoder in the base class library; see <see
        ///     href='http://msdn.microsoft.com/en-us/library/zttxte6w(v=VS.90).aspx'>
        ///     HttpServerUtility.UrlEncode</see>. But that encoder is not
        ///     sufficient for use with OAuth.
        ///   </para>
        ///   <para>
        ///     The builtin encoder emits the percent encoding in lower case,
        ///     which works for HTTP purposes, as described in the latest HTTP
        ///     specification (see <see
        ///     href="http://tools.ietf.org/html/rfc3986">RFC 3986</see>). But the
        ///     Oauth specification, provided in <see
        ///     href="http://tools.ietf.org/html/rfc5849">RFC 5849</see>, requires
        ///     that the encoding characters be upper case throughout OAuth.
        ///   </para>
        ///   <para>
        ///     For example, if you try to post a tweet message that includes a
        ///     forward slash, the slash needs to be encoded as %2F, and the
        ///     second hex digit needs to be uppercase.
        ///   </para>
        ///   <para>
        ///     It's not enough to simply convert the entire message to uppercase,
        ///     because that would of course convert un-encoded characters to
        ///     uppercase as well, which is undesirable.  This class provides an
        ///     OAuth-friendly encoder to do the right thing.
        ///   </para>
        /// </remarks>
        ///
        /// <param name="value">The value to encode</param>
        ///
        /// <example>
        /// <code>
        ///   var twitterUpdateUrlBase = "http://api.twitter.com/1/statuses/update.xml?status=";
        ///   var url = twitterUpdateUrlBase + OAuth.Manager.UrlEncode(message);
        ///   var authzHeader = oauth.GenerateAuthzHeader(url, "POST");
        ///   var request = (HttpWebRequest)WebRequest.Create(url);
        ///   request.Method = "POST";
        ///   request.PreAuthenticate = true;
        ///   request.AllowWriteStreamBuffering = true;
        ///   request.Headers.Add("Authorization", authzHeader);
        ///   using (var response = (HttpWebResponse)request.GetResponse())
        ///   {
        ///     ...
        ///   }
        /// </code>
        /// </example>
        /// <returns>the Url-encoded version of that string</returns>
        public static string UrlEncode(string value)
        {
            var result = new StringBuilder();
            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                    result.Append(symbol);
                else
                {
                    foreach (byte b in Encoding.UTF8.GetBytes(symbol.ToString()))
                    {
                        result.Append('%' + String.Format("{0:X2}", b));
                    }
                }
            }
            return result.ToString();
        }

        private static string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";


        /// <summary>
        /// Formats the list of request parameters into string a according
        /// to the requirements of oauth. The resulting string could be used
        /// in the Authorization header of the request.
        /// </summary>
        ///
        /// <remarks>
        ///   <para>
        ///     See <see
        ///     href="http://dev.twitter.com/pages/auth#intro">Twitter's OAUth
        ///     documentation page</see> for some background. The output of
        ///     this call is not suitable for signing.
        ///   </para>
        ///   <para>
        ///     There are 2 formats for specifying the list of oauth
        ///     parameters in the oauth spec: one suitable for signing, and
        ///     the other suitable for use within Authorization HTTP Headers.
        ///     This method emits a string suitable for the latter.
        ///   </para>
        /// </remarks>
        ///
        /// <param name="p">The Dictionary of parameters. It need not be
        /// sorted. Actually, strictly speaking, it need not be a
        /// dictionary, either. Just a collection of KeyValuePair.</param>
        ///
        /// <returns>a string representing the parameters</returns>
        private static string EncodeRequestParameters(ICollection<KeyValuePair<String, String>> p)
        {
            var sb = new System.Text.StringBuilder();
            foreach (KeyValuePair<String, String> item in p.OrderBy(x => x.Key))
            {
                if (!String.IsNullOrEmpty(item.Value) &&
                    !item.Key.EndsWith("secret"))
                    sb.AppendFormat("oauth_{0}=\"{1}\", ",
                                    item.Key,
                                    UrlEncode(item.Value));
            }

            return sb.ToString().TrimEnd(' ').TrimEnd(',');
        }

        private string EncodeAuthorizationParameters()
        {
            if (string.IsNullOrEmpty(this._params["client_id"]))
                throw new ArgumentNullException("client_id");

            if (string.IsNullOrEmpty(this._params["client_secret"]))
                throw new ArgumentNullException("client_secret");

            var client_id = _params["client_id"];
            var client_secret = _params["client_secret"];
            return Base64Encode($"{client_id}:{client_secret}");
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }



        /// <summary>
        ///   Acquire a request token from the given URI using the given
        ///   HTTP method.
        /// </summary>
        ///
        /// <remarks>
        ///   <para>
        ///     To use this method, first instantiate a new Oauth.Manager
        ///     object, then, optionally, set the callback param
        ///     (oauth["callback"]='oob'). Consult the Twitter documentation
        ///     for the meaning and usage of the callback parameter. After the
        ///     call returns, you should direct the user to open a browser
        ///     window to the authorization page for the OAuth-enabled
        ///     service. Or, you can automatically open that page yourself. Do
        ///     this with System.Diagnostics.Process.Start(), passing the URL
        ///     of the page.  If you're using Twitter, there should be one
        ///     query param: oauth_token with the value obtained from
        ///     oauth["token"]. See the example
        ///   </para>
        ///   <para>
        ///     According to the OAuth spec, you need to do this only ONCE per
        ///     application, the first time the application is run.  The
        ///     normal oauth workflow is: (1) get a request token, (2) use
        ///     that to acquire an access token (which requires explicit user
        ///     approval), then (3) using that access token, invoke protected
        ///     services. The first two steps need to be done only once, ever,
        ///     for each registered application. The third step can be
        ///     performed many times, over many invocations of the
        ///     application.
        ///   </para>
        ///   <para>
        ///     For Twitter, at least, you can cache the access tokens
        ///     indefinitely; Twitter says they never expire.  However, other
        ///     oauth services may not do the same. Also: the user may at any
        ///     time revoke his authorization for your app, in which case you
        ///     need to perform the first 2 steps again.
        ///   </para>
        /// </remarks>
        ///
        /// <seealso cref='AcquireAccessToken(string,string,string)'/>
        /// <seealso cref='AcquireAccessToken(string)'/>
        ///
        /// <example>
        ///   <para>
        ///     This example shows how to request an access token and key
        ///     from Twitter. It presumes you've already obtained a
        ///     consumer key and secret via app registration. Requesting
        ///     an access token is necessary only the first time you
        ///     contact the service. You can cache the access key and
        ///     token for subsequent runs, later.
        ///   </para>
        ///   <code>
        ///   // the URL to obtain a temporary "request token"
        ///   var rtUrl = "https://api.twitter.com/oauth/request_token";
        ///   var oauth = new OAuth.Manager();
        ///   // The consumer_{key,secret} are obtained via registration
        ///   oauth["client_id"] = "~~~client_id~~~~";
        ///   oauth["client_secret"] = "~~~client_secret~~~";
        ///   oauth.AcquireRequestToken(rtUrl, "POST");
        ///   // The request token is now available in oauth["token"]
        ///   // The next step is to pop the authorization approval dialog.
        ///   var authzUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth["token"];
        ///   System.Diagnostics.Process.Start(authzUrl);
        ///   </code>
        /// </example>
        ///
        /// <param name="uri">The uri to the "request token" endpoint of
        /// the service that implements oauth.  For Twitter, this is
        /// "https://api.twitter.com/oauth/request_token".
        /// </param>
        ///
        /// <param name="method">The method you will use to send the
        /// message asking for a request token.  For Twitter, this
        /// should be "POST".
        /// </param>
        ///
        /// <returns>
        ///   a response object that contains the entire text of the response,
        ///   as well as extracted parameters. This method presumes the
        ///   response is query-param encoded. In other words,
        ///   poauth_token=foo&amp;something_else=bar.
        /// </returns>
        public OAuthResponse AcquireRequestToken(string uri, string method)
        {
            //NewRequest();
            var authzHeader = GetAuthorizationHeader(uri, method);

            // prepare the token request
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            request.Headers.Add("Authorization", authzHeader);
            request.Headers.Add("Ocp-Apim-Subscription-Key", _params["subscription-key"]);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = method;

            var campaign_id = Uri.EscapeDataString(_params["campaign_id"]);
            var username = Uri.EscapeDataString(_params["username"]);
            var password = Uri.EscapeDataString(_params["password"]);
            var postData = $"grant_type=password&username={username}&password={password}&campaign_id={campaign_id}";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            using (var requestBody = request.GetRequestStream())
            {
                requestBody.Write(byteArray, 0, byteArray.Length);
            }

            using (var response = (System.Net.HttpWebResponse)request.GetResponse())
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var s = reader.ReadToEnd();
                    var r = new OAuthResponse(s);
                    this["access_token"] = r["access_token"];
                    this["token_type"] = r["token_type"];
                    this["expires_in"] = r["expires_in"];
                    return r;
                }
            }
        }
        
        /// <summary>
        ///   Acquire an access token, from the given URI, using the given
        ///   HTTP method.
        /// </summary>
        ///
        /// <remarks>
        ///   <para>
        ///     To use this method, you must first set the oauth_token to the value
        ///     of the request token.  Eg, oauth["token"] = "whatever".
        ///   </para>
        ///   <para>
        ///     According to the OAuth spec, you need to do this only ONCE per
        ///     application.  In other words, the first time the application
        ///     is run.  The normal oauth workflow is:  (1) get a request token,
        ///     (2) use that to acquire an access token (which requires explicit
        ///     user approval), then (3) using that access token, invoke
        ///     protected services.  The first two steps need to be done only
        ///     once per application.
        ///   </para>
        ///   <para>
        ///     For Twitter, at least, you can cache the access tokens
        ///     indefinitely; Twitter says they never expire.  However, other
        ///     oauth services may not do the same. Also: the user may at any
        ///     time revoke his authorization for your app, in which case you
        ///     need to perform the first 2 steps again.
        ///   </para>
        /// </remarks>
        ///
        /// <seealso cref='AcquireRequestToken()'/>
        ///
        /// <example>
        ///   <para>
        ///     This example shows how to request an access token and key
        ///     from Twitter. It presumes you've already obtained a
        ///     consumer key and secret via app registration. Requesting
        ///     an access token is necessary only the first time you
        ///     contact the service. You can cache the access key and
        ///     token for subsequent runs, later.
        ///   </para>
        ///   <code>
        ///   // the URL to obtain a temporary "request token"
        ///   var rtUrl = "https://api.twitter.com/oauth/request_token";
        ///   var oauth = new OAuth.Manager();
        ///   // The consumer_{key,secret} are obtained via registration
        ///   oauth["client_id"] = "~~~client_id~~~~";
        ///   oauth["client_secret"] = "~~~client_secret~~~";
        ///   oauth.AcquireRequestToken(rtUrl, "POST");
        ///   var authzUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth["token"];
        ///   // Normally you'd want to pop this in a WebBrowser control.
        ///   // For the example only, we'll pop the URL in a separate browser.
        ///   System.Diagnostics.Process.Start(authzUrl);
        ///   // instruct the user to type in the PIN from that browser window
        ///   var pin = "...";
        ///   var atUrl = "https://api.twitter.com/oauth/access_token";
        ///   oauth.AcquireAccessToken(atUrl, "POST", pin);
        ///
        ///   // now, update twitter status using that access token
        ///   var appUrl = "http://api.twitter.com/1/statuses/update.xml?status=Hello";
        ///   var authzHeader = oauth.GenerateAuthzHeader(appUrl, "POST");
        ///   var request = (HttpWebRequest)WebRequest.Create(appUrl);
        ///   request.Method = "POST";
        ///   request.PreAuthenticate = true;
        ///   request.AllowWriteStreamBuffering = true;
        ///   request.Headers.Add("Authorization", authzHeader);
        ///
        ///   using (var response = (HttpWebResponse)request.GetResponse())
        ///   {
        ///     if (response.StatusCode != HttpStatusCode.OK)
        ///       MessageBox.Show("There's been a problem trying to tweet:" +
        ///                       Environment.NewLine +
        ///                       response.StatusDescription);
        ///   }
        ///   </code>
        /// </example>
        ///
        /// <param name="uri">The uri to the "access token" endpoint of
        /// the service that implements oauth.  For Twitter, this is
        /// "https://api.twitter.com/oauth/access_token".
        /// </param>
        ///
        /// <param name="method">The method you will use to send the
        /// message asking for an access token.  For Twitter, this
        /// should be "POST".
        /// </param>
        ///
        /// <param name="pin">The PIN returned by the "Application approval" page
        /// shown by Twitter.  It's a string of numeric digits, 7 or so digits in
        /// length.
        /// </param>
        ///
        /// <returns>
        ///   a response object that contains the entire text of the response,
        ///   as well as extracted parameters. This method presumes the
        ///   response is query-param encoded. In other words,
        ///   oauth_token=foo&amp;something_else=bar.
        /// </returns>
        public OAuthResponse AcquireAccessToken(string uri, string method, string pin = null)
        {
            //Tracing.Trace("AcquireAccessToken.");
            //NewRequest();
            //_params.Remove("callback"); // no longer needed
            //_params["verifier"] = pin;

            var authzHeader = GetAuthorizationHeader(uri, method);

            try
            {
                // prepare the token request
                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
                request.Headers.Add("Authorization", authzHeader);
                request.Headers.Add("Ocp-Apim-Subscription-Key", _params["subscription-key"]);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = method;
                
                var campaign_id = Uri.EscapeDataString(_params["campaign_id"]);
                var postData = $"grant_type=client_credentials&campaign_id={campaign_id}";

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                using (var requestBody = request.GetRequestStream())
                {
                    requestBody.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        var s = reader.ReadToEnd();
                        //Tracing.Trace("resp: {0}", s);
                        var r = new OAuthResponse(s);
                        this["access_token"] = r["access_token"];
                        this["token_type"] = r["token_type"];
                        this["expires_in"] = r["expires_in"];
                        //_params.Remove("verifier"); // not needed any longer
                        return r;
                    }
                }
            }
            catch (Exception ex1)
            {
                //Tracing.Trace("Exception: {0}", ex1.ToString());
                //_params.Remove("verifier"); // not needed any longer
                return null;
            }
        }
        
        /// <summary>
        ///   Generate a string to be used in an Authorization header in
        ///   an HTTP request.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This method assembles the available oauth_ parameters that
        ///     have been set in the Dictionary in this instance, produces
        ///     the signature base (As described by the OAuth spec, RFC 5849),
        ///     signs it, then re-formats the oauth_ parameters into the
        ///     appropriate form, including the oauth_signature value, and
        ///     returns the result.
        ///   </para>
        ///   <para>
        ///     If you pass in a non-null, non-empty realm, this method will
        ///     include the realm='foo' clause in the generated Authorization header.
        ///   </para>
        /// </remarks>
        ///
        /// <seealso cref='GenerateAuthzHeader'/>
        ///
        /// <param name="uri">The "verify credentials" endpoint for the
        /// service to communicate with, via an OAuth-authenticated
        /// message. For Twitpic (authenticated through Twitter), this is
        /// "https://api.twitter.com/1/account/verify_credentials.json".
        /// </param>
        ///
        /// <param name="method">The HTTP method to use to request the
        /// credentials verification.  For Twitpic (authenticated
        /// through Twitter), this is "GET".</param>
        ///
        /// <param name="realm">The "Realm" to use to verify
        /// credentials. For Twitpic (authenticated through Twitter),
        /// this is "http://api.twitter.com/".</param>
        ///
        /// <returns>The OAuth authorization header parameter that has been
        /// generated given all the oauth input parameters.</returns>
        public string GenerateCredsHeader(string uri, string method, string realm)
        {
            NewRequest();
            var authzHeader = GetAuthorizationHeader(uri, method, realm);
            return authzHeader;
        }


        /// <summary>
        ///   Generate a string to be used in an Authorization header in
        ///   an HTTP request.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This method assembles the available oauth_ parameters that
        ///     have been set in the Dictionary in this instance, produces
        ///     the signature base (As described by the OAuth spec, RFC 5849),
        ///     signs it, then re-formats the oauth_ parameters into the
        ///     appropriate form, including the oauth_signature value, and
        ///     returns the result.
        ///   </para>
        /// </remarks>
        ///
        /// <example>
        ///   <para>
        ///     This example shows how to update the Twitter status
        ///     using the stored consumer key and secret, and a previously
        ///     obtained access token and secret.
        ///   </para>
        ///   <code>
        ///   var oauth = new OAuth.Manager();
        ///   oauth["client_id"]    = "~~ your stored consumer key ~~";
        ///   oauth["client_secret"] = "~~ your stored consumer secret ~~";
        ///   oauth["token"]           = "~~ your stored access token ~~";
        ///   oauth["token_secret"]    = "~~ your stored access secret ~~";
        ///   var appUrl = "http://api.twitter.com/1/statuses/update.xml?status=Hello";
        ///   var authzHeader = oauth.GenerateAuthzHeader(appUrl, "POST");
        ///   var request = (HttpWebRequest)WebRequest.Create(appUrl);
        ///   request.Method = "POST";
        ///   request.PreAuthenticate = true;
        ///   request.AllowWriteStreamBuffering = true;
        ///   request.Headers.Add("Authorization", authzHeader);
        ///
        ///   using (var response = (HttpWebResponse)request.GetResponse())
        ///   {
        ///     if (response.StatusCode != HttpStatusCode.OK)
        ///       MessageBox.Show("There's been a problem trying to tweet:" +
        ///                       Environment.NewLine +
        ///                       response.StatusDescription);
        ///   }
        ///   </code>
        /// </example>
        ///
        /// <seealso cref='GenerateCredsHeader'/>
        ///
        /// <param name="uri">The target URI that the application will connet
        /// to, via an OAuth-protected protocol. </param>
        ///
        /// <param name="method">The HTTP method that will be used to connect
        /// to the target URI. </param>
        ///
        /// <returns>The OAuth authorization header that has been generated
        /// given all the oauth input parameters.</returns>
        public string GenerateAuthzHeader(string uri, string method)
        {
            NewRequest();
            var authzHeader = GetAuthorizationHeader(uri, method, null);
            return authzHeader;
        }

        private string GetAuthorizationHeader(string uri, string method)
        {
            return GetAuthorizationHeader(uri, method, null);
        }

        private string GetAuthorizationHeader(string uri, string method, string realm)
        {
            //Tracing.Trace("GetAuthorizationHeader.");

            if (string.IsNullOrEmpty(this._params["client_id"]))
                throw new ArgumentNullException("client_id");

            //if (string.IsNullOrEmpty(this._params["signature_method"]))
            //    throw new ArgumentNullException("signature_method");

            //Sign(uri, method);

            //var erp = EncodeRequestParameters(this._params);
            var erp = EncodeAuthorizationParameters();
            return (String.IsNullOrEmpty(realm))
                //? "OAuth " + erp
                ? "Basic " + erp
                : String.Format("OAuth realm=\"{0}\", ", realm) + erp;
        }


        private void Sign(string uri, string method)
        {
            //Tracing.Trace("Sign.");
            var signatureBase = GetSignatureBase(uri, method);
            var hash = GetHash();

            byte[] dataBuffer = Encoding.ASCII.GetBytes(signatureBase);
            byte[] hashBytes = hash.ComputeHash(dataBuffer);
            var sig = Convert.ToBase64String(hashBytes);
            //Tracing.Trace("Signatiure: {0}", sig);
            this["signature"] = sig;
        }

        /// <summary>
        /// Formats the list of request parameters into "signature base" string as
        /// defined by RFC 5849.  This will then be MAC'd with a suitable hash.
        /// </summary>
        private string GetSignatureBase(string url, string method)
        {
            // normalize the URI
            var uri = new Uri(url);
            var normUrl = string.Format("{0}://{1}", uri.Scheme, uri.Host);
            if (!((uri.Scheme == "http" && uri.Port == 80) ||
                  (uri.Scheme == "https" && uri.Port == 443)))
                normUrl += ":" + uri.Port;

            normUrl += uri.AbsolutePath;

            // the sigbase starts with the method and the encoded URI
            var sb = new System.Text.StringBuilder();
            sb.Append(method)
                .Append('&')
                .Append(UrlEncode(normUrl))
                .Append('&');

            //Tracing.Trace("Sigbase start: {0}", sb.ToString());

            // The parameters follow. This must include all oauth params
            // plus any query params on the uri.  Also, each uri may
            // have a distinct set of query params.

            // first, get the query params
            var p = ExtractQueryParameters(uri.Query);

            // add to that list all non-empty oauth params
            foreach (var p1 in this._params)
            {
                // Exclude all oauth params that are secret or
                // signatures; any secrets must not be shared,
                // and any existing signature will be invalid.

                if (!String.IsNullOrEmpty(this._params[p1.Key]) &&
                    !p1.Key.EndsWith("_secret") &&
                    !p1.Key.EndsWith("signature"))
                {
                    // workitem 15756 - handle non-oob scenarios
                    p.Add("oauth_" + p1.Key,
                          (p1.Key == "callback") ? UrlEncode(p1.Value) : p1.Value);
                }
            }

            // concat+format the sorted list of all those params
            var sb1 = new System.Text.StringBuilder();
            foreach (KeyValuePair<String, String> item in p.OrderBy(x => x.Key))
            {
                // even "empty" params need to be encoded this way.
                sb1.AppendFormat("{0}={1}&", item.Key, item.Value);
            }

            //Tracing.Trace("oauth piece: {0}", sb1.ToString());

            // append the UrlEncoded version of that string to the sigbase
            sb.Append(UrlEncode(sb1.ToString().TrimEnd('&')));
            var result = sb.ToString();
            //Tracing.Trace("Sigbase final: {0}", result);

            return result;
        }


        private HashAlgorithm GetHash()
        {
            if (this["signature_method"] != "HMAC-SHA1")
                throw new NotImplementedException();

            var keystring = string.Format("{0}&{1}",
                                   UrlEncode(this["client_secret"]),
                                          UrlEncode(this["token_secret"]));
            //Tracing.Trace("signing keystring: {0}", keystring);
            return new HMACSHA1
            {
                Key = Encoding.ASCII.GetBytes(keystring)
            };
        }
        
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private Dictionary<String, String> _params;
        private Random _random;
    }

    /// <summary>
    ///   A class to hold an OAuth response message.
    /// </summary>
    public class OAuthResponse
    {
        /// <summary>
        ///   All of the text in the response. This is useful if the app wants
        ///   to do its own parsing.
        /// </summary>
        public string AllText { get; set; }
        private Dictionary<String, String> _params;

        /// <summary>
        ///   a Dictionary of response parameters.
        /// </summary>
        public string this[string ix]
        {
            get
            {
                return _params[ix];
            }
        }

        /// <summary>
        ///   Constructor for the response to one transmission in an oauth dialogue.
        ///   An application or may not not want direct access to this response.
        /// </summary>
        internal OAuthResponse(string alltext)
        {
            AllText = alltext;
            ////Tracing.Trace("OAuthResponse.ctor: {0}", alltext);
            _params = new Dictionary<String, String>();
            //var kvpairs = alltext.Split('&');
            //foreach (var pair in kvpairs)
            //{
            //    //Tracing.Trace("pair: {0}", pair);
            //    var kv = pair.Split('=');
            //    _params.Add(kv[0], kv[1]);
            //}
            //// expected keys:
            ////   oauth_token, oauth_token_secret, user_id, screen_name, etc

            var textObject = JObject.Parse(alltext);
            var keys = textObject.Children();
            foreach (JProperty key in keys)
            {
                _params.Add(key.Name, key.First.Value<string>());
            }
        }
    }
}
