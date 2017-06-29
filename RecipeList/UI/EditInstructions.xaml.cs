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
using RecipeList.Database;
using RecipeList.DataModel;

namespace RecipeList.UI
{
    /// <summary>
    /// Interaction logic for EditInstructions.xaml
    /// </summary>
    public partial class EditInstructions : Window
    {
        private int index;
        private System.Collections.ObjectModel.ObservableCollection<RecipeModel> ob_recipeName;

        public EditInstructions()
        {
            InitializeComponent();
        }

        public EditInstructions(int p, System.Collections.ObjectModel.ObservableCollection<RecipeModel> ob_recipeName)
        {
            // TODO: Complete member initialization
            this.index = p;
            this.ob_recipeName = ob_recipeName;
            InitializeComponent();
            tbInstructions.Text = ob_recipeName.ElementAt(p).Instructions;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ob_recipeName.ElementAt(index).Instructions = tbInstructions.Text.ToString();
            RecipeTableDB.UpdateRecipeInstructions(ob_recipeName.ElementAt(index));
            this.Close();
        }

    }
}
