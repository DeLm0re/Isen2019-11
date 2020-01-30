using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

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

        public int? ServiceId {get;set;}
        public Service Service {get;set;}

        public string RolesDisplay() 
        {
            var rolesDisplay = new StringBuilder();

            if(PersonRoles != null)
            {
                foreach(var relation in PersonRoles)
                    rolesDisplay.Append(relation?.Role?.Name + ", ");
            }
            else
            {
                rolesDisplay.Append("Aucun");
            }

            return rolesDisplay.ToString();
        }
        
        public override string ToString() =>
            $"{FirstName} {LastName} | {DateOfBirth} ({Telephone} / {Email}) [{Service}]";
        
    }
}