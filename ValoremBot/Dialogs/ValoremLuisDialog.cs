using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ValoremBot.Dialogs
{
    [LuisModel("1ed2eb71-f02b-4c50-80dc-9c6a8b91fe88", "0aecd1d068b04654bb7e5bd61ae81e3d")]
    [Serializable]
    public class ValoremLuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            string message = $"Hi Friends! I'am your virtual assistant for any company related queries.Feel free to ask me any questions to dig through tons of Employee documents at lightning fast speed.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }
    }
}