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
    public class ManageRecipeLogic
    {

        //Properties
        public static List<RecipeIngredLinkModel> list_ingredients { get; set; }



        //Functions


        //Takes the current recipe selected in the cmbMeal combobox and populates listIngredients with that recipe's current ingredients
        public static ObservableCollection<IngredientModel> ui_addIngredientToListBoxCollection(int recipeId)
        {
            // int recipeId = (int)cmbMeal.SelectedValue;
            ObservableCollection<IngredientModel> ob_ingredients = new ObservableCollection<IngredientModel>();

            list_ingredients = Database.RecipeIngredLinkTableDB.GetIngredientsForRecipe(recipeId);

            //takes all of the ingredients in the ingredientname list and puts them into an observable collection -- Makes it easier to data bind
            foreach (RecipeIngredLinkModel ingredient in list_ingredients)
            {
                int id = ingredient.IngredientId;
                IngredientModel crntRecipeIngred = IngredientTableDB.GetIngredient(id);
                ob_ingredients.Add(crntRecipeIngred);
            }
            return ob_ingredients;
        }

        //takes an input of the name and instructions as entered on the form as well as a reference to where the meal names are stored
        //adds the new meal to the DB and then to the reference collection
        public static void addMealToCollection(string meal_name, string instructions, ref ObservableCollection<RecipeModel> mealList)
        {

            DataModel.RecipeModel newRecipe = new DataModel.RecipeModel();
            newRecipe.Name = meal_name;
            newRecipe.Instructions = instructions;

            int newRecipeId = Database.RecipeTableDB.AddRecipe(newRecipe);
            newRecipe.Id = newRecipeId;
            mealList.Add(newRecipe);

            // cmbMeal.SelectedIndex = ob_recipeName.IndexOf(newRecipe);
        }


        //fully deletes the selected recipe from both the meal collection as well as the database 
        //also deletes all ingredients that the recipe had as well as the links
        public static void ui_deleteRecipeFromEverything(RecipeModel recipeToDelete, ref ObservableCollection<RecipeModel> mealList)
        {
            List<RecipeIngredLinkModel> ingredientsAndLinksToDelete = RecipeIngredLinkTableDB.GetIngredientsForRecipe(recipeToDelete.Id);

            Database.RecipeTableDB.RemoveRecipe(recipeToDelete);
            mealList.Remove(recipeToDelete);

            //delete the ingredients
            foreach (RecipeIngredLinkModel ingred in ingredientsAndLinksToDelete)
            {
               IngredientTableDB.RemoveIngredient(IngredientTableDB.GetIngredient(ingred.IngredientId));
            }

            //delete the link recipeingredlink table
            foreach (RecipeIngredLinkModel link in ingredientsAndLinksToDelete)
            {
                RecipeIngredLinkTableDB.RemoveIngredientFromRecipe(link);
            }

        }

        public static void ui_deleteIngredientsFromCurrentRecipe(int id, int selectedRecipe, int selectedIngredient, ref ObservableCollection<IngredientModel> ingredientList)
        {
            //int id = Convert.ToInt32(listboxIngredients.SelectedValue);

            //get the ingredient
            IngredientModel ingredientToDelete = IngredientTableDB.GetIngredient(id);

            //delete the meal link
            List<RecipeIngredLinkModel> ingred = RecipeIngredLinkTableDB.GetIngredientsForRecipe(selectedRecipe);
            RecipeIngredLinkModel linkToDelete = ingred.Find(x => x.IngredientId.Equals(id));
            RecipeIngredLinkTableDB.RemoveIngredientFromRecipe(linkToDelete);

            //delete the ingredient
            IngredientTableDB.RemoveIngredient(ingredientToDelete);

            //remove the deleted ingredient from the listbox
            ingredientList.RemoveAt(selectedIngredient);
        }




    }
}
