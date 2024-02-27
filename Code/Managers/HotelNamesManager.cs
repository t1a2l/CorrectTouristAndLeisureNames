using System;
using System.Collections.Generic;

namespace CorrectTouristAndLeisureNames.Managers
{
    public static class HotelNamesManager
    {
        public static Dictionary<ushort, string> HotelNames;

        public static string[] hotel_names = ["Hotel Hiya", "Grand Hotel", "Night Inn", "Fiesta Hotel", "Your Choice Hotel", 
            "Hotel Intercontinental", "Crest Resorts", "Best Eastern", "Stratus Hotel", "Almost Five Star Hotel", "Octahotel", "Hotel Beacon"];

        public static void Init()
        {
            if (HotelNames == null)
            {
                HotelNames = [];
            }
        }

        public static void Deinit()
        {
            HotelNames = [];
        }

        public static string GetHotelName(ushort buildingId)
        {
            if (!HotelNames.TryGetValue(buildingId, out string name))
            {
                Random random = new();
                int index = random.Next(hotel_names.Length);
                name = hotel_names[index];
                HotelNames.Add(buildingId, name);
            }
            return name;
        }

        public static void SetHotelName(ushort buildingId, string name)
        {
            HotelNames[buildingId] = name;
        }

        public static void RemoveHotelName(ushort buildingId)
        {
            HotelNames.Remove(buildingId);
        }
    }

}