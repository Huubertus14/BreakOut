﻿using System.IO;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

public class SaveData
{

    private static string saveName = "gamesave";
    private static bool saving = false;

    private static PlayerData loadedData;

    public static PlayerData LoadData()
    {
        Debug.Log("path: "+Application.persistentDataPath);
        saving = false;
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            //  Debug.Log("Try to load from GP");
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(saveName, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }
        if (loadedData == null)
        {
            return LoadLocal();
        }
        return loadedData;
    }

    private static string SaveGameName => Application.persistentDataPath + "/gamesave.save";


    private static PlayerData LoadLocal()
    {
        // Debug.Log("Load local");

        if (File.Exists(SaveGameName))
        {
            try
            {
                FileStream file = new FileStream(SaveGameName, FileMode.Open);
                if (file.Length > 0)
                {
                    // Debug.Log("File exist");
                    BinaryFormatter bf = new BinaryFormatter();
                    PlayerData save = (PlayerData)bf.Deserialize(file);
                    file.Close();
                    return save;
                }
                else
                {
                    Debug.Log("Create new Save File");
                    return CreateNewSaveFile();
                }
            }
            catch (System.Exception e)
            {
                
                Debug.LogError("Failed to load data " + e);
                throw;
            }
        }
        else
        {
            Debug.Log("Create new Save File");
            return CreateNewSaveFile();
        }
    }

    private static PlayerData CreateNewSaveFile()
    {
        // Debug.Log("File does not exist, creating a new one");
        PlayerData save = new PlayerData();
        save.ResetData();
        return save; //First boot ever
    }

    private static void SaveLocal(PlayerData save)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(SaveGameName, FileMode.Create, FileAccess.Write);
        bf.Serialize(file, save);
        file.Close();
    }

    public static void Save(PlayerData save)
    {
        if (save == null)
        {
            //Debug.LogWarning("Save is empty");
            return;
        }
        loadedData = save;
        SaveLocal(save);//Always save a local game
        saving = true;
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            //Save on Googe play
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(saveName, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }
        else
        {
            //Debug.Log("Not authenticated GP");
        }
    }

    public static void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        //Debug.Log("SaveOpen");
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            if (saving) //Save the game
            {
                // Debug.Log("SaveGP");
                SaveGooglePlay(game, ObjectSerializationExtension.SerializeToByteArray(loadedData), TimeSpan.FromMinutes(0));
            }
            else //load the game
            {
                // Debug.Log("LoadGP");
                LoadFromGooglePlay(game);
            }
        }
        else
        {
            // handle error
            //Debug.LogWarning("Could not open save game");
            //Always save local, so ony loaded needs to be done here
            if (!saving)
            {
                LoadLocal();
            }
        }
    }

    private static void SaveGooglePlay(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        //Debug.Log("SavingGP");
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);

        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    private static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        // Debug.Log("DoneSavingGP: " + status);
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
        }
        else
        {
            // handle error
        }
    }

    private static void LoadFromGooglePlay(ISavedGameMetadata game)
    {
        // Debug.Log("LoadfromGP");
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    private static void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        //Debug.Log("DoneLoading GP: " +status);
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
            loadedData = ObjectSerializationExtension.Deserialize<PlayerData>(data);
        }
        else
        {
            // handle error
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerHighScore;
    public int playerTotalScore;

    public int playerGold;
    public int playerPremiumCurrency;

    public void ResetData()
    {
        playerHighScore = 0;
        playerTotalScore = 0;
        playerGold = 0;
        playerPremiumCurrency = 0;
    }


    public override string ToString()
    {
        base.ToString();
        return "";
    }
}

//Extension class to provide serialize / deserialize methods to object.
//src: http://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
//NOTE: You need add [Serializable] attribute in your class to enable serialization
public static class ObjectSerializationExtension
{
    public static byte[] SerializeToByteArray(this object obj)
    {
        if (obj == null)
        {
            return null;
        }
        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static T Deserialize<T>(this byte[] byteArray) where T : class
    {
        if (byteArray == null)
        {
            return null;
        }
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(byteArray, 0, byteArray.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (T)binForm.Deserialize(memStream);
            return obj;
        }
    }
}