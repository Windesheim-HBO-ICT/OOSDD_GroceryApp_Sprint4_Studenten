using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Models
{
    /// <summary>
    /// Definieert de rollen die een client kan hebben
    /// Gebruikt voor toegangsbeheer (bijvoorbeeld alleen Admin ziet klantgegevens)
    /// </summary>
    public enum Role
    {
        None,
        Admin
    }
}