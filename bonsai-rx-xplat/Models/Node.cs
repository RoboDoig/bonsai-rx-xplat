using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat.Models
{
    public class Node
    {
        public string Name { get; set; }

        public Func<CombinatorBuilder> Builder;
    }
}
