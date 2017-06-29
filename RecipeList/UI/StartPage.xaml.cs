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

namespace RecipeList.UI
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MangeButton_Click(object sender, RoutedEventArgs e)
        {
            ManageRecipe mr = new ManageRecipe();
            this.NavigationService.Navigate(mr);
        }

        private void btnWhatIgot_Click(object sender, RoutedEventArgs e)
        {
            FindMeal oi = new FindMeal();
            this.NavigationService.Navigate(oi);
        }

        private void btnRandomMeal_Click(object sender, RoutedEventArgs e)
        {
            MealDisplay mp = new MealDisplay();
            this.NavigationService.Navigate(mp);
        }
    }
}
