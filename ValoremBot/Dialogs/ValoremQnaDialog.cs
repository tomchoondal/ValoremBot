﻿using System;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using AdaptiveCards;
using System.Collections.Generic;
using System.Threading;


namespace ValoremBot.Dialogs
{
    [Serializable]
    [QnAMaker("76a58a9963e14c398cf630a9677dc525", "9dfcb7b8-4f15-43ee-8f69-21a2b9451e9d")]
    public class ValoremQnaDialog : QnAMakerDialog
    {
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            HeroCard card = new HeroCard();
            string cardTitle = "Here's the online link to {0}.";
            string buttonText = "View {0}";

            if (results.Answers.Count > 0)
            {
                if (results.Answers.First().Questions.Count == 1)
                {
                    if (results.Answers.First().Questions.First().ToString() == "Planned PTOs")
                    {
                        cardTitle = "These are the Planned PTOs for this calendar year";
                        card = GetUrlCard(string.Format(cardTitle, "Planned PTOs"), string.Format(buttonText, "Planned PTOs"), results);
                    }
                    if (results.Answers.First().Questions.First().ToString() == "Help")
                    {
                        card = new HeroCard()
                        {
                            Text = results.Answers.First().Answer.ToString()
                        };
                    }
                    else if (results.Answers.First().Questions.First().ToString() == "Performance Review Template")
                    {
                        cardTitle = "Here's the latest updated Performance Review Template";
                        card = GetUrlCard(string.Format(cardTitle, "Performance Review Template"), string.Format(buttonText, "Performance Review Template"), results);
                    }
                    else if (results.Answers.First().Questions.First().ToString() == "Guide")
                    {
                        cardTitle = "Our Mission in Action";
                        card = GetUrlCard(string.Format(cardTitle, "The Valorem Guide"), string.Format(buttonText, "Guide"), results);
                    }
                    else if (results.Answers.First().Questions.First().ToString() == "Branding")
                    {
                        cardTitle = "This folder contains the most recent updates to Templates and Assets";
                        card = GetUrlCard(string.Format(cardTitle, "Valorem Branding"), string.Format(buttonText, "Branding"), results);
                    }
                    else if (results.Answers.First().Questions.First().ToString() == "Employee Handbook")
                    {
                        cardTitle = "Hope you find it handy. If you're still having trouble finding something please reach out to : hr@valorem.com";
                        card = GetUrlCard(string.Format(cardTitle, "Employee Handbook"), string.Format(buttonText, "Employee Handbook"), results);
                    }
                    else if (results.Answers.First().Questions.First().ToString() == "Organization chart")
                    {
                        cardTitle = "The who's who of Valorem";
                        card = GetUrlCard(string.Format(cardTitle, "Organization chart"), string.Format(buttonText, "Organization chart"), results);
                    }
                    else if (results.Answers.First().Questions.First().ToString() == "Open Opportunities")
                    {
                        cardTitle = "Our current listing for US and India offices..";
                        card = GetUrlCard(string.Format(cardTitle, "Open Opportunities"), string.Format(buttonText, "Open Opportunities"), results);
                    }
                }
                else if (results.Answers.First().Questions.Count == 2)
                {
                    //Command with 2 
                }
                else if (results.Answers.First().Questions.Count > 2)
                {
                    if (results.Answers.First().Questions[0].ToString() == "Hi")
                    {
                        card = new HeroCard()
                        {
                            Text = results.Answers.First().Answer.ToString()
                        };
                    }
                    else if (results.Answers.First().Questions[2].ToString() == "pto")
                    {
                        card = GetUrlCard("Here's the online link to the Valorem PTO Request Form.", "Apply PTO", results);
                        cardTitle += "Enjoy your time off!";
                    }
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
                // var response = "Here is the match from FAQ:  \r\n  Q: " + results.Answers.First().Questions.First() + "  \r\n A: " + results.Answers.First().Answer;
                // await context.PostAsync(response);
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


        // Override to log matched Q&A before ending the dialog
        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            Console.WriteLine("KB Question: " + results.Answers.First().Questions.First());
            Console.WriteLine("KB Answer: " + results.Answers.First().Answer);
            await base.DefaultWaitNextMessageAsync(context, message, results);
        }
    }
}