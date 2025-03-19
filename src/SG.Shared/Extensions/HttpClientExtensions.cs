using System.Collections.Specialized;
using System.Web;

namespace SG.Shared.Extensions;

public static class HttpClientExtensions
{
        public static Uri AddQueryParams(this Uri uri, string query)
        {
            var ub = new UriBuilder(uri);
            ub.Query = string.IsNullOrEmpty(uri.Query) ? query : string.Join("&", uri.Query.Substring(1), query);
            return ub.Uri;
        }

        public static Uri AddQueryParams(this Uri uri, IEnumerable<string> query)
        {
            return uri.AddQueryParams(string.Join("&", query));
        }

        public static Uri AddQueryParams(this Uri uri, string key, string value)
        {
            return uri.AddQueryParams(string.Join("=", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)));
        }

        public static Uri AddQueryParams(this Uri uri, params KeyValuePair<string, string>[] kvps)
        {
            return uri.AddQueryParams(kvps.Select(kvp => string.Join("=", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value))));
        }

        public static Uri AddQueryParams(this Uri uri, IDictionary<string, string> kvps)
        {
            return uri.AddQueryParams(kvps.Select(kvp => string.Join("=", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value))));
        }

        public static Uri AddQueryParams(this Uri uri, NameValueCollection nvc)
        {
            return uri.AddQueryParams(nvc.AllKeys.SelectMany(nvc.GetValues!, (key, value) => string.Join("=", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))));
        }
        public static Uri AddQueryParams(this Uri uri, string key, IEnumerable<Guid> values)
        {
            var query = string.Join("&", values.Select(value => $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value.ToString())}"));
            return uri.AddQueryParams(query);
        }
}
