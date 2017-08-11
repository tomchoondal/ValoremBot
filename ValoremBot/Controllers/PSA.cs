using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using ValoremBot.Helpers;
using Microsoft.Bot.Connector;

namespace ValoremBot.Controllers
{
    public class PSA
    {
        #region Fields

        private const double hoursInOneYear = 2080;
        private DateTime startDate;
        private DateTime endDate;
        public static List<Attachment> attachments;

        StringBuilder crmMessages = new StringBuilder();
        // List<TeamMembership> teamMemberships = new List<TeamMembership>();

        private static string accessToken = string.Empty;

        private static string refreshToken = string.Empty;

        private static DateTime expiryTime;
        StringBuilder mailDraft = new StringBuilder();
        private static string userName = "resourcemanagement@valorem.com";
        private static string password = "NewUser1!";
        List<string> recipients = new List<string>() { "avarghese@valorem.com", "jmangaly@valorem.com" };
        const string SMTP_SERVER = "smtp.office365.com";
        const int PORT_NUMBER = 587;
        private bool resourceMigrationFailed = false;
        private bool projectMigrationFailed = false;
        private bool timeMigrationFailed = false;
        #endregion

        public async Task Authenticate(bool isRefresh, string text)
        {
            string clientId = "5290667b-5862-40ef-9661-67f874c82283";
            string resource = "https://valorem.crm.dynamics.com";
            string loginUri = "https://login.windows.net/common/oauth2/token";
            try
            {
                //AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/common", false);
                //UserPasswordCredential cred = new UserPasswordCredential("mmohan@Valorem.com", "MyPassword");
                //AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientId, cred);
                //accessToken = result.AccessToken;b

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

                crmMessages.AppendLine("Sending Authentication request");
                string response = await loginHelper.CallWebServiceAsync();
                JObject result = JObject.Parse(response);

                accessToken = result.SelectToken("access_token") != null ? result.SelectToken("access_token").ToString() : string.Empty;

                refreshToken = result.SelectToken("refresh_token") != null ? result.SelectToken("refresh_token").ToString() : string.Empty;

                //  double expireOnSeconds = result.SelectToken("expires_on") != null ? result.SelectToken("expires_on").JsonValueToDouble() : 0;

                //  expiryTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expireOnSeconds).ToLocalTime();

                //if (!string.IsNullOrWhiteSpace(accessToken))
                //    crmMessages.AppendLine("Auhtentication successfull. Access token received");
                await FetchResources(text);
            }
            catch (Exception ex)
            {
                // crmMessages.AppendLine(string.Format("         ********** Error in Authentication. {0}", GetExceptionString(ex)));
                // mailDraft.AppendLine(string.Format("********** Error in Authentication. {0}", GetExceptionString(ex)));
            }
        }

        public async Task FetchResources(string empName)
        {
            string psaProfileExtensionUserId = "95af9f8a-765e-e611-80e8-5065f38ac9a1";   //PSA id for 'PSA Extension Profile User'
            string valoremId = "8c7668d1-4f16-e511-80d5-3863bb35cc28"; //PSA id for valorem
            attachments = new List<Attachment>();
            WebRequestHelper resourceHelper;
            //  string empName = "Amanda";
            //URI to fetch valorem employee details
            // string userUri = string.Format("https://valorem.crm.dynamics.com/api/data/v8.1/systemusers?$select=fullname,_parentsystemuserid_value,internalemailaddress,systemuserid,_businessunitid_value,_psa_legalentity_value&$filter=isdisabled%20eq%20false");
            string userUri = string.Format("https://valorem.crm.dynamics.com/api/data/v8.1/systemusers?$select=title,fullname&$filter=firstname%20eq%20'" + empName + "'");

            string authorization = string.Format("Bearer {0}", accessToken);
            try
            {
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
                        attachments.Add(attachment);
                        empCards.Add(card);

                    }
                }
            }
            catch (Exception ex)
            {
                resourceMigrationFailed = true;
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