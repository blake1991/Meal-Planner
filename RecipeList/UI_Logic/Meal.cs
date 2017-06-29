using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecipeList.DataModel;

namespace RecipeList.UI_Logic
{
    class Meal
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _ingredients;
        public string Ingredients
        {
            get { return _ingredients; }
            set { _ingredients = value; }
        }

        private string _instructions;
        public string Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }


    }
}
