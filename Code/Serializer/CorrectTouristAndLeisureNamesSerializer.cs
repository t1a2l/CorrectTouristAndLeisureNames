using System;
using ICities;
using UnityEngine;

namespace CorrectTouristAndLeisureNames.Serializer
{
    public class CorrectTouristAndLeisureNamesSerializer : ISerializableDataExtension
    {
        // Some magic values to check we are line up correctly on the tuple boundaries
        private const uint uiTUPLE_START = 0xFEFEFEFE;
        private const uint uiTUPLE_END = 0xFAFAFAFA;

        public const ushort DataVersion = 1;
        public const string DataID = "CorrectTouristAndLeisureNamesSerializer";

        public static CorrectTouristAndLeisureNamesSerializer instance = null;
        private ISerializableData m_serializableData = null;

        public void OnCreated(ISerializableData serializedData)
        {
            instance = this;
            m_serializableData = serializedData;
        }

        public void OnLoadData()
        {
            try
            {
                if (m_serializableData != null)
                {
                    byte[] Data = m_serializableData.LoadData(DataID);
                    if (Data != null && Data.Length > 0)
                    {
                        ushort SaveGameFileVersion;
                        int Index = 0;

                        SaveGameFileVersion = StorageData.ReadUInt16(Data, ref Index);

                        Debug.Log("Data length: " + Data.Length.ToString() + "; Data Version: " + SaveGameFileVersion);

                        if (SaveGameFileVersion <= DataVersion)
                        {
                            while (Index < Data.Length)
                            {
                                CheckStartTuple("RestaurantSerializer", SaveGameFileVersion, Data, ref Index);
                                HotelNamesSerializer.LoadData(SaveGameFileVersion, Data, ref Index);
                                CheckEndTuple("RestaurantSerializer", SaveGameFileVersion, Data, ref Index);
                                break;
                            }
                        }
                        else
                        {
                            string sMessage = "This saved game was saved with a newer version of Correct Tourist And Leisure Names Mod.\r\n";
                            sMessage += "\r\n";
                            sMessage += "Unable to load settings.\r\n";
                            sMessage += "\r\n";
                            sMessage += "Saved game data version: " + SaveGameFileVersion + "\r\n";
                            sMessage += "MOD data version: " + DataVersion + "\r\n";
                            Debug.Log(sMessage);
                        }
                    }
                    else
                    {
                        Debug.Log("Data is null");
                    }
                }
                else
                {
                    Debug.Log("m_serializableData is null");
                }
            }
            catch (Exception ex)
            {
                string sErrorMessage = "Loading of Correct Tourist And Leisure Names Mod save game settings failed with the following error:\r\n";
                sErrorMessage += "\r\n";
                sErrorMessage += ex.Message;
                Debug.LogError(sErrorMessage);
            }
        }

        public void OnSaveData()
        {
            Debug.Log("OnSaveData - Start");
            try
            {
                if (m_serializableData != null)
                {
                    FastList<byte> Data = new();
                    // Always write out data version first
                    StorageData.WriteUInt16(DataVersion, Data);

                    StorageData.WriteUInt32(uiTUPLE_START, Data);
                    HotelNamesSerializer.SaveData(Data);
                    StorageData.WriteUInt32(uiTUPLE_END, Data);

                    m_serializableData.SaveData(DataID, Data.ToArray());
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Could not save data. " + ex.Message);
            }
            Debug.Log("OnSaveData - Finish");
        }

        private void CheckStartTuple(string sTupleLocation, int iDataVersion, byte[] Data, ref int iIndex)
        {
            if (iDataVersion >= 1)
            {
                uint iTupleStart = StorageData.ReadUInt32(Data, ref iIndex);
                if (iTupleStart != uiTUPLE_START)
                {
                    throw new Exception($"Start tuple not found at: {sTupleLocation}");
                }
            }
        }

        private void CheckEndTuple(string sTupleLocation, int iDataVersion, byte[] Data, ref int iIndex)
        {
            if (iDataVersion >= 1)
            {
                uint iTupleEnd = StorageData.ReadUInt32(Data, ref iIndex);
                if (iTupleEnd != uiTUPLE_END)
                {
                    throw new Exception($"End tuple not found at: {sTupleLocation}");
                }
            }
        }

        public void OnReleased()
        {
            instance = null;
        }

    }
}