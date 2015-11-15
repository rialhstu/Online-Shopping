using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
      public class ShippingDetails
    {

          public string Name { get; set; }
          public string Adress { get; set; }
          public int PostCode { get; set; }
          public string Thana { get; set; }

          public string Zilla { get; set; }

          public Int64 PhoneNumber {get;set;}

          public bool GiftWrap { get; set; }

    }
}
