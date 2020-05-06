using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Models
{
    public class Address : Entity
    {
        public Guid ProviderId { get; set; }
        public string PublicPlace { get; set; }
        public string Number { get; set; }
        public string AddressComplement { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        /* EF Relation */

        public Provider Providers { get; set; }
    }
}
