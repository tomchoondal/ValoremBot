using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ValoremBot.Helpers;

namespace ValoremBot.Controllers
{
    public class PSA
    {
        #region Fields

        public static List<Attachment> attachments;
        private static string accessToken = string.Empty;
        private static string refreshToken = string.Empty;
        private static bool isAuthenticated = false;
        #endregion

        public async Task Authenticate(bool isRefresh, string text)
        {
            string clientId = "5290667b-5862-40ef-9661-67f874c82283";
            string resource = "https://valorem.crm.dynamics.com";
            string loginUri = "https://login.windows.net/common/oauth2/token";
            if (!isAuthenticated)
            {
                WebRequestHelper loginHelper;
                loginHelper = new WebRequestHelper(RequestMethod.POST);
                loginHelper.AcceptHeaderRequired = true;
                loginHelper.Request = loginUri.ToString();

                if (!isRefresh)
                {
                    loginHelper.PostData = string.Format("resource={0}&client_id={1}&grant_type=password&username={2}&password={3}&scope=openid", resource, clientId, "CRMuser@valoremconsulting.com", "Redc@r213");
                }
                else
                {
                    loginHelper.PostData = string.Format("client_id={0}&grant_type=refresh_token&refresh_token={1}", clientId, refreshToken);
                }

                string response = await loginHelper.CallWebServiceAsync();
                JObject result = JObject.Parse(response);

                accessToken = result.SelectToken("access_token") != null ? result.SelectToken("access_token").ToString() : string.Empty;

                refreshToken = result.SelectToken("refresh_token") != null ? result.SelectToken("refresh_token").ToString() : string.Empty;
                isAuthenticated = true;
            }
            await FetchResources(text);

        }

        public async Task FetchResources(string empName)
        {
            attachments = new List<Attachment>();
            WebRequestHelper resourceHelper;
            string userUri = string.Format("https://valorem.crm.dynamics.com/api/data/v8.1/systemusers?$select=title,fullname&$filter=firstname%20eq%20'" + empName + "'");
            string authorization = string.Format("Bearer {0}", accessToken);

            resourceHelper = new WebRequestHelper();
            resourceHelper.AcceptHeaderRequired = true;
            resourceHelper.Request = userUri.ToString();
            resourceHelper.Headers = new Dictionary<string, string>();
            resourceHelper.Headers.Add("Authorization", authorization);

            string response = await resourceHelper.CallWebServiceAsync();
            var responseHelper = new JsonHelper();
            JToken result = responseHelper.TryParse(response);
            List<ThumbnailCard> empCards = new List<ThumbnailCard>();
            if (result != null)
            {
                foreach (JToken userToken in result.Children())
                {
                    string title = userToken.SelectToken("title").ToString();
                    string fullname = userToken.SelectToken("fullname").ToString();

                    ThumbnailCard card = new ThumbnailCard();
                    card.Title = fullname;
                    card.Subtitle = title;
                    card.Images = new List<CardImage>() { new CardImage("http://wwwimages.adobe.com/content/dam/acom/en/marketing-cloud/_overview/images/54658.en.marketing.cloud.overview.icon.personalize.50x50.png") };

                    Attachment attachment = new Attachment()
                    {
                        ContentType = ThumbnailCard.ContentType,
                        Content = card,

                    };
                    bool isDuplicate = false;
                    if (attachments.Count == 0)
                    {
                        attachments.Add(attachment);
                    }
                    else
                    {
                        foreach (Attachment cardAttachment in attachments)
                        {
                            if ((cardAttachment.Content as ThumbnailCard).Title == card.Title)
                            {
                                isDuplicate = true;
                            }
                        }
                        if (!isDuplicate)
                        {
                            attachments.Add(attachment);
                        }
                    }
                    //var webClient = new WebClient();
                    //byte[] imageBytes = webClient.DownloadData(@"C:\Users\lthomas\Documents\ValoremBot\ValoremBot\ValoremBot\Images\Guide.PNG");
                    //string url = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    //attachments.Add(new Attachment { ContentUrl = url, ContentType = "image/png" });


                }
            }

        }
        private static HeroCard GetUrlCard(string text, string buttonText)
        {
            return new HeroCard()
            {
                Text = text,
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, buttonText) }
            };
        }



    }
}