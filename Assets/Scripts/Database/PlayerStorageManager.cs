using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections;

public class PlayerStorageManager : MonoBehaviour {

    static string connectionString;

    private int PLAYER_DATABASE_VERSION_NUMBER = 5;

    void Awake() {
        if (Application.platform == RuntimePlatform.Android) {
            connectionString = Application.persistentDataPath + "/player_assets.sqlite";
            StartCoroutine(FileCheck());
        } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
            connectionString = Application.persistentDataPath + "/player_assets.sqlite";
            StartCoroutine(FileCheck());
        } else {
            connectionString = "URI=file:" + Application.dataPath + "/Database/player_assets.sqlite";
        }
    }

    public IEnumerator FileCheck() {
        int installedDBVersion = PlayerPrefs.GetInt("PLAYER_DATABASE_VERSION_NUMBER", 0);
        if (installedDBVersion != PLAYER_DATABASE_VERSION_NUMBER) {
            byte[] data = Resources.Load<TextAsset>("player_assets").bytes;
            File.WriteAllBytes(connectionString, data);
            PlayerPrefs.SetInt("PLAYER_DATABASE_VERSION_NUMBER", PLAYER_DATABASE_VERSION_NUMBER);
        }
        connectionString = "URI=file:" + connectionString;
        yield return null;
    }

    public void StartBTN() {
        PlayerPrefs.DeleteAll();
        StartCoroutine(ResetDatabaseLoading());
        StartCoroutine(ResetDatabase());
    }

    //AD Obtain
    public void UpdateADObtainTime(int ADID, int Counter, string startTime) {
        string sqlQuary = String.Format("UPDATE ADObtain SET Counter = {1}, ADGotTime = \"{2}\" where ADID = {0};", ADID, Counter, startTime);
        SQLInput(sqlQuary);
    }

    public List<ADObtain> GetADObtainTime() {
        List<ADObtain> aDObtains = new List<ADObtain>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM ADObtain;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        aDObtains.Add(new ADObtain(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return aDObtains;
    }


    //AD Process
    public void UpdateADtime(int ADID, string startTime) {
        string sqlQuary = String.Format("UPDATE ADProcess SET ProcessTime = \"{1}\" where ADID = {0};", ADID, startTime);
        SQLInput(sqlQuary);
    }

    public string GetADtime(int ADID) {
        string ADtime = "";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT ProcessTime FROM ADProcess where ADID = {0};", ADID);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    ADtime = reader.GetString(0);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return ADtime;
    }

    public void InsertNew(int ID) { // including count not just one and update if that is already exist 
        string sqlQuary = String.Format(
            "INSERT INTO Inventory(ItemID, ItemCount) " +
            "VALUES ({0}, 1);",
            ID
        );
        SQLInput(sqlQuary);
    }

    // CoinInfos
    public int GetCoinFromDB() {
        int coin = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM PlayerProfile;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    coin = reader.GetInt32(1);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return coin;
    }
    public int UpdateNewCoin(int coin) {
        string sqlQuary = String.Format("UPDATE PlayerProfile SET Coin = {0};", coin);
        SQLInput(sqlQuary);
        return coin;
    }

    // PlayerData
    public PlayerData GetPlayerData() {
        PlayerData playerData;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM PlayerProfile;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    playerData = new PlayerData(reader.GetString(0),
                            reader.GetInt32(1), reader.GetInt32(2),
                            reader.GetInt32(3), reader.GetInt32(4),
                            reader.GetInt32(5), reader.GetInt32(6),
                            reader.GetInt32(7), reader.GetInt32(8),
                            reader.GetInt32(9), reader.GetInt32(10),
                            reader.GetInt32(11), reader.GetString(12));
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return playerData;
    }

    public void UpdatePlayerData(string s, int i) {
        string sqlQuary = String.Format("UPDATE PlayerProfile SET \"{0}\" = {1};", s, i);
        SQLInput(sqlQuary);
    }
    public void UpdatePlayerData(string s, string i) {
        string sqlQuary = String.Format("UPDATE PlayerProfile SET \"{0}\" = \"{1}\";", s, i);
        SQLInput(sqlQuary);
    }

    //Name
    public string GetNameFromDB() {
        string name = "";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM PlayerProfile;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    name = reader.GetString(0);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return name;
    }
    //Eperience
    public int GetExperienceFromDB() {
        int exp = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM PlayerProfile;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    exp = reader.GetInt32(3);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return exp;
    }
    public int UpdateExp(int exp) {
        string sqlQuary = String.Format("UPDATE PlayerProfile SET Experience = {0};", exp);
        SQLInput(sqlQuary);
        return exp;
    }

    //Expansion
    public int Expansion() {
        int expensionData;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT Expansion FROM PlayerProfile;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    expensionData = reader.GetInt32(0);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return expensionData;
    }
    public void BuyExpansion(int Level) {
        string sqlQuary = String.Format("UPDATE PlayerProfile SET Expansion = {0};", Level);
        SQLInput(sqlQuary);
    }

    //Inventory Information 
    public void InsertInventoryData(int ItemID, int ItemCount) {
        string sqlQuary = String.Format(
            "INSERT INTO Inventory (ItemID, ItemCount)" +
            "VALUES ({0},{1});",
             ItemID, ItemCount
        );
        SQLInput(sqlQuary);
    }
    public void DeleteInventoryData(int ItemID) {
        string sqlQuary = String.Format(
            "DELETE FROM Inventory " +
            "WHERE ItemID = {0};",
            ItemID
        );
        SQLInput(sqlQuary);
    }
    public void UpdateInventoryData(int ItemID, int ItemCount) {
        string sqlQuary = String.Format(
            "UPDATE Inventory " +
            "SET ItemCount = {0} " +
            "WHERE  ItemID = {1};",
            ItemCount, ItemID
        );
        SQLInput(sqlQuary);
    }

    public List<Inventory> GetInventoryFromDatabase() {
        List<Inventory> inventoryList = new List<Inventory>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM Inventory;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        inventoryList.Add(new Inventory(
                            reader.GetInt32(0), reader.GetInt32(1)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return inventoryList;
    }
    //Bulding Production Access
    public List<Production> CheckingProgressProduction() {
        List<Production> production = new List<Production>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT BlockID, ProductionProcess, ProductionItem,ProductionCount, ProductionStartTime FROM BuildingFormation;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        production.Add(new Production(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return production;
    }

    public void UpdateProductionStartingTime(string productionTime, int blockID) {
        string sqlQuary = String.Format(
            "UPDATE BuildingFormation " +
            "SET ProductionStartTime = \"{0}\" " +
            "WHERE BlockID = {1};",
             productionTime, blockID
        );
        SQLInput(sqlQuary);
    }

    public void UpdateStatusProduction(int blockID, int process, int itemID, int productionCount, string productionTime) {
        string sqlQuary = String.Format(
            "UPDATE BuildingFormation " +
            "SET ProductionProcess = {0}, " +
            "ProductionItem = {1}, ProductionStartTime = \"{2}\", ProductionCount = {4} " +
            "WHERE BlockID = {3};",
             process, itemID, productionTime, blockID, productionCount
        );
        SQLInput(sqlQuary);
    }
    public int CheckingProductionProcess(int blockID) {
        int processStatus = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT ProductionProcess FROM BuildingFormation WHERE BlockID = {0};", blockID);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    processStatus = reader.GetInt32(0);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return processStatus;
    }

    //Building Information access
    public void DeleteBuildingFromDatabase(int blockID) {
        string sqlQuary = String.Format(
            "DELETE FROM BuildingFormation " +
            "WHERE BlockID = {0};",
            blockID
        );
        SQLInput(sqlQuary);
    }

    public void UpdateBuildingStartingTime(string startingTime, int blockID) {
        string sqlQuary = String.Format(
            "UPDATE BuildingFormation " +
            "SET StartingTime = \"{0}\" " +
            "WHERE BlockID = {1};",
             startingTime, blockID
        );
        SQLInput(sqlQuary);
    }

    public void UpdateBuildingProgress(int process, int blockID) {
        string sqlQuary = String.Format(
            "UPDATE BuildingFormation " +
            "SET Process = {0} " +
            "WHERE BlockID = {1};",
             process, blockID
        );
        SQLInput(sqlQuary);
    }
    public void UpdateBuildingFormation(int newBlockID, int oldBlockID) {
        string sqlQuary = String.Format(
            "UPDATE BuildingFormation " +
            "SET BlockID = {0} " +
            "WHERE BlockID = {1};",
            newBlockID, oldBlockID
        );
        SQLInput(sqlQuary);
    }

    public void UpdateBuildingBuilding(int BlockID, int newBuildingID) {
        string sqlQuary = String.Format(
            "UPDATE BuildingFormation " +
            "SET BuildingID = {0} " +
            "WHERE BlockID = {1};",
            newBuildingID, BlockID // just change the building id data 
        );
        SQLInput(sqlQuary);
    }
    public void UpdateBuildingUpgradeData(int level, int process, string startingTime, int blockID) {
        string sqlQuary = String.Format(
            "UPDATE BuildingFormation " +
            "SET Level = {0}, Process = {1}, StartingTime = \"{2}\" " +
            "WHERE  BlockID = {3};",
            level, process, startingTime, blockID
            );
        SQLInput(sqlQuary);
    }
    public void InsertBuildingFormation(Building buildingInfo) {
        string sqlQuary = String.Format(
            "INSERT INTO BuildingFormation (BlockID, BuildingID, StartingTime, Level, Process, ProductionProcess, ProductionItem,ProductionCount, ProductionStartTime)" +
            "VALUES ({0},{1},\"{2}\",{3},{4},0,0,0,\"{2}\");",
             buildingInfo.GetBlockID(), buildingInfo.GetBuildingID(), buildingInfo.GetStartingTime(),
             buildingInfo.GetLevel(), buildingInfo.GetProcess()
        );
        SQLInput(sqlQuary);
    }
    public List<Building> GetBuildingFormationFromDatabase() {
        List<Building> buildingList = new List<Building>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT BlockID, BuildingID, StartingTime, Level, Process FROM BuildingFormation;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        buildingList.Add(new Building(
                            reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2),
                            reader.GetInt32(3), reader.GetInt32(4)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return buildingList;
    }



    //Achievement Information access
    public void InsertInitialAchievement(int ID, string startDate, int currentProgress, int maxProgress, int unlocked) {//run once only
        string sqlQuary = String.Format(
            "INSERT INTO AchievementProgress(ID, StartDate, CurrentProgress, MaxProgress, Unlocked) " +
            "VALUES ({0},\"{1}\",{2},{3},{4});",
            ID, startDate, currentProgress, maxProgress, unlocked
        );
        SQLInput(sqlQuary);
    }
    public void UpdateAchievementProgress(int ID, int currentProgress) { //checking startDate process
        string sqlQuary = String.Format(
            "UPDATE AchievementProgress " +
            "SET CurrentProgress = {0} " +
            "WHERE ID = {1};",
             currentProgress, ID
        );
        SQLInput(sqlQuary);
    }
    public int GetAchievementFromDatabase(int id) {
        int currentProgress = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT currentProgress FROM AchievementProgress where id = {0};", id);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        currentProgress = reader.GetInt32(0);
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return currentProgress;
    }

    //Login
    public string GetLoginDate() {
        string startDate = "";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM AchievementProgress;");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    startDate = reader.GetString(1);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return startDate;
    }

    public void ResetLogin(int currentProgress, int unlocked, string startDate) {
        string sqlQuary = String.Format(
            "UPDATE AchievementProgress " +
            "SET CurrentProgress = {0}, Unlocked = {1}, StartDate = \"{2}\"",
             currentProgress, unlocked, startDate
        );
        SQLInput(sqlQuary);
    }

    //Unlocked
    public int GetUnlocked(int id) {
        int unLocked = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT Unlocked FROM AchievementProgress where id = {0};", id);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        unLocked = reader.GetInt32(0);
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return unLocked;
    }
    public void UpdateUnlocked(int id, int unlocked) {
        string sqlQuary = String.Format(
            "UPDATE AchievementProgress " +
            "SET Unlocked = {0} " +
            "WHERE ID = {1};",
            unlocked, id
        );
        SQLInput(sqlQuary);
    }

    //MajorAchievement
    public int GetClaimedLevel(int id) {
        int claimedLevel = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT ClaimedLevel FROM MajorAchievement where id = {0};", id);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        claimedLevel = reader.GetInt32(0);
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return claimedLevel;
    }

    public int GetCurrentProgress(int id) {
        int currentProgress = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT CurrentProgress FROM MajorAchievement where id = {0};", id);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        currentProgress = reader.GetInt32(0);
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return currentProgress;
    }

    public void UpdateLevelAndProgress(int id, int level, int progress) {
        string sqlQuary = String.Format(
            "UPDATE MajorAchievement " +
            "SET ClaimedLevel = {0}, CurrentProgress = {1} " +
            "WHERE ID = {2};",
             level, progress, id
        );
        SQLInput(sqlQuary);
    }

    //Award
    public List<AwardObj> GetAwards() {
        List<AwardObj> Awards = new List<AwardObj>();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT * FROM AwardTable");
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        Awards.Add(new AwardObj(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return Awards;
    }

    public void InsertAward(string serialKeycode) {
        string sqlQuary = String.Format(
            "INSERT INTO AwardTable(KeyCode, Serial, Title) " +
            "VALUES (\"{0}\" ,\"_\", \"0\");",
            serialKeycode
        );
        SQLInput(sqlQuary);
    }

    public void InsertSerialTitle(string keycode, int titleID) {
        string sqlQuary = String.Format(
            "INSERT INTO AwardTable(KeyCode, Serial, Title) " +
            "VALUES (\"{0}\" ,\"_\", {1});",
            keycode, titleID
        );
        SQLInput(sqlQuary);
    }

    public void UpdateSerial(string keycode, string serial) {
        string sqlQuary = String.Format(
            "UPDATE AwardTable " +
            "SET Serial = \"{0}\"" +
            "WHERE KeyCode = \"{1}\";",
             serial, keycode
        );
        SQLInput(sqlQuary);
    }

    public string GetSerial(string serialKeycode) {
        string serial = "";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT Serial FROM AwardTable WHERE KeyCode = \"{0}\";", serialKeycode);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        //Debug.Log(reader.GetString(0));
                        serial = reader.GetString(0);
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return serial;
    }

    //Title
    public void InsertTitle(int title) {
        string sqlQuary = String.Format(
            "INSERT INTO AwardTable(KeyCode, Serial, Title) " +
            "VALUES (\"0\", \"0\", {0});",
            title
        );
        SQLInput(sqlQuary);
    }

    //Technical
    public int GetTechnicalItem(string type) {
        int TechnicalItem;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuary = String.Format("SELECT \"{0}\" FROM PlayerProfile", type);
                dbCmd.CommandText = sqlQuary;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    reader.Read();
                    TechnicalItem = reader.GetInt32(0);
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        return TechnicalItem;
    }
    public int UpdateTechnicalItem(int type) {
        string sqlQuary = String.Format("UPDATE PlayerProfile SET Improvement = {0};", type);
        SQLInput(sqlQuary);
        return type;
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


    IEnumerator ResetDatabaseLoading() {
        while (restarting) {
            Debug.Log("Restarting in progress ... ");
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Restart Done");
        yield return null;
    }

    bool restarting = true;
    IEnumerator ResetDatabase() {
        restarting = true;
        string sqlQuary = String.Format("UPDATE ADObtain SET Counter = 0, ADGotTime =\"1 / 1 / 2020 1:0:0\"; ");
        SQLInput(sqlQuary);
        sqlQuary = String.Format("UPDATE ADProcess SET ProcessTime =\"1 / 1 / 2020 1:0:0\"; ");
        SQLInput(sqlQuary);
        sqlQuary = String.Format("UPDATE AchievementProgress SET StartDate = \"1 / 1 / 2020 1:0:0\", CurrentProgress = 0, Unlocked = 0; ");
        SQLInput(sqlQuary);
        sqlQuary = String.Format("DELETE FROM BuildingFormation; ");
        SQLInput(sqlQuary);
        sqlQuary = String.Format("DELETE FROM AwardTable; ");
        SQLInput(sqlQuary);
        ResetInv();
        sqlQuary = String.Format("UPDATE MajorAchievement SET ClaimedLevel = 0, CurrentProgress = 0; ");
        SQLInput(sqlQuary);
        sqlQuary = String.Format("UPDATE PlayerProfile SET Coin = 1000, Level = 1, Experience = 0, Title = 0, Expansion = 0, Improvement =0, Quality =0, Evolution= 0, Product = 0, CurrentUpgrade = 0; ");
        SQLInput(sqlQuary);
        restarting = false;
        yield return null;
    }
    public void ResetInv() {
        string sqlQuary = String.Format("DELETE FROM Inventory; ");
        SQLInput(sqlQuary);
        for (int i = 1; i < 24; i++) {
            sqlQuary = String.Format("INSERT INTO Inventory(ItemID, ItemCount) VALUES ({0}, 5);", i);
            SQLInput(sqlQuary);
        }
    }
}


