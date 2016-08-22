using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace SerrisCoffee
{
    public class SerrisCoffeeCompilators
    {
        private WebView view = new WebView(); private bool loaded = false;

        ///<summary>
        ///Compile JavaScript to Coffee - ex: string test = await SerrisCoffee.CompileJSToCoffee("var nmb = 42;");
        ///</summary>
        public async Task<string> CompileJSToCoffee(string code)
        {       
            if(!loaded)
            {
                /*
                 * LOAD WEBPAGE
                 */

                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///SerrisCoffee/web_assets/compilator.html"));
                view.NavigateToString(await FileIO.ReadTextAsync(file));

                view.LoadCompleted += (a, b) =>
                { loaded = true; return; };

                while (!loaded)
                { await Task.Delay(10); }

            }

            /*
             * COMPILE CODE AND RETURN RESULT
             */

            return await view.InvokeScriptAsync("eval", new string[] { "GetCoffee('" + JavaScriptEncode(code) + "');" });
        }

        ///<summary>
        ///Compile Coffee to JavaScript - ex: string test = await SerrisCoffee.CompileCoffeeToJS("nmb = 42");
        ///</summary>
        public async Task<string> CompileCoffeeToJS(string code)
        {
            if(!loaded)
            {
                /*
                 * LOAD WEBPAGE
                 */

                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///SerrisCoffee/web_assets/compilator.html"));
                view.NavigateToString(await FileIO.ReadTextAsync(file));

                view.LoadCompleted += (a, b) =>
                { loaded = true; return; };

                while (!loaded)
                { await Task.Delay(10); }
            }

            /*
             * COMPILE CODE AND RETURN RESULT
             */

            return await view.InvokeScriptAsync("eval", new string[] { "GetJS('" + JavaScriptEncode(code) + "');" });
        }

        //Convert C# String to JS String (HttpUtility doesn't exist in WinRT) - this function has been fund here: http://joonhachu.blogspot.fr/2010/01/c-javascript-encoder.html
        private static string JavaScriptEncode(string s)
        {
            if (s == null || s.Length == 0)
            {
                return string.Empty;
            }
            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);


            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>') || (c == '\''))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else
                {
                    if (c < ' ')
                    {
                        /*string tmp = new string(c, 1); 
                        t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                        sb.Append("\\u" + t.Substring(t.Length - 4));*/
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            return sb.ToString();
        }

    }
}
