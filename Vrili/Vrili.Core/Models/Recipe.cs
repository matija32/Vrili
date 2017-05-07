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
        public List<CookingActivity> SerializableActivities
        {
            get { return new List<CookingActivity>(this.Activities); }
            set { Activities.AddRange(value); }
        }

        [Ignore]
        public ReactiveList<CookingActivity> Activities { get; private set; } = new ReactiveList<CookingActivity>();

    }
}
