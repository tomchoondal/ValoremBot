using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Employee Handbook")]
        public async Task EmployeeHandbook(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Guide")]
        public async Task Guide(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Open Opportunities")]
        public async Task OpenOpportunities(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Organization chart")]
        public async Task Organizationchart(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }


        [LuisIntent("PTO")]
        public async Task PTO(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }


        [LuisIntent("Performance Review Template")]
        public async Task PerformanceReviewTemplate(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Planned PTOs")]
        public async Task PlannedPTOs(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }

        [LuisIntent("Travel Request Form")]
        public async Task TravelRequestForm(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnaDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(qnaDialog, null, messageToForward, CancellationToken.None);
        }
    }
}