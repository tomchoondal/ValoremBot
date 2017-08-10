using Newtonsoft.Json.Linq;
using System;

namespace ValoremBot.Helpers
{
    public class JsonHelper
    {
        private string nextUrl;

        public JsonHelper()
        {

        }

        public string NexUrl()
        {
            return nextUrl;
        }


        public JToken TryParse(string responseText)
        {
            JToken jresult = null;
            nextUrl = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(responseText))
                {
                    JObject jsonObject = JObject.Parse(responseText);
                    jresult = jsonObject.SelectToken("value");

                    if (jsonObject.SelectToken("['@odata.nextLink']") != null)
                        nextUrl = jsonObject.SelectToken("['@odata.nextLink']").ToString();
                }
            }
            catch (Exception exp)
            {
                jresult = null;
                nextUrl = string.Empty;
            }
            return jresult;
        }
    }
}
