using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeList.DataModel;
using System.Data.SqlClient;
namespace RecipeList.Database
{
    class RecipeTableDB
    {
        public static List<RecipeModel> GetRecipeList()
        {
            var recipeList = new List<RecipeModel>();

            SqlConnection connection = RecipeListDB.GetConnection();

            string selectStatement =
                 "SELECT RecipeID, RecipeName, RecipeInstructions "
               + "FROM Recipe ORDER BY RecipeName ASC";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    RecipeModel r = new RecipeModel();
                    r.Id = Convert.ToInt32(reader["RecipeID"].ToString());
                    r.Name = reader["RecipeName"].ToString();
                    r.Instructions = reader["RecipeInstructions"].ToString();
                    recipeList.Add(r);
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

            return recipeList;

        }

        public static int AddRecipe(RecipeModel recipe)
        {
            SqlConnection connection = RecipeListDB.GetConnection();

            string insertStatement = "INSERT INTO Recipe " +
                                     "(RecipeName, RecipeInstructions) " +
                                     "VALUES(@Name, @Instructions)";
            
            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            insertCommand.Parameters.AddWithValue("@Name", recipe.Name);
            insertCommand.Parameters.AddWithValue("@Instructions", recipe.Instructions);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                string selectStatement = "SELECT IDENT_CURRENT('Recipe') "
                + "FROM Recipe";
                SqlCommand selectCommand =
                    new SqlCommand(selectStatement, connection);
                int recipeID = Convert.ToInt32(selectCommand.ExecuteScalar().ToString());
                return recipeID;
                
            }

            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

                                     
        }

        public static bool RemoveRecipe(RecipeModel recipe)
        {
            SqlConnection connection = RecipeListDB.GetConnection();

            string deleteStatement = "DELETE FROM Recipe " +
                                     "WHERE RecipeID = @RecipeID";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);
            deleteCommand.Parameters.AddWithValue("RecipeID", recipe.Id);

            try
            {
                connection.Open();
                int rows =  deleteCommand.ExecuteNonQuery();
                if (rows >= 1)
                    return true;
                else
                    return false;
               
            }

            catch(SqlException ex)
            {
                throw ex;
            }
            
            finally
            {
                connection.Close();
            }
        }

        //Updates the instructions of a recipe from the given recipeID.
        //Requires the "id" and "Instructions" field in the RecipeModel class.
        public static bool UpdateRecipeInstructions(RecipeModel recipe)
        {

            SqlConnection connection = RecipeListDB.GetConnection();

            String updateStatement = "UPDATE Recipe " +
                                     "SET RecipeInstructions= @Instructions " +
                                     "WHERE RecipeID = @RecipeID";

            SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
            updateCommand.Parameters.AddWithValue("Instructions", recipe.Instructions);
            updateCommand.Parameters.AddWithValue("RecipeID", recipe.Id);

            try
            {
                connection.Open();
                int rows = updateCommand.ExecuteNonQuery();
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
        
        //Searches available recipes that have include one or more of the ingredients in the list. The exclusive parameter
        // to tell the search to exclude the meals that contain those ingredients like if you are allergic to eggs or something. 

        public static List<RecipeModel> SearchAvailableRecipe(List<IngredientModel> ingredients, bool exclusive )
        {
             var recipeList = new List<RecipeModel>();
             

            //if there isn't any ingredients then just return all of the recipes
            if(ingredients == null || ingredients.Count == 0)
            {
                return GetRecipeList();
            }
           

            SqlConnection connection = RecipeListDB.GetConnection();



            string selectStatement = null;
              


            String likeString = "LIKE ";
            String boolString = "OR ";

            if (exclusive)
            {
                
                selectStatement = "SELECT r.RecipeID, r.RecipeName, r.RecipeInstructions "+
                                  "FROM Recipe r "+
                                  "WHERE r.RecipeID NOT IN " +
                                  "(SELECT  Distinct link.RecipeID "+
                                  "FROM  RecipeIngredientLink link " +
                                   "JOIN Ingredient i "+
                                   "ON link.IngredientID = i.IngredientID "+
                                   "WHERE ";
                
            }

            else
            {
                selectStatement =    "SELECT DISTINCT r.RecipeID, r.RecipeName, r.RecipeInstructions "
               + "FROM Recipe r " +
                 "JOIN RecipeIngredientLink link " +
                 "ON r.RecipeID = link.RecipeID " +
                 "JOIN Ingredient i " +
                 "ON link.IngredientID = i.IngredientID " +
                 "WHERE ";

            }

            
            for(int i =0; i < ingredients.Count; i++)
            {
                selectStatement += "i.IngredientName "+ likeString + "@Parameter" + i + " ";
                if (i < ingredients.Count - 1)
                    selectStatement += boolString;

                    
            }

            if (exclusive)
                selectStatement += ")";

            selectStatement += " ";
            selectStatement += "ORDER BY r.RecipeName ASC";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);

            for (int i = 0; i < ingredients.Count; i++ )
            {
                selectCommand.Parameters.AddWithValue("Parameter" + i, "%"+ingredients[i].Name + "%");
            }

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    RecipeModel r = new RecipeModel();
                    r.Id = Convert.ToInt32(reader["RecipeID"].ToString());
                    r.Name = reader["RecipeName"].ToString();
                    r.Instructions = reader["RecipeInstructions"].ToString();
                    recipeList.Add(r);
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

            return recipeList;
        }

    
    }
}
