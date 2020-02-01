using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace Isen.Dotnet.Library.Model
{
    public class Role : BaseEntity
    {
        public Role() {}
        public Role(string name)
        {
            Name = name;
        }
        public string Name {get;set;}

        public MyCollection<PersonRole> PersonRoles {get;set;}

        public string PersonDisplay() 
        {
            var personsDisplay = new StringBuilder();

            if(PersonRoles?.Count > 0)
            {
                foreach(var relation in PersonRoles)
                {
                    personsDisplay.Append(", ");
                    personsDisplay.Append(relation?.Person?.FirstName + " " + relation?.Person?.LastName);
                }
                personsDisplay.Remove(0, 1);
            }

            return personsDisplay.ToString();
        }
        
        public override string ToString() =>
            $"{Name}";
    }
}