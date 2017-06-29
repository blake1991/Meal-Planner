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
using RecipeList.UI;
using RecipeList.DataModel;

namespace RecipeList.UI
{
    /// <summary>
    /// Interaction logic for OwnedIngredients.xaml
    /// </summary>
    public partial class FindMeal : Page
    {

        private ObservableCollection<IngredientModel> ob_ingredients;
        private IngredientModel ingredient;

        public FindMeal()
        {
            InitializeComponent();

            ob_ingredients = new ObservableCollection<IngredientModel>();
           // DataContext = ob_ingredients;
        }

        private void pageOwnedIngredients_Loaded(object sender, RoutedEventArgs e)
        {
            //set the data sources
            DataContext = ob_ingredients;
            listboxIngredients.ItemsSource = ob_ingredients;
            tbIngredient.Focus();
        }

        private void btnMake_Click(object sender, RoutedEventArgs e)
        {
            if (ob_ingredients.Count > 0)
            {
                bool exclusive = exclusiveCheckBox.IsChecked.Value;
                MealDisplay mp = new MealDisplay(ob_ingredients,exclusive);
                this.NavigationService.Navigate(mp);
            }
        }

        private void btnIngredientAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbIngredient.Text != "")
            {
                ingredient = new IngredientModel();
                ingredient.Name = tbIngredient.Text.ToString();
                ob_ingredients.Add(ingredient);
                tbIngredient.Clear();
                tbIngredient.Focus();
            }
        }

        private void btnIngredientDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ob_ingredients.Count > 0 && listboxIngredients.SelectedIndex > -1)
            {
                ob_ingredients.RemoveAt(listboxIngredients.SelectedIndex);
            }
        }


    }
}
