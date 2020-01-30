using System;
using System.Collections.Generic;
using System.Linq;
using Isen.Dotnet.Library.Context;
using Isen.Dotnet.Library.Model;
using Microsoft.Extensions.Logging;

namespace Isen.Dotnet.Library.Services
{
    public class DataInitializer : IDataInitializer
    {
        private List<string> _firstNames => new List<string>
        {
            "Sang", 
            "Anne",
            "Boris",
            "Pierre",
            "Laura",
            "Hadrien",
            "Camille",
            "Louis",
            "Alicia"
        };
        private List<string> _lastNames => new List<string>
        {
            "Schuck",
            "Arbousset",
            "Lopasso",
            "Jubert",
            "Lebrun",
            "Dutaud",
            "Sarrazin",
            "Vu Dinh"
        };

        private List<string> _serviceNames => new List<string>
        {
            "Marketing",
            "Développement",
            "Commercial",
            "Maîtrise d'ouvrage",
            "Référenceur",
            "Designer",
            "Administration"
        };

        private List<string> _roleNames => new List<string>
        {
            "Responsable",
            "Assistant",
            "Chef de produit",
            "Chef de projet",
            "Maitre de l'air",
            "Développeur fullstack",
            "Développeur frontend",
            "Développeur backend",
            "Administrateur",
            "Testeur",
            "Stagiaire"
        };

        // Générateur aléatoire
        private readonly Random _random;

        // DI de ApplicationDbContext
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataInitializer> _logger;
        public DataInitializer(
            ILogger<DataInitializer> logger,
            ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _random = new Random();
        }

        // Générateur de prénom
        private string RandomFirstName => 
            _firstNames[_random.Next(_firstNames.Count)];
        // Générateur de nom
        private string RandomLastName => 
            _lastNames[_random.Next(_lastNames.Count)];
        // Générateur de date
        private DateTime RandomDate =>
            new DateTime(_random.Next(1980, 2010), 1, 1)
                .AddDays(_random.Next(0, 365));
        // Générateur de téléphone
        private string RandomTelephone =>
            '0' + _random.Next(100000000, 999999999).ToString();
        // Générateur de service
        private Service RandomService
        {
            get
            {
                var services = _context.ServiceCollection.ToList();
                return services[_random.Next(services.Count)];
            }
        }

        private List<PersonRole> RandomPersonRoles(Person person)
        {
            List<PersonRole> randomPersonRoles = new List<PersonRole>();

            var roles = _context.RoleCollection.ToList();

            // Nombre aléatoire de rôles à ajouter
            int nbRolesToAdd = _random.Next(4);
            List<int> indexes = new List<int>();

            // Retirer les doublons
            for(int i = 0; i < nbRolesToAdd; ++i)
                indexes.Add(_random.Next(roles.Count()));
                
            indexes = indexes.Distinct().ToList();
            

            // Générer les relations correspondantes
            foreach(var index in indexes)
            {
                Role role = roles[index];
                PersonRole personRole = new PersonRole();
                
                personRole.PersonId = person.Id;
                personRole.Person = person;
                personRole.RoleId = role.Id;
                personRole.Role = role;

                randomPersonRoles.Add(personRole);
            }

            return randomPersonRoles;
        }

        // Générateur de personne
        private Person RandomPerson()
        {
            Person person =  new Person();
            string firstName = RandomFirstName;
            string lastName = RandomLastName;
            person.FirstName = firstName;
            person.LastName = lastName;
            person.DateOfBirth = RandomDate;
            person.Telephone = RandomTelephone;
            person.Email = firstName.ToLower() + '.' + lastName.ToLower() + "@isen.yncrea.fr";
            person.Service = RandomService;
            return person;
        }

        // Générateur de personnes
        public List<Person> GetPersons(int size)
        {
            var persons = new List<Person>();
            for(var i = 0 ; i < size ; i++)
            {
                persons.Add(RandomPerson());
            }
            return persons;
        }

        public List<Service> GetServices()
        {
            var services = new List<Service>();
            int nbService = _serviceNames.Count();
            for (var i = 0; i < nbService; i++)
            {
                services.Add(new Service(_serviceNames[i]));
            }
            return services;
        }

        public List<Role> GetRoles()
        {
            var roles = new List<Role>();
            int nbRoles = _roleNames.Count();
            for (var i = 0; i < nbRoles; i++)
            {
                roles.Add(new Role(_roleNames[i]));
            }
            return roles;
        }   

        public void DropDatabase()
        {
            _logger.LogWarning("Dropping database");
            _context.Database.EnsureDeleted();
        }

        public void CreateDatabase()
        {
            _logger.LogWarning("Creating database");
            _context.Database.EnsureCreated();
        }

        public void AddPersons()
        {
            _logger.LogWarning("Adding persons...");
            // S'il y a déjà des personnes dans la base -> ne rien faire
            if (_context.PersonCollection.Any()) return;
            // Générer des personnes
            var persons = GetPersons(50);
            // Les ajouter au contexte
            _context.AddRange(persons);
            // Sauvegarder le contexte
            _context.SaveChanges();
        }

        public void AddServices()
        {
            _logger.LogWarning("Adding services...");
            if (_context.ServiceCollection.Any()) return;
            var services = GetServices();
            _context.AddRange(services);
            _context.SaveChanges();
        }

        public void AddRoles()
        {
            _logger.LogWarning("Adding roles...");
            if (_context.RoleCollection.Any()) return;
            var roles = GetRoles();
            _context.AddRange(roles);
            _context.SaveChanges();
        }

        public void LinkPersonsRoles()
        {
            _logger.LogWarning("Linking persons with roles...");
            var persons = _context.PersonCollection.ToList();
            var roles = _context.RoleCollection.ToList();

            foreach(var person in persons)
            {
                // Get relations Role-Person from Person talbe
                List<PersonRole> relations = RandomPersonRoles(person);

                foreach(var relation in relations)
                {
                    // Add it to the PersonRole table
                    _context.Add(relation);
                }

            }

            _context.SaveChanges();

        }
    }
}