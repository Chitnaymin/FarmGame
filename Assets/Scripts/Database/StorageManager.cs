using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections;

public class StorageManager : MonoBehaviour {

    public List<GameObject> managers;
    static string connectionString;

    private int DATABASE_VERSION_NUMBER = 7;

    void Awake() {
        if (Application.platform == RuntimePlatform.Android) {
            connectionString = Application.persistentDataPath + "/assets.sqlite";
            StartCoroutine(FileCheck());
        } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
            connectionString = Application.persistentDataPath +  "/assets.sqlite";
            StartCoroutine(FileCheck());
        } else {
            connectionString = "URI=file:" + Application.dataPath + "/Database/assets.sqlite";
            for (int i = 0; i < managers.Count; i++) {
                managers[i].SetActive(true);
            }
        }
    }
    public IEnumerator FileCheck() {
        int installedDBVersion = PlayerPrefs.GetInt("DATABASE_VERSION_NUMBER", 0);
        if (installedDBVersion != DATABASE_VERSION_NUMBER) {
            byte[] data = Resources.Load<TextAsset>("assets").bytes;
            File.WriteAllBytes(connectionString, data);
            PlayerPrefs.SetInt("DATABASE_VERSION_NUMBER", DATABASE_VERSION_NUMBER);
        }
        connectionString = "URI=file:" + connectionString;
        for (int i = 0; i < managers.Count; i++) {
            managers[i].SetActive(true);
        }
        yield return null;
    }


    //Production
    public List<int> GetProductableItems(int id) {
        List<int> ProductableItem = new List<int>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT InventoryID FROM Production where BuildingID = {0};", id);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        ProductableItem.Add(reader.GetInt32(0));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return ProductableItem;
    }

    public List<DependentItem> GetItemDependencyList(int id) {
        List<DependentItem> ItemDep = new List<DependentItem>(); ;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT RequireID, RequireItemCount FROM ItemDependency where ItemId = {0};", id);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        ItemDep.Add(new DependentItem(reader.GetInt32(0), reader.GetInt32(1)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return ItemDep;
    }

    //LevelXpGraph
    public List<LevelXPGraph> GetLevelXpGraphFromDB() {
        List<LevelXPGraph> levelList = new List<LevelXPGraph>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM	LevelXPGraph;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        levelList.Add(new LevelXPGraph(reader.GetInt32(0), reader.GetInt32(1)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return levelList;
    }

    //Language
    public List<TextID> LanguageData(String lang) {
        List<TextID> LanguageString = new List<TextID>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT ID, {0} FROM Languages", lang);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        LanguageString.Add(new TextID(reader.GetInt32(0), reader.GetString(1)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return LanguageString;
    }

    // private function for reuse
    void SQLInput(string sql) {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                dbCmd.CommandText = sql;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
}


