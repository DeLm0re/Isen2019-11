using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Isen.Dotnet.Library.Model
{
    public class Person : BaseEntity
    {        
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public DateTime? DateOfBirth {get;set;}
        public string Telephone{get;set;}
        public string Email {get;set;}

        public ICollection<PersonRole> PersonRoles {get;set;}
        
        public override string ToString() =>
            $"{FirstName} {LastName} | {DateOfBirth} ({Telephone} / {Email})";
        
    }
}