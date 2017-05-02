using MvvmCross.Plugins.Sqlite;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrili.Core.Models;

namespace Vrili.Core.Services
{
    public class SqliteRecipeRepo : IRecipeRepo
    {
        private readonly SQLiteConnection _connection;

        public SqliteRecipeRepo(IMvxSqliteConnectionFactory factory)
        {
            _connection = factory.GetConnection("cookbook.sql");
            _connection.CreateTable<Recipe>();
            _connection.CreateTable<CookingActivity>();
        }
        
        public int FindRecipeWithActivities()
        {
            return (from recipe in _connection.GetAllWithChildren<Recipe>()
                    where recipe.Activities.Count > 0
                    select recipe.Id).First();
        }

        public Recipe Get(int recipeId)
        {
            return _connection.GetWithChildren<Recipe>(recipeId);
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return _connection.Table<Recipe>();
        }

        public void Save(Recipe recipe)
        {
            _connection.InsertWithChildren(recipe);
        }
    }
}
