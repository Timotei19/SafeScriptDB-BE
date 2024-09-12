using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AppConstants
{
    public class Enums
    {
        public enum Status
        {
            NotStarted = 0,
            InProgress = 1,
            Finished = 2,
            Failed = 3
        }

        public enum Result
        {
            Failed = 0,
            Success = 1,
        }
    }
}
