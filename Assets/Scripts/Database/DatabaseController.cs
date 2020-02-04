using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DatabaseController {

    public static SqliteConnection dbConnection;
    public static readonly string dbpath = Path.Combine(Application.streamingAssetsPath,  "Game_config.sqlite");
    #region Connection
    public static void Connection()
    {
        try
        {
            if (File.Exists(dbpath))
            {
                dbConnection = new SqliteConnection("Data Source = " + dbpath + "; " + " Version = 3;");
                dbConnection.Open();
            }
            else
            {
                Debug.LogError("Connection-Error:\nFile not found!\n" + dbpath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("\nConnection-Error:\n" + e.ToString());
        }
    }
    #endregion

    #region SelectMap
    public static string[] SelectMap(string number)
    {
        try
        {
            List<string> map = new List<string>();
            string commandstring = "SELECT * FROM Map" + number + ";";
            SqliteCommand command = new SqliteCommand(commandstring, dbConnection);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string data = reader["X_Row"].ToString();
                map.Add(data);
            }
            return map.ToArray();
        }
        catch (Exception e)
        {
            Debug.Log("Select-Error:\n" + e.ToString());
        }
        return null;
    }
    #endregion

    #region Generic Select Table
    public static List<string> ReadAttributesFromTable(string table, string type)
    {
        try
        {
            List<string> data = new List<string>();
            string commandstring = "Select * FROM " + table + " WHERE Typ = '" + type + "';";

            SqliteCommand command = new SqliteCommand(commandstring, dbConnection);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    data.Insert(i, reader[i].ToString());
                }
            }
            return data;
        }
        catch (Exception e)
        {

            Debug.LogError("Selecting Error with table: " + table + ". \n" + e.ToString());
            return null;
        }
    }
    #endregion

    #region Exit
    public static void Exit()
    {
        try
        {
            if (dbConnection.State == System.Data.ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Object reference not set to an instance of an object"))
            {
                Debug.LogError("No connection to close");
            }
            else
            {
                Debug.LogError("Error:\n" + e.ToString());
            }
        }
    }
    #endregion
}
