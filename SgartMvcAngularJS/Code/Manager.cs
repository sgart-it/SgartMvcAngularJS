using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BO = Sgart.MvcAngularJS.Models.BO;

namespace Sgart.MvcAngularJS.Code
{
  public static class Manager
  {
    // la versione va aggiornata ad ogni cambiamento dei css/js per essere
    // sicuri che i client prendano le modifiche senza eseguire un refresh (F5 o CTRL+F5)
    public const string VERSION = "2016-04-11";

    private static BO.DBQueries queries = new BO.DBQueries()
    {
      Search = "spu_todos_search",
      Read = "SELECT T.[id], [date], [title], [note], [idCategory], [category], [completed], [created], [modified]"
        + " FROM [todos] T INNER JOIN [categories] C ON T.[idCategory]=C.[id]"
        + " WHERE T.[id]=@id;",
      Insert = "INSERT INTO [todos] ([date],[title],[note],[idCategory],[modified],[created]) VALUES(@date,@title,@note,@idCategory,GETDATE(),GETDATE());",
      Update = "UPDATE [todos] SET [title]=@title,[date]=@date,[note]=@note,[idCategory]=@idCategory,[completed]=@completed,[modified]=GETDATE() WHERE [id]=@id;",
      Remove = "DELETE FROM [todos] WHERE [id]=@id;",
      Toggle = "UPDATE [todos] SET [completed]=CASE WHEN [completed] is null THEN GETDATE() ELSE null END, [modified]=GETDATE() WHERE [id]=@id;",
      UpdateCategory = "UPDATE [todos] SET [idCategory]=@idCategory,[modified]=GETDATE() WHERE [id]=@id;SELECT [id],[idCategory] FROM [todos] WHERE [id]=@id;",
      Categories = "SELECT [id],[category],[color] FROM [categories] ORDER BY [id];",
      Statistics = "SELECT c.[id],C.[category], C.[color], count(*) AS [count] FROM [todos] T"
        + " INNER JOIN [dbo].[categories] C ON T.[IDCategory]=C.[ID]"
        + " GROUP BY c.[ID],c.category, c.color"
        + " ORDER BY c.[ID];"
    };

    private static SqlConnection GetConnection()
    {
      SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connection1"].ConnectionString);
      return cnn;

    }

    public static List<BO.TodoItem> Search(int startIndex, int pageSize, string text, int? idCategory, int? status, string sort)
    {
      List<BO.TodoItem> result = new List<BO.TodoItem>();
      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Search;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add("@startIndex", SqlDbType.Int).Value = startIndex; ;
          cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;
          SqlParameter pText = cmd.Parameters.Add("@text", SqlDbType.NVarChar, 100);
          if (text != null)
            pText.Value = text;
          else
            pText.Value = DBNull.Value;
          SqlParameter pCategory = cmd.Parameters.Add("@idCategory", SqlDbType.Int);
          if (idCategory.HasValue)
            pCategory.Value = idCategory.Value;
          else
            pCategory.Value = DBNull.Value;
          SqlParameter pStatus = cmd.Parameters.Add("@status", SqlDbType.Int);
          if (status.HasValue)
            pStatus.Value = status.Value;
          else
            pStatus.Value = DBNull.Value;
          cmd.Parameters.Add("@sort", SqlDbType.NVarChar, 100).Value = sort;
          cnn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            while (dr.Read())
            {
              BO.TodoItem item = new BO.TodoItem();
              item.ID = (int)dr["ID"];
              item.Date = (DateTime)dr["Date"];
              item.Title = dr["Title"] == DBNull.Value ? "" : (string)dr["Title"];
              item.Note = dr["Note"] == DBNull.Value ? "" : (string)dr["Note"];
              item.IDCategory = (int)dr["IDCategory"];
              item.Category = (string)dr["Category"];
              item.Color = (string)dr["Color"];
              if (dr["Completed"] != DBNull.Value)
                item.Completed = (DateTime)dr["Completed"];
              item.Created = (DateTime)dr["Created"];
              item.Modified = (DateTime)dr["Modified"];
              item.TotalItems = (int)dr["TotalItems"];
              result.Add(item);
            }
          }
        }
      }
      return result;
    }

    public static BO.TodoItem Read(int id)
    {
      BO.TodoItem result = null;

      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Read;
          cmd.CommandType = CommandType.Text;
          cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
          cnn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            if (dr.Read())
            {
              BO.TodoItem item = new BO.TodoItem();
              item.ID = (int)dr["ID"];
              item.Date = (DateTime)dr["Date"];
              item.Title = dr["Title"] == DBNull.Value ? "" : (string)dr["Title"];
              item.Note = dr["Note"] == DBNull.Value ? "" : (string)dr["Note"];
              item.IDCategory = (int)dr["IDCategory"];
              item.Category = (string)dr["Category"];
              if (dr["Completed"] != DBNull.Value)
                item.Completed = (DateTime)dr["Completed"];
              item.Created = (DateTime)dr["Created"];
              item.Modified = (DateTime)dr["Modified"];
              result = item;
            }
          }
        }
      }
      return result;
    }

    public static int Insert(DateTime date, string title, string note, int idCategory)
    {
      int result = 0;
      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Insert;
          cmd.CommandType = CommandType.Text;
          cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = date;
          cmd.Parameters.Add("@title", SqlDbType.NVarChar, 100).Value = title;
          cmd.Parameters.Add("@note", SqlDbType.NVarChar, 4000).Value = note;
          cmd.Parameters.Add("@idCategory", SqlDbType.Int).Value = idCategory;
          cnn.Open();
          result = cmd.ExecuteNonQuery();

        }
      }
      return result;
    }

    public static int Update(int id, DateTime date, string title, string note, int idCategory, DateTime? completed)
    {
      int result = 0;
      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Update;
          cmd.CommandType = CommandType.Text;
          cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = date;
          cmd.Parameters.Add("@title", SqlDbType.NVarChar, 100).Value = title;
          cmd.Parameters.Add("@note", SqlDbType.NVarChar, 4000).Value = note;
          cmd.Parameters.Add("@idCategory", SqlDbType.Int).Value = idCategory;
          SqlParameter pCompleted = cmd.Parameters.Add("@Completed", SqlDbType.DateTime);
          if (completed.HasValue)
            pCompleted.Value = completed;
          else
            pCompleted.Value = DBNull.Value;
          cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

          cnn.Open();
          result = cmd.ExecuteNonQuery();
        }
      }
      return result;
    }

    public static int Delete(int id)
    {
      int result = 0;
      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Remove;
          cmd.CommandType = CommandType.Text;
          cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
          cnn.Open();
          result = cmd.ExecuteNonQuery();
        }
      }
      return result;
    }

    public static BO.TodoItem Toggle(int id)
    {
      BO.TodoItem result = null;

      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Toggle + queries.Read;
          cmd.CommandType = CommandType.Text;
          cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
          cnn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            if (dr.Read())
            {
              BO.TodoItem item = new BO.TodoItem();
              item.ID = (int)dr["ID"];
              item.Date = (DateTime)dr["Date"];
              item.Title = dr["Title"] == DBNull.Value ? "" : (string)dr["Title"];
              item.Note = dr["Note"] == DBNull.Value ? "" : (string)dr["Note"];
              item.IDCategory = (int)dr["IDCategory"];
              item.Category = (string)dr["Category"];
              if (dr["Completed"] != DBNull.Value)
                item.Completed = (DateTime)dr["Completed"];
              item.Created = (DateTime)dr["Created"];
              item.Modified = (DateTime)dr["Modified"];
              result = item;
            }
          }
        }
      }
      return result;
    }

    /// <summary>
    /// category update
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idCategory"></param>
    /// <returns></returns>
    public static BO.TodoCategoryItem Category(int id, int idCategory)
    {
      BO.TodoCategoryItem result = new BO.TodoCategoryItem();

      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.UpdateCategory;
          cmd.CommandType = CommandType.Text;
          cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
          cmd.Parameters.Add("@idCategory", SqlDbType.Int).Value = idCategory;
          cnn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            if (dr.Read())
            {
              result.ID = (int)dr["ID"];
              result.IDCategory = (int)dr["IDCategory"];
            }
          }
        }
      }
      return result;
    }

    public static List<BO.CategoryItem> Categories()
    {
      List<BO.CategoryItem> result = new List<BO.CategoryItem>();

      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Categories;
          cmd.CommandType = CommandType.Text;
          cnn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            while (dr.Read())
            {
              BO.CategoryItem item = new BO.CategoryItem();
              item.ID = (int)dr["ID"];
              item.Category = dr["Category"] == DBNull.Value ? "" : (string)dr["Category"];
              item.Color = dr["Color"] == DBNull.Value ? "" : (string)dr["Color"];
              result.Add(item);
            }
          }
        }
      }
      return result;
    }

    public static List<BO.CategoryItem> Statistics()
    {
      List<BO.CategoryItem> result = new List<BO.CategoryItem>();

      //eseguo la query sul DB
      using (SqlConnection cnn = GetConnection())
      {
        using (SqlCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandText = queries.Statistics;
          cmd.CommandType = CommandType.Text;
          cnn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            while (dr.Read())
            {
              BO.CategoryItem item = new BO.CategoryItem();
              item.ID = (int)dr["ID"];
              item.Category = dr["Category"] == DBNull.Value ? "" : (string)dr["Category"];
              item.Color = dr["Color"] == DBNull.Value ? "" : (string)dr["Color"];
              item.Count = (int)dr["Count"];
              result.Add(item);
            }
          }
        }
      }
      return result;
    }

  }
}