using ReactiveUI;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace Vrili.Core.Models
{
    [Table("Recipe")]
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CookingActivity> Activities { get; set; } = new List<CookingActivity>();
    }

    public class ReactiveRecipe : ReactiveObject
    {
        public int Id { get; private set; }

        private string _name;
        public string Name
        {
            get { return this._name; }
            private set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public ReactiveList<CookingActivity> Activities { get; private set; } = new ReactiveList<CookingActivity>();

        public ReactiveRecipe(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Activities.AddRange(recipe.Activities);
        }
        
        public Recipe ExtractRecipe()
        {
            return new Recipe
            {
                Id = this.Id,
                Name = this.Name,
                Activities = new List<CookingActivity>(this.Activities)
            };
        }
    }

}
