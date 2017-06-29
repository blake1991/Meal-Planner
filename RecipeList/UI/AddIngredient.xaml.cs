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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using RecipeList.DataModel;
using RecipeList.Database;

namespace RecipeList.UI
{
    /// <summary>
    /// Interaction logic for AddIngredient.xaml
    /// </summary>
    public partial class AddIngredient : Window
    {
        private ObservableCollection<DataModel.IngredientModel> ob_ingredientName;
        private int recipeId;

        public AddIngredient()
        {
            InitializeComponent();
        }

        public AddIngredient(ref ObservableCollection<IngredientModel> ob_ingredientName, int recipeId)
        {
            // TODO: Complete member initialization
            this.ob_ingredientName = ob_ingredientName;
            this.recipeId = recipeId;
            InitializeComponent();
            tbName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            IngredientModel ingredient = new IngredientModel();
            ingredient.Name = tbName.Text.ToString();
           
            int id = IngredientTableDB.AddIngredient(ingredient);
            ingredient.Id = id;

            ob_ingredientName.Add(ingredient);

            //create the link between ingredient and recipe
            RecipeIngredLinkModel link = new RecipeIngredLinkModel();
            link.IngredientId = id;
            link.RecipeId = recipeId;
            
            RecipeIngredLinkTableDB.AddIngredientToRecipe(link);
         
            this.Close();

        }
    }
}
