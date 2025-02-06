using Microsoft.SqlServer.Management.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ScriptsResultModel
    {
        public int Success { get; set; }
        public int Failed { get; set; }
    }
}
