using System.Linq;
using Isen.Dotnet.Library.Context;
using Isen.Dotnet.Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Isen.Dotnet.Web.Controllers
{
    public class PersonController : BaseController<Person>
    {
        public PersonController(
            ILogger<PersonController> logger,
            ApplicationDbContext context) : base(logger, context)
        {
        }               

        // Override de la query : liste les personnes
        protected override IQueryable<Person> BaseQuery() =>
            base.BaseQuery()
                // Inclure Service lors d'une requête faite sur une ville
                .Include(p => p.Service)
                .Include(p => p.PersonRoles);
    }
}