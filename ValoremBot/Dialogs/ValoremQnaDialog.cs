using System;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using AdaptiveCards;
using System.Collections.Generic;
using System.Threading;
using System.Net;

namespace ValoremBot.Dialogs
{
    [Serializable]
    [QnAMaker("e7af343e213e453a9f9de0b342da36be", "05be6ac3-3181-4f50-9c89-f570f6dc9634")]
    public class ValoremQnaDialog : QnAMakerDialog
    {
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            HeroCard card = new HeroCard();
            string cardTitle = "Here's the online link to {0}.";
            string buttonText = "View {0}";
            string qnaQuestionText = results.Answers.First().Questions.First().ToString();

            if (results.Answers.Count > 0)
            {
                switch (qnaQuestionText)
                {
                    case "Greetings":
                        {
                            card = new HeroCard()
                            {
                                Text = results.Answers.First().Answer.ToString()
                            };
                        }
                        break;
                    case "PTO":
                        {
                            card = GetUrlCard("Here's the online link to the Greytip site.", "Apply PTO", results);
                            cardTitle = "Enjoy your time off!";
                        }
                        break;
                    case "Planned PTOs":
                        {
                            cardTitle = "These are the Planned PTOs for this calendar year";
                            card = GetUrlCard(string.Format(cardTitle, "Planned PTOs"), string.Format(buttonText, "Planned PTOs"), results);
                        }
                        break;
                    case "Performance Review Template":
                        {
                            cardTitle = "Here's the latest updated Performance Review Template";
                            card = GetUrlCard(string.Format(cardTitle, "Performance Review Template"), string.Format(buttonText, "Performance Review Template"), results);
                        }
                        break;
                    case "Guide":
                        {
                            cardTitle = "Our Mission in Action";
                            card = GetImageCard(string.Format(cardTitle, "The Valorem Guide"), string.Format(buttonText, "Guide"), results, @"C:\Users\tjose\Documents\Visual Studio 2017\Projects\ValoremBot\ValoremBot\Images\Guide.PNG");
                        }
                        break;
                    case "Branding":
                        {
                            cardTitle = "This folder contains the most recent updates to Templates and Assets";
                            card = GetUrlCard(string.Format(cardTitle, "Valorem Branding"), string.Format(buttonText, "Branding"), results);
                        }
                        break;
                    case "Employee Handbook":
                        {
                            cardTitle = "Hope you find it handy. If you're still having trouble finding something please reach out to : rsabu@valorem.com";
                            card = GetUrlCard(string.Format(cardTitle, "Employee Handbook"), string.Format(buttonText, "Employee Handbook"), results);
                        }
                        break;
                    case "Organization chart":
                        {
                            cardTitle = "The who's who of Valorem";
                            card = GetUrlCard(string.Format(cardTitle, "Organization chart"), string.Format(buttonText, "Organization chart"), results);
                        }
                        break;
                    case "Open Opportunities":
                        {
                            cardTitle = "Our current listing for US and India offices..";
                            card = GetUrlCard(string.Format(cardTitle, "Open Opportunities"), string.Format(buttonText, "Open Opportunities"), results);
                        }
                        break;
                    case "Travel Request Form":
                        {
                            cardTitle = "Find Travel Request Form here";
                            card = GetUrlCard(string.Format(cardTitle, "Travel Request Form"), string.Format(buttonText, "Travel Request Form"), results);
                        }
                        break;
                    case "Good Bye":
                        {
                            card = new HeroCard()
                            {
                                Text = results.Answers.First().Answer.ToString()
                            };
                        }
                        break;
                    case "Thanks":
                        {
                            card = new HeroCard()
                            {
                                Text = results.Answers.First().Answer.ToString()
                            };
                        }
                        break;
                    case "Help":
                        {
                            card = new HeroCard()
                            {
                                Text = results.Answers.First().Answer.ToString()
                            };
                        }
                        break;
                    default:
                        break;
                }

                Attachment attachment = new Attachment()
                {
                    ContentType = HeroCard.ContentType,
                    Content = card
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(attachment);
                await context.PostAsync(reply, CancellationToken.None);
                context.Wait(MessageReceivedAsync);
                context.Done(true);

            }
        }
        private static HeroCard GetUrlCard(string text, string buttonText, QnAMakerResults results)
        {
            return new HeroCard()
            {
                Text = text,
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, buttonText, value: results.Answers.First().Answer) }
            };
        }


        private static HeroCard GetImageCard(string text, string buttonText, QnAMakerResults results, string imageUrl)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(imageUrl);
                url = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
            }

            return new HeroCard()
            {
                Title = text,
                //Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, buttonText, value: results.Answers.First().Answer) },
                Images = new List<CardImage> { new CardImage(url) },
                Tap = new CardAction(ActionTypes.OpenUrl, buttonText, value: results.Answers.First().Answer),
            };
        }


        // Override to log matched Q&A before ending the dialog
        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            Console.WriteLine("KB Question: " + results.Answers.First().Questions.First());
            Console.WriteLine("KB Answer: " + results.Answers.First().Answer);
            await base.DefaultWaitNextMessageAsync(context, message, results);
        }
    }

}