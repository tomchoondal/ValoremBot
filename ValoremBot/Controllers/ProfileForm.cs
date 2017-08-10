using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValoremBot.Controllers
{
    [Serializable]
    public class FeedbackForm
    {
        [Prompt(new string[] { "What is your name?" })]
        public string Name { get; set; }

        [Prompt("What's your feedback?")]
        public string Feedback { get; set; }

        public static IForm<FeedbackForm> BuildForm()
        {
            return new FormBuilder<FeedbackForm>()
                //.Field(nameof(Contact))
                //.Field(nameof(Feedback))
                //.AddRemainingFields()
                .Build();
        }
    }
}