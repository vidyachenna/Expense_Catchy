using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Expensess
{
    [Serializable]
    [LuisModel("fc5a91ba-2fe7-41ec-968b-7c4b7809e91c", "79b44584a48c4a52be74d52000841911")]
    public class Luis : LuisDialog<object>
    {
        public string message;
        public Luis(string message)
        {
            this.message = message;
        }
        public int total_amtFood()
        {
            int total = 0;

            Models.DatabaseEntities2 db = new Models.DatabaseEntities2();
            var amt = (from Category_det in db.Category_det
                       select Category_det.Food)
                            .ToList();

            foreach (var score in amt)
            {
                // Add the High Score to the response
                int sc = int.Parse(score.ToString());
                total += sc;
            }
            return total;
        }

        public int total_amtEducation()
        {
            int total = 0;

            Models.DatabaseEntities2 db = new Models.DatabaseEntities2();
            var amt = (from Category_det in db.Category_det
                       select Category_det.Education)
                            .ToList();

            foreach (var score in amt)
            {
                // Add the High Score to the response
                int sc = int.Parse(score.ToString());
                total += sc;
            }
            return total;
        }

        public int total_amtHealth()
        {
            int total = 0;

            Models.DatabaseEntities2 db = new Models.DatabaseEntities2();
            var amt = (from Category_det in db.Category_det
                       select Category_det.Health)
                            .ToList();

            foreach (var score in amt)
            {
                // Add the High Score to the response
                int sc = int.Parse(score.ToString());
                total += sc;
            }
            return total;
        }

        public int total_amtTravel()
        {
            int total = 0;

            Models.DatabaseEntities2 db = new Models.DatabaseEntities2();
            var amt = (from Category_det in db.Category_det
                       select Category_det.Travel)
                            .ToList();

            foreach (var score in amt)
            {
                // Add the High Score to the response
                int sc = int.Parse(score.ToString());
                total += sc;
            }
            return total;
        }
        public int total_amtOthers()
        {
            int total = 0;

            Models.DatabaseEntities2 db = new Models.DatabaseEntities2();
            var amt = (from Category_det in db.Category_det
                       select Category_det.Others)
                            .ToList();

            foreach (var score in amt)
            {
                // Add the High Score to the response
                int sc = int.Parse(score.ToString());
                total += sc;
            }
            return total;
        }

        [LuisIntent("Greetings")]
        
        public async Task GreetingsIntent(IDialogContext context, LuisResult result)
        {
            PromptDialog.Confirm(context, ConfirmationAboutPrompt, $"" + message + "  Do you want to know about me?");
            //await context.PostAsync($"hello.........");
            
        }
        [LuisIntent("About")]
        public async Task AboutIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"I help you in having an eye on your expenses." + Environment.NewLine + Environment.NewLine +
                    "I love to learn your interests in expenditure and suggest accordingly" + Environment.NewLine + Environment.NewLine +
                    "So please give me your income");
        }
        //await context.PostAsync($"hello.........");
        //  context.Wait(MessageReceived);

        private async Task ConfirmationAboutPrompt(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync($"I help you in having an eye on your expenses." + Environment.NewLine + Environment.NewLine +
                    "I love to learn your interests in expenditure and suggest accordingly" + Environment.NewLine + Environment.NewLine +
                    "So please give me your income");
                await context.PostAsync($"Please enter your income");
            }
            else
            {
                await context.PostAsync($"Hope you already heard about me" + Environment.NewLine + Environment.NewLine +
                    "If not, you can know about me any time when you are free");
                await context.PostAsync($"Please enter your income");
            }
        }
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            int reply = total_amtFood();
            string res = reply.ToString();
            await context.PostAsync(res);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Category Wise")]
        public async Task CategoryNull(IDialogContext context, LuisResult result)
        {
            int[] t_amt = new int[] {total_amtFood(), total_amtEducation(), total_amtHealth(), total_amtTravel(), total_amtOthers()};
            string[] arr = new string[] { "Food", "Education", "Health", "Travel", "Others" };
            string Zero = "hai";
            for (int i = 0; i < t_amt.Length; i++)
            {
                if (t_amt[i].Equals(0) )
                {
                    Zero = arr[i];
                }
            }


            await context.PostAsync($"the category in which you havent spent amount is "+Zero);
            context.Wait(MessageReceived);
        }
        [LuisIntent("More Expense")]
        public async Task Highestamount(IDialogContext context, LuisResult result)
        {
            
            int[] high_amt = new int[] { total_amtFood(), total_amtEducation(), total_amtHealth(), total_amtTravel(), total_amtOthers() };
            string[] arr = new string[] { "Food", "Education", "Health", "Travel", "Others" };
            int high = high_amt[0];
            string cat = arr[0];
            for (int i = 1; i < high_amt.Length; i++)
            {
                if (high < high_amt[i])
                {
                    cat = arr[i];
                }
            }
            await context.PostAsync($"the category in which you spent more amount is + {cat}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("AmountSpent")]
        public async Task AmountSpentIntent(IDialogContext context, LuisResult result)
        {
            string categ = "";
            string response = "";
            int reply = 0;
            EntityRecommendation res;
            await context.PostAsync($"forrrr");
            if (result.TryFindEntity("Category", out res))
            {
                await context.PostAsync($"since");
                categ = res.Entity;
                //RestClient rs = new RestClient();
                //string http_response = rs.MakeRequest(placename);
                await context.PostAsync($"since");
                if (categ.ToLower().Equals("food"))
                {
                    reply = total_amtFood();
                }

                if (categ.ToLower().Equals("education"))
                {
                    reply = total_amtEducation();
                }

                if (categ.ToLower().Equals("health"))
                {
                    reply = total_amtHealth();
                }

                if (categ.ToLower().Equals("travel"))
                {
                    reply = total_amtTravel();


                    if (categ.ToLower().Equals("others"))
                    {
                        reply = total_amtOthers();
                    }
                    response = reply.ToString();
                    await context.PostAsync($"the amount you spent in {categ} is {response}");
                }
                response = reply.ToString();
                await context.PostAsync($"the amount you spent in category is {response}");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Income")]
        public async Task IncomeIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have successfully entered your income");
            context.Wait(MessageReceived);

        }
        [LuisIntent("Bye")]
        public async Task ByeIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Byeee.....");
            context.Wait(MessageReceived);
        }
        private static List<CardAction> CreateButtons()
        {
            // Create 5 CardAction buttons 
            // and return to the calling method 
            List<CardAction> cardButtons = new List<CardAction>();
            string[] arr1 = new string[] { "Food", "Education", "Health", "Travel", "Others" };
            for (int i = 0; i < arr1.Length; i++)
            {
                string Category = Convert.ToString(arr1[i]);
                CardAction CardButton = new CardAction()
                {
                    Type = default(string),
                    Title = Category,
                    Value = Category
                };
                cardButtons.Add(CardButton);
            }
            return cardButtons;
        }

    }
}