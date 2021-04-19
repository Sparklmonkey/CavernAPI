using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Entities
{
    public class BuildingObject
    {
        public Vector3Object WorldPosition { get; set; }
        public int Dir { get; set; }
        public Vector2Object Origin { get; set; }
        public int BuildingObjectType { get; set; }
        public List<Vector2Object> GridPositionList { get; set; }
        public ulong StartBuildTime { get; set; }
        public ulong ConstructionTime { get; set; }
        public bool IsBuilt { get; set; }
        public bool IsUpgrading { get; set; }
        public int CurrentLevel { get; set; }
        public int IndexInList { get; set; }
    }
}
