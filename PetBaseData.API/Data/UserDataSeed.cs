using MongoDB.Driver;
using PetBaseData.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Data
{
    public class UserDataSeed
    {
        public static void SeedData(IMongoCollection<UserData> userDataCollection)
        {
            bool existPet = userDataCollection.Find(p => true).Any();
            if (!existPet)
            {
                userDataCollection.InsertOneAsync(CreateUserData());
            }
        }

        private static UserData CreateUserData()
        {
            UserData newUserData = new()
            {
                Username = "",
                Password = "",
                SaltValue = "",
                SavedDataId = ""
            };
            return newUserData;
        }

        public static SavedData GetDefaultSavedData()
        {
            SavedData newSavedData = new()
            {


                UiTransition = false,

                PlayerLevel = 1,
                PlayerXP = 0,
                StonePerHour = 0.0,
                WoodPerHour = 0.0,
                CoinPerHour = 0.0,
                AdventureRevive = false,

                //Resource Variables
                WoodCount = 100,
                StoneCount = 100,
                CoinCount = 100,
                EggCount = 5,
                MaxFood = 5,
                CurrentFood = 5,

                //Building Variables
                UnlockedRegions = new List<string>(),
                ConstructedBuilding = new List<BuildingObject>(),

                //Pet Variables
                ListOfPets = new List<PetObject>(),
                StarterPetList = new List<PetObject>(),
                PlayerPetList = new List<PetObject>(),
                AdventurePet = null,
                IncubatingPets = new List<IncubationObject>(),
                PetCap = 1,
                HatchModifier = 0.0,

                //Adventure Variables
                AdventureType = 0,
                DropModifier = 0.0,
            };
            return newSavedData;
        }
    }
}

