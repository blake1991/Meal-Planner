using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using RecipeList.DataModel;
using RecipeList.Database;
using RecipeList.UI_Logic;

namespace RecipeList.UI
{


    /// <summary>
    /// Interaction logic for ManageRecipe.xaml
    /// </summary>
    public partial class ManageRecipe : Page
    {

        private List<RecipeModel> list_recipeName;
        private ObservableCollection<RecipeModel> ob_recipeName;
        private ObservableCollection<IngredientModel> ob_ingredientName;

        private bool canAddRecipe; //if the user has entered a new name this value will be true otherwise it'll be false

        public ManageRecipe()
        {
            InitializeComponent();
            canAddRecipe = true;
        }

        private void pageManageRecipe_Loaded(object sender, RoutedEventArgs e)
        {

            list_recipeName = Database.RecipeTableDB.GetRecipeList();
            ob_recipeName = new ObservableCollection<RecipeModel>();
            ob_ingredientName = new ObservableCollection<IngredientModel>();

            //put all of list_recipeName into ob_recipeName  -- Helps with databinding
            foreach (RecipeModel recipe in list_recipeName)
            {
                ob_recipeName.Add(recipe);
            }

            cmbMeal.DataContext = ob_recipeName;
            cmbMeal.Focus();
        }

        private void cmbMeal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listboxIngredients.ItemsSource = null; //keeps ingredient items from showing up in the wrong meal
            ob_ingredientName.Clear(); // empties out the previous recipes ingredients

            if (cmbMeal.SelectedValue != null)
            {
                canAddRecipe = false; //sets canAddRecipe to false because an already defined recipe was selected
                ob_ingredientName = UI_Logic.ManageRecipeLogic.ui_addIngredientToListBoxCollection(Convert.ToInt32(cmbMeal.SelectedValue));
                listboxIngredients.ItemsSource = ob_ingredientName;
            }
            else
            {
                canAddRecipe = true; //the user has cleared out a defined recipe and typed in their own
            }
        }

        private void btnMealAdd_Click(object sender, RoutedEventArgs e)
        {
            if (canAddRecipe == true)
            {
                if (cmbMeal.Text != "")
                {
                    UI_Logic.ManageRecipeLogic.addMealToCollection(cmbMeal.Text.ToString(), "", ref ob_recipeName);

                    //sets the selected index to be the recently added recipe
                    cmbMeal.SelectedIndex = ob_recipeName.Count() - 1;
                }
            }
        }

        private void btnMealDelete_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMeal.SelectedIndex > -1)
            {
                UI_Logic.ManageRecipeLogic.ui_deleteRecipeFromEverything(ob_recipeName.ElementAt(cmbMeal.SelectedIndex), ref ob_recipeName);
            }
        }

        private void btnDeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (listboxIngredients.SelectedIndex > -1)
            {
                UI_Logic.ManageRecipeLogic.ui_deleteIngredientsFromCurrentRecipe(Convert.ToInt32(listboxIngredients.SelectedValue),
                    Convert.ToInt32(cmbMeal.SelectedValue), listboxIngredients.SelectedIndex, ref ob_ingredientName);
            }

        }

        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMeal.SelectedIndex > -1)
            {
                int recipeId = Convert.ToInt32(cmbMeal.SelectedValue);

                AddIngredient add = new AddIngredient(ref ob_ingredientName, recipeId);
                add.Show();
            }
        }

        private void btnEditInstructions_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMeal.SelectedIndex > -1)
            {
                EditInstructions ei = new EditInstructions(Convert.ToInt32(cmbMeal.SelectedIndex), ob_recipeName);
                ei.Show();
            }

        }

    }
}
