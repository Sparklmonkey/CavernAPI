using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Entities
{
    public class SavedData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool UiTransition { get; set; }
        public int PlayerLevel { get; set; }
        public int PlayerXP { get; set; }
        public double StonePerHour { get; set; }
        public double WoodPerHour { get; set; }
        public double CoinPerHour { get; set; }
        public bool AdventureRevive { get; set; }

        //Resource Variables
        public int WoodCount { get; set; }
        public int StoneCount { get; set; }
        public int CoinCount { get; set; }
        public int EggCount { get; set; }
        public int MaxFood { get; set; }
        public int CurrentFood { get; set; }

        //Building Variables
        public IEnumerable<string> UnlockedRegions { get; set; }
        public IEnumerable<BuildingObject> ConstructedBuilding { get; set; }

        //Pet Variables
        public IEnumerable<PetObject> ListOfPets { get; set; }
        public IEnumerable<PetObject> StarterPetList { get; set; }
        public IEnumerable<PetObject> PlayerPetList { get; set; }
        public PetObject AdventurePet { get; set; }
        public IEnumerable<IncubationObject> IncubatingPets { get; set; }
        public int PetCap { get; set; }
        public double HatchModifier { get; set; }

        //Adventure Variables
        public int AdventureType { get; set; }
        public double DropModifier { get; set; }
    }
}
