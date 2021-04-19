using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetBaseData.API.Entities;

namespace PetBaseData.API.Data
{
    public class PetObjectSeed
    {
        public static void SeedData(IMongoCollection<PetObject> petObjectCollection) 
        {
            bool existPet = petObjectCollection.Find(p => true).Any();
            if (!existPet)
            {
                petObjectCollection.InsertManyAsync(GetPreconfiguredPets());
            }
        }

        private static IEnumerable<PetObject> GetPreconfiguredPets()
        {
            return new List<PetObject>()
            {
                new PetObject()
                {
                      Id = "607b013bb9efc807d84e5a80",
                      PetName = "Racoon",
                      Ability = 3,
                      AbilityMulti = 1.3,
                      PetGrade = "C",
                      Level = 1,
                      MaxStar = 2,
                      Stamina = 3,
                      Exp = 0,
                      Agility = 4
                },
                new PetObject()
                {
                      Id = "607b013bb9efc807d84e5a97",
                      PetName = "WhiteBear",
                      Ability = 1,
                      AbilityMulti = 1.7,
                      PetGrade = "A",
                      Level = 1,
                      MaxStar = 4,
                      Stamina = 3,
                      Exp = 0,
                      Agility = 6
                },
                new PetObject()
                {
                      Id = "607b013bb9efc807d84e5a84",
                      PetName = "Tiger",
                      Ability = 2,
                      AbilityMulti = 1.5,
                      PetGrade = "B",
                      Level = 1,
                      MaxStar = 3,
                      Stamina = 3,
                      Exp = 0,
                      Agility = 5
                }
            };
                
            }
        }
    }

