using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.Model
{
    public class Work
    {
        public int TaskId { get; set; }
        public string GroupName { get; set; }
        public CancellationTokenSource Token { get; set; }
    }
}
