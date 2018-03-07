using System.Collections.Generic;
using System.Security.Principal;

namespace Grid.Infrastructure
{
    public class Principal : IPrincipal
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IList<int> Permissions { get; set; }
        public Principal(string name, string code)
        {
            Permissions = new List<int>();
            Name = name;
            Code = code;
            Identity = new GenericIdentity(name);
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity { get; }

        public bool IsAdmin => Permissions.Contains(9999);
    }
}