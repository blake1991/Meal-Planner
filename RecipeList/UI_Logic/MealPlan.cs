using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RecipeList.Database;
using RecipeList.DataModel;
using RecipeList.UI;

namespace RecipeList.UI_Logic
{
    class MealPlan
    {
        public ObservableCollection<Meal> ob_mealPlan;
        private List<RecipeModel> recipes;
        private List<RecipeIngredLinkModel> recipeIngredientLink;
        private List<IngredientModel> ingredients;
        private Meal newMeal;
        private Random rand;
        private int index;


        public MealPlan()
        {
            rand = new Random();
            ob_mealPlan = new ObservableCollection<Meal>();
            ingredients = new List<IngredientModel>();
            index = 0;
        }

        public void GenerateMeals()
        {
            //gets the list of available recipes
            recipes = RecipeTableDB.GetRecipeList();

            for (int i = 0; i < 3; i++)
            {
                
                newMeal = new Meal();
                index = rand.Next(recipes.Count);

                if (index < recipes.Count)
                {
                    newMeal.Name = recipes.ElementAt(index).Name;
                    recipeIngredientLink = RecipeIngredLinkTableDB.GetIngredientsForRecipe(recipes.ElementAt(index).Id);

                    StringBuilder sb = new StringBuilder();
                    foreach (RecipeIngredLinkModel link in recipeIngredientLink)
                    {
                        //ingredients.Add(IngredientTableDB.GetIngredient(link.IngredientId));
                        sb.Append(IngredientTableDB.GetIngredient(link.IngredientId).Name + "\n");
                    }



                    newMeal.Ingredients = sb.ToString();
                    newMeal.Instructions = recipes.ElementAt(index).Instructions;
                    ob_mealPlan.Add(newMeal);

                    //remove the selected recipe from the list to avoid repeats
                    recipes.RemoveAt(index);
                }
            }
        }

        public ObservableCollection<Meal> GetDataCollection()
        {
            return ob_mealPlan;
        }
    }
}
