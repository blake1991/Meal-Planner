using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeList.DataModel
{
  public  class RecipeIngredLinkModel
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }

        public int IngredientId { get; set; }
    }
}
