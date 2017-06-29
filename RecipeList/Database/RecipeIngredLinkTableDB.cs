using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeList.DataModel;
using System.Data.SqlClient;

namespace RecipeList.Database
{
    class RecipeIngredLinkTableDB
    {
        public static List<RecipeIngredLinkModel> GetIngredientsForRecipe(int recipeId)
        {
            var ingredients = new List<RecipeIngredLinkModel>();

            SqlConnection connection = RecipeListDB.GetConnection();

            string selectStatement =
                 "SELECT i.IngredientID AS 'IngredientID', i.IngredientName, ril.RecipeID AS 'RecipeID', ril.RecIngredID AS 'RecIngredID' "
               + "FROM RecipeIngredientLink ril "+
               "JOIN Ingredient i " +
               "ON ril.IngredientID = i.IngredientID "+
                 
               "WHERE ril.RecipeID = @RecipeID " + 
               "ORDER BY i.IngredientName ASC";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("RecipeID", recipeId);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    var i = new RecipeIngredLinkModel();
                    i.Id = Convert.ToInt32(reader["RecIngredID"].ToString());
                    i.IngredientId = Convert.ToInt32(reader["IngredientID"].ToString());
                    i.RecipeId = Convert.ToInt32(reader["RecipeID"].ToString());

                    ingredients.Add(i);
                }
            }

            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return ingredients;
        }

        public static int AddIngredientToRecipe(RecipeIngredLinkModel model)
        {
            SqlConnection connection = RecipeListDB.GetConnection();

            string insertStatement = "INSERT INTO RecipeIngredientLink " +
                                     "(RecipeID, IngredientID) " +
                                     "VALUES(@RecipeID, @IngredientID)";
            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            insertCommand.Parameters.AddWithValue("RecipeID", model.RecipeId);
            insertCommand.Parameters.AddWithValue("IngredientID", model.IngredientId);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                string selectStatement = "SELECT IDENT_CURRENT('RecipeIngredientLink') "
                + "FROM RecipeIngredientLink";
                SqlCommand selectCommand =
                    new SqlCommand(selectStatement, connection);
                int ingredientID = Convert.ToInt32(selectCommand.ExecuteScalar().ToString());
                return ingredientID;

            }

            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool RemoveIngredientFromRecipe(RecipeIngredLinkModel model)
        {
            SqlConnection connection = RecipeListDB.GetConnection();

            string deleteStatement = "DELETE FROM RecipeIngredientLink " +
                                     "WHERE RecIngredID = @RecIngredID";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);
            deleteCommand.Parameters.AddWithValue("RecIngredID", model.Id);

            try
            {
                connection.Open();
                int rows = deleteCommand.ExecuteNonQuery();
                if (rows >= 1)
                    return true;
                else
                    return false;

            }

            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
                connection.Close();
            }
        }
        
    }
}
