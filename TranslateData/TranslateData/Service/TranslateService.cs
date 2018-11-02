using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TranslateData.Service
{
    public static class TranslateService
    {
        /// <summary>
        /// Translates a string into another language using Google's translate API JSON calls.
        /// <seealso>Class TranslationServices</seealso>
        /// </summary>
        /// <param name="Text">Text to translate. Should be a single word or sentence.</param>
        /// <param name="FromCulture">
        /// Two letter culture (en of en-us, fr of fr-ca, de of de-ch)
        /// </param>
        /// <param name="ToCulture">
        /// Two letter culture (as for FromCulture)
        /// </param>
        public static string TranslateGoogle(string text, string fromCulture, string toCulture)
        {
            fromCulture = fromCulture.ToLower();
            toCulture = toCulture.ToLower();

            // normalize the culture in case something like en-us was passed 
            // retrieve only en since Google doesn't support sub-locales
            string[] tokens = fromCulture.Split('-');
            if (tokens.Length > 1)
                fromCulture = tokens[0];

            // normalize ToCulture
            tokens = toCulture.Split('-');
            if (tokens.Length > 1)
                toCulture = tokens[0];

            string url = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                                       HttpUtility.UrlEncode(text), fromCulture, toCulture);

            // Retrieve Translation with HTTP GET call
            string html = null;
            try
            {
                WebClient web = new WebClient();

                // MUST add a known browser user agent or else response encoding doen't return UTF-8 (WTF Google?)
                web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

                // Make sure we have response encoding to UTF-8
                web.Encoding = Encoding.UTF8;
                html = web.DownloadString(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message);
                //return null;
            }

            // Extract out trans":"...[Extracted]...","from the JSON string
            //string result = Regex.Match(html, "trans\":(\".*?\"),\"", RegexOptions.IgnoreCase).Groups[1].Value;

            //if (string.IsNullOrEmpty(result))
            //{
            //    //this.ErrorMessage = Westwind.Globalization.Resources.Resources.InvalidSearchResult;
            //    return null;
            //}

            //return WebUtils.DecodeJsString(result);


            // Result is a JavaScript string so we need to deserialize it properly
            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //return ser.Deserialize(result, typeof(string)) as string;
            return html;
        }


        //public static string TranslateBabelFish(string Text, string FromCulture, string ToCulture)
        //{
        //    FromCulture = GetNeutralCulture(FromCulture).TwoLetterISOLanguageName;
        //    ToCulture = GetNeutralCulture(ToCulture).TwoLetterISOLanguageName;

        //    // Override since yahoo doesn't understand zh-Hans/zh-Hant
        //    if (FromCulture == "zh")
        //    {
        //        if (GetNeutralCulture(FromCulture).ThreeLetterISOLanguageName == "CHT")
        //        {
        //            FromCulture = "zt";
        //        }
        //    }

        //    if (ToCulture == "zh")
        //    {
        //        if (GetNeutralCulture(ToCulture).ThreeLetterISOLanguageName == "CHT")
        //        {
        //            ToCulture = "zt";
        //        }
        //    }
        //    string LangPair = FromCulture + "_" + ToCulture;

        //    string url = string.Format(@"http://babelfish.yahoo.com/translate_txt?ei=UTF-8&doit=done&fr=bf-home&intl=1&tt=urltext&trtext={0}&lp={1}&btnTrTxt=Translate",
        //                               HttpUtility.UrlEncode(Text), LangPair);

        //    // Retrieve Translation with HTTP GET call
        //    string Html = null;
        //    try
        //    {
        //        WebClient web = new WebClient();

        //        // MUST add the following browser user agent or else yahoo doesn't respond correctly (WTF Yahoo?)
        //        web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

        //        // Make sure we have response encoding to UTF-8
        //        web.Encoding = Encoding.UTF8;
        //        Html = web.DownloadString(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = Resources.Resources.ConnectionFailed + ": " +
        //                            ex.GetBaseException().Message;
        //        return null;
        //    }

        //    // <div id="result"><div style="padding:0.6em;">Hallo</div></div>
        //    string Result = StringUtils.ExtractString(Html, "<div id=\"result\">", "</div>");
        //    if (Result == "")
        //    {
        //        ErrorMessage = "Invalid search result. Couldn't find marker.";
        //        return null;
        //    }
        //    Result = Result.Substring(Result.LastIndexOf(">") + 1);

        //    return HttpUtility.HtmlDecode(Result);
        //}
    }
}
