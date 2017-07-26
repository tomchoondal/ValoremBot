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

        //[LuisIntent("")]
        //[LuisIntent("None")]
        //public async Task None(IDialogContext context, LuisResult result)
        //{
        //    string message = $"Sorry";

        //    await context.PostAsync(message);

        //    context.Wait(this.MessageReceived);
        //}
        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var faqDialog = new ValoremQnaDialog();
            var messageToForward = await message;
            messageToForward.Text = result.Intents[0].Intent;
            await context.Forward(faqDialog, null, messageToForward, CancellationToken.None);


        }

        private async Task AfterFAQDialog(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var messageHandled = await result;
            if (messageHandled != null)
            {
                await context.PostAsync("Sorry, I wasn't sure what you wanted.");
            }

            context.Wait(MessageReceived);
        }

        private async Task AfterFAQDialog(IDialogContext context, IAwaitable<bool> result)
        {
            var messageHandled = await result;
            if (!messageHandled)
            {
                await context.PostAsync("Sorry, I wasn't sure what you wanted.");
            }

            context.Wait(MessageReceived);
        }
    }
}