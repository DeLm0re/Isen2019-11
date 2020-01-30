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

        public ICollection<PersonRole> PersonRoles {get;set;}

        public string PersonDisplay() 
        {
            var personsDisplay = new StringBuilder();

            if(PersonRoles != null)
            {
                foreach(var relation in PersonRoles)
                    personsDisplay.Append(relation?.Person?.FirstName + " " + relation?.Person?.LastName + ", ");
            }
                        
            return personsDisplay.ToString();
        }
        
        public override string ToString() =>
            $"{Name}";
    }
}