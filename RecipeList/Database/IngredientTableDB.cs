using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeList.DataModel;
using System.Data.SqlClient;
namespace RecipeList.Database
{
    class IngredientTableDB
    {
        public static List<IngredientModel> GetIngredientList()
        {
            var ingredients = new List<IngredientModel>();

            SqlConnection connection = RecipeListDB.GetConnection();

            string selectStatement =
                 "SELECT IngredientID, IngredientName "
               + "FROM Ingredient ORDER BY IngredientName ASC";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    IngredientModel i = new IngredientModel();
                    i.Id = Convert.ToInt32(reader["IngredientID"].ToString());
                    i.Name = reader["IngredientName"].ToString();
                   
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



        public static IngredientModel GetIngredient(int ingredientId)
        {
            SqlConnection connection = RecipeListDB.GetConnection();

            string selectStatement =
                 "SELECT IngredientID, IngredientName "
               + "FROM Ingredient " +
               "WHERE IngredientID = @IngredientID";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("IngredientID", ingredientId);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                if(reader.Read())
                {
                    var ingredient = new IngredientModel();
                    ingredient.Id = Convert.ToInt32(reader["IngredientID"].ToString());
                    ingredient.Name = reader["IngredientName"].ToString();
                    return ingredient;
                }
                else
                {
                    return null;
                }
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

        public static int AddIngredient(IngredientModel ingredient)
        {

            SqlConnection connection = RecipeListDB.GetConnection();

            string insertStatement = "INSERT INTO Ingredient " +
                                     "(IngredientName) " +
                                     "VALUES(@Name)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            insertCommand.Parameters.AddWithValue("Name", ingredient.Name);
            

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                string selectStatement = "SELECT IDENT_CURRENT('Ingredient') "
                + "FROM Ingredient";
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

        public static bool RemoveIngredient(IngredientModel ingredient)
        {
            SqlConnection connection = RecipeListDB.GetConnection();

            string deleteStatement = "DELETE FROM Ingredient " +
                                     "WHERE IngredientID = @IngredientID";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);
            deleteCommand.Parameters.AddWithValue("IngredientID", ingredient.Id);

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
