using MvvmCross.Plugins.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrili.Core.Models;

namespace Vrili.Core.Services
{
    public class SqliteRecipeRepo : RecipeRepo
    {
        private readonly SQLiteConnection _connection;

        public SqliteRecipeRepo(IMvxSqliteConnectionFactory factory)
        {
            _connection = factory.GetConnection("cookbook.sql");
            _connection.CreateTable<Recipe>();
            _connection.CreateTable<CookingActivity>();
        }

        public Recipe Get()
        {
            return _connection.Table<Recipe>().FirstOrDefault();
        }

        public void Save(Recipe recipe)
        {
            _connection.Insert(recipe);
        }
    }
}
