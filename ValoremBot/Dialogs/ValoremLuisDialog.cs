using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ValoremBot.Controllers;

namespace ValoremBot.Dialogs
{
    [LuisModel("1ed2eb71-f02b-4c50-80dc-9c6a8b91fe88", "0aecd1d068b04654bb7e5bd61ae81e3d")]
    [Serializable]
    public class ValoremLuisDialog : LuisDialog<object>
    {
        public static string Intent;

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I wasn't sure what you wanted.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }


        [LuisIntent("Branding")]
        public async Task Branding(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Employee Handbook")]
        public async Task EmployeeHandbook(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            if (result.Intents[0].Score > 0.5)
            {
                var messageToForward = await message;
                messageToForward.Text = result.Intents[0].Intent;
                await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
            }
            else
            {
                string messagetext = $"Sorry, I wasn't sure what you wanted.";
                await context.PostAsync(messagetext);
                context.Wait(this.MessageReceived);
            }
        }


        [LuisIntent("Guide")]
        public async Task Guide(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Open Opportunities")]
        public async Task OpenOpportunities(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }


        //[LuisIntent("Organization chart")]
        //public async Task Organizationchart(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        //{
        //    var messageToForward = await message;
        //    messageToForward.Text = result.Intents[0].Intent;
        //    await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        //}


        [LuisIntent("PTO")]
        public async Task PTO(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Performance Review Template")]
        public async Task PerformanceReviewTemplate(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Planned PTOs")]
        public async Task PlannedPTOs(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Travel Request Form")]
        public async Task TravelRequestForm(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Good Bye")]
        public async Task GoodBye(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Thanks")]
        public async Task Thanks(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(new ValoremQnaDialog(), AfterDialog, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Organization chart")]
        public async Task SignUp(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            EntityRecommendation er = new EntityRecommendation();
            // bool foind=Microsoft.Bot.Builder.Luis.Extensions.TryFindEntity(result, "entity",out er);
            await context.PostAsync("Great! I just need a few pieces of information to get you signed up.");
            PSA psa = new PSA();
            await psa.Authenticate(false, "Amanda");
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = PSA.attachments;
            await context.PostAsync(reply, CancellationToken.None);

            //var form = new FormDialog<FeedbackForm>(
            //    new FeedbackForm(),
            //    FeedbackForm.BuildForm,
            //    FormOptions.PromptInStart);
            //var messageToForward = await message;
            //messageToForward.Text = result.Intents[0].Intent;
            //await context.Forward(FormDialog.FromForm(FeedbackForm.BuildForm), FeedbackFormComplete, messageToForward,
            //CancellationToken.None);
        }

        private async Task FeedbackFormComplete(IDialogContext context, IAwaitable<FeedbackForm> result)
        {
            try
            {
                await context.PostAsync("Thanks for the feedback.");
                await context.PostAsync("What else would you like to do?");
            }
            catch (FormCanceledException)
            {
                await context.PostAsync("Don't want to send feedback? That's ok. You can drop a comment below.");
            }
            catch (Exception)
            {
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }


        private async Task AfterDialog(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var success = await result;
            if (!success.HasContent())
                await context.PostAsync("I did not understand you. Can you ask the question in a different way.");
            context.Wait(MessageReceived);
        }
    }
}