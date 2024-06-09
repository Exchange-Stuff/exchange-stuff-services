using ExchangeStuff.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Models.Images;

public class ImageViewModel : BaseEntity<Guid>
{
    public string Url { get; set; }
}
