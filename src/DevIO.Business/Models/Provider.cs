using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace DevIO.Business.Models
{
    public class Provider : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public ProviderType ProviderType { get; set; }
        public bool Active { get; set; }

        /* EF Relations*/

        public Address Address { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}