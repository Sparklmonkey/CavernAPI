using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Entities
{
    public class IncubationObject
    {
        public string EggGrade { get; set; }
        public ulong StartTime { get; set; }
        public ulong HatchTime { get; set; }
        public int HatcheryIndex { get; set; }
    }
}
