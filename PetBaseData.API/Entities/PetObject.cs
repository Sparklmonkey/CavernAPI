using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Entities
{
    public class PetObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PetName { get; set; }
        public int Ability { get; set; }
        public float AbilityMulti { get; set; }
        public string PetGrade { get; set; }
        public int Level { get; set; }
        public int MaxStar { get; set; }
        public int Stamina { get; set; }
        public int Exp { get; set; }
        public int Agility { get; set; }
    }
}
