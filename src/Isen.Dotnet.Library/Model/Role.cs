using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

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
        
        public override string ToString() =>
            $"{Name}";
    }
}