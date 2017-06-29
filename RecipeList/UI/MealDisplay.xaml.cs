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
using RecipeList.UI_Logic;


namespace RecipeList.UI
{
    /// <summary>
    /// Interaction logic for WeeklyPlan.xaml
    /// </summary>
    public partial class MealDisplay : Page
    {

        MealPlan meals;
        MealFinder finder;
        
        //default constructor generates it's own list of meals randomly
        public MealDisplay()
        {
            InitializeComponent();
            meals = new MealPlan();
            meals.GenerateMeals();
            DataContext = meals.GetDataCollection();
        }

        public MealDisplay(ObservableCollection<IngredientModel> ingredients, bool exclusive)
        {
            InitializeComponent();
            finder = new MealFinder(ingredients,exclusive);
            DataContext = finder.GetDataCollection();
        }
    }
}
