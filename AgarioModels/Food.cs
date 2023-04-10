using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AgarioModels
{
    //[DebuggerDisplay("Food({X},{Y})")]
    public class Food : GameObject
    {
        public float radius { get; set; }
    }
}
