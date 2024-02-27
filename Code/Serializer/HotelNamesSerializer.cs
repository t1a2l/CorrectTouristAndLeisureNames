using System;
using System.Collections.Generic;
using CorrectTouristAndLeisureNames.Managers;
using UnityEngine;

namespace CorrectTouristAndLeisureNames.Serializer
{
    public class HotelNamesSerializer
    {
        // Some magic values to check we are line up correctly on the tuple boundaries
        private const uint uiTUPLE_START = 0xFEFEFEFE;
        private const uint uiTUPLE_END = 0xFAFAFAFA;

        private const ushort iHOTEL_NAMES_DATA_VERSION = 1;

        public static void SaveData(FastList<byte> Data)
        {
            // Write out metadata
            StorageData.WriteUInt16(iHOTEL_NAMES_DATA_VERSION, Data);
            StorageData.WriteInt32(HotelNamesManager.HotelNames.Count, Data);

            // Write out each buffer settings
            foreach (KeyValuePair<ushort, string> kvp in HotelNamesManager.HotelNames)
            {
                // Write start tuple
                StorageData.WriteUInt32(uiTUPLE_START, Data);

                // Write actual settings
                StorageData.WriteUInt16(kvp.Key, Data);
                StorageData.WriteString(kvp.Value, Data);

                // Write end tuple
                StorageData.WriteUInt32(uiTUPLE_END, Data);
            }
        }

        public static void LoadData(int iGlobalVersion, byte[] Data, ref int iIndex)
        {
            if (Data != null && Data.Length > iIndex)
            {
                int iHotelNamesVersion = StorageData.ReadUInt16(Data, ref iIndex);
                Debug.Log("Global: " + iGlobalVersion + " BufferVersion: " + iHotelNamesVersion + " DataLength: " + Data.Length + " Index: " + iIndex);
                HotelNamesManager.HotelNames ??= [];
                var HotelNames_Count = StorageData.ReadInt32(Data, ref iIndex);
                if (iHotelNamesVersion >= iHOTEL_NAMES_DATA_VERSION)
                {
                    for (int i = 0; i < HotelNames_Count; i++)
                    {
                        CheckStartTuple($"Buffer({i})", iHotelNamesVersion, Data, ref iIndex);
                        ushort buildingId = StorageData.ReadUInt16(Data, ref iIndex);
                        string hotel_name = StorageData.ReadString(Data, ref iIndex);
                        HotelNamesManager.HotelNames.Add(buildingId, hotel_name);
                        CheckEndTuple($"Buffer({i})", iHotelNamesVersion, Data, ref iIndex);
                    }
                }
            }
        }

        private static void CheckStartTuple(string sTupleLocation, int iDataVersion, byte[] Data, ref int iIndex)
        {
            if (iDataVersion >= 1)
            {
                uint iTupleStart = StorageData.ReadUInt32(Data, ref iIndex);
                if (iTupleStart != uiTUPLE_START)
                {
                    throw new Exception($"Buffer start tuple not found at: {sTupleLocation}");
                }
            }
        }

        private static void CheckEndTuple(string sTupleLocation, int iDataVersion, byte[] Data, ref int iIndex)
        {
            if (iDataVersion >= 1)
            {
                uint iTupleEnd = StorageData.ReadUInt32(Data, ref iIndex);
                if (iTupleEnd != uiTUPLE_END)
                {
                    throw new Exception($"Buffer end tuple not found at: {sTupleLocation}");
                }
            }
        }

    }
}