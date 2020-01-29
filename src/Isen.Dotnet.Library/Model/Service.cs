using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Isen.Dotnet.Library.Model
{
    public class Service : BaseEntity
    {
        public Service() {}
        public Service(string name)
        {
            Name = name;
        }
        public string Name {get;set;}
        
        public override string ToString() =>
            $"{Name}";
    }
}