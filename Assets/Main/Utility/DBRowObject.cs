using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Text;
using UnityEngine;

public class DBRowObject
{
    private IDbConnection dbconn;

    // Use this for initialization
    public void Init () {
        string conn = "URI=file:" + Application.dataPath + "/Resources/Data/TowerCollectionDataBase.s3db"; //Path to database.
        this.dbconn = (IDbConnection)new SqliteConnection(conn);
    }

    public List<T> LoadList<T>(string tableName) where T : class, new()
    {
        this.dbconn.Open();
        List<T> lists = new List<T>();
        Dictionary<string, object> row = new Dictionary<string,object>();
        List<string> columnNames = new List<string>();

        IDbCommand dbcmd = this.dbconn.CreateCommand();
        
        string sqlQuery = "PRAGMA table_info(" + tableName + ")";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
            columnNames.Add(reader.GetValue(1).ToString());
        reader.Close();

        //dbcmd = this.dbconn.CreateCommand();
        sqlQuery = "SELECT * FROM " + tableName;
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; ++i)
                row[columnNames[i]] = reader.GetValue(i);

            Type type = typeof(T);
            T result = new T();
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (row.ContainsKey(field.Name))
                {
                    object value = row[field.Name];
                    if (row[field.Name].GetType() == typeof(byte[]))
                    {
                        Type valueType;
                        if (row.ContainsKey(field.Name + "Type"))
                        {
                            string valueTypeName = row[field.Name + "Type"] as string;
                            if (valueTypeName.Equals(""))
                                valueType = field.FieldType;
                            else
                                valueType = Type.GetType(valueTypeName);
                        }
                        else
                            valueType = field.FieldType;
                        value = Encoding.Default.GetString(value as byte[]);
                        value = Util.Deserialize(valueType, value as string);
                    }
                    else if (field.FieldType.IsArray)
                    {
                        value = Util.Deserialize(field.FieldType, value as string);
                    }
                    else if (field.FieldType == typeof(float))
                        value = Convert.ToSingle(row[field.Name]) / 100.0f;
                    else if (field.FieldType == typeof(int))
                        value = Convert.ToInt32(row[field.Name]);
                    else if (field.FieldType.IsEnum)
                        value = Enum.ToObject(field.FieldType, value);
                    else
                        value = row[field.Name];
                    if (field.FieldType.IsClass)
                        field.SetValue(result, value);
                    else
                        field.SetValue(result, Convert.ChangeType(value,field.FieldType));
                }
            }

            lists.Add(result);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        this.dbconn.Close();

        return lists;
    }
}
