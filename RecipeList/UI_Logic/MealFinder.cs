using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RecipeList.Database;
using RecipeList.DataModel;

namespace RecipeList.UI_Logic
{
    class MealFinder
    {
        private ObservableCollection<IngredientModel> ob_ingredients;
        private ObservableCollection<Meal> ob_meal;
      //  private List<IngredientModel> list_ingredients;
        private List<IngredientModel> tempIngred;
        private List<RecipeIngredLinkModel> tempLink;
       // private List<RecipeModel> tempRecipe;
        private List<RecipeModel> potentialRecipe;
        private Meal newMeal;

        public MealFinder(ObservableCollection<IngredientModel> ingredients,bool exclusive)
        {
            this.ob_ingredients = ingredients;
            tempIngred = new List<IngredientModel>();
            potentialRecipe = new List<RecipeModel>();
            ob_meal = new ObservableCollection<Meal>();
            findPotentialRecipeInDatabase(exclusive);
        }

        private void findPotentialRecipeInDatabase(bool exclusive)
        {

            try
            {
                potentialRecipe = RecipeTableDB.SearchAvailableRecipe(new List<IngredientModel>(ob_ingredients), exclusive);

                foreach (RecipeModel recipe in potentialRecipe)
                {
                    newMeal = new Meal();

                    newMeal.Name = recipe.Name;
                    newMeal.Instructions = recipe.Instructions;

                    tempLink = RecipeIngredLinkTableDB.GetIngredientsForRecipe(recipe.Id);

                    StringBuilder sb = new StringBuilder();
                    foreach (RecipeIngredLinkModel link in tempLink)
                    {
                        //ingredients.Add(IngredientTableDB.GetIngredient(link.IngredientId));
                        sb.Append(IngredientTableDB.GetIngredient(link.IngredientId).Name + "\n");
                    }

                    newMeal.Ingredients = sb.ToString();
                    Console.WriteLine(newMeal.Name);

                    if (newMeal != null)
                    {
                        ob_meal.Add(newMeal);
                    }

                }
            }
           catch(Exception ex)
           {
                return;
           }
           

        }

        //Levenshtein algorithm for computing differences between strings
        /*    public static int Compute(string s, string t)
            {
                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // Step 1
                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                // Step 2
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                }

                for (int j = 0; j <= m; d[0, j] = j++)
                {
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    //Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                        // Step 6
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }
                // Step 7
                return d[n, m];
            }
            */



        public ObservableCollection<Meal> GetDataCollection()
        {
            return ob_meal;
        }
    }
}
