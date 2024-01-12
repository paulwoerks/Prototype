using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using PocketHeroes.Serialization;

namespace PocketHeroes.SaveSystem
{
    /// <summary>
    /// Local save and load system
    /// </summary>
    public class LocalSaveSystem
    {
        static readonly string subfolder = "fileio";
        static readonly string fileExtention = ".io";

        #region Public
        /// <summary>
        /// Saves data as a local file. Example Use: 'Save(playerHealth)' => '.../fileio/playerHealth.io'
        /// </summary>
        public static void Save<T>(T data, bool debug = false)
        {
            Save(data, data.ToString(), debug);
        }

        /// <summary>
        /// Saves a File locally with custom fileName. Example Use: 'Save(playerHealth, "healthOfPlayer")' => '.../fileio/healthOfPlayer.io'
        /// </summary>
        public static void Save<T>(T data, string key, bool debug = false)
        {
            string path = $"{Application.persistentDataPath}/{subfolder}";
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += $"/{key.ToLower()}{fileExtention}";
            FileStream stream = new(path, FileMode.Create);
            GetBinaryFormatter().Serialize(stream, data);
            stream.Close();

            if (debug)
            {
                Debug.Log($"Save '{key}' at:   '{path}'");
            }
        }

        /// <summary>
        /// Loads data by type. Only works with data types that are just used once throughout the game. Example Use: 'PlayerHealth health = (PlayerHealth)Load(health)'
        /// </summary>
        public static T Load<T>(T data, bool debug = false)
        {
            return (T)Load(data, data.ToString().ToLower(), debug);
        }

        /// <summary>
        /// Loads an Object locally by fileName. Example Use: 'PlayerHealth health = (PlayerHealth)Load(health, "healthOfPlayer")'
        /// </summary>
        public static T Load<T>(T data, string key, bool debug = false)
        {
            string path = $"{Application.persistentDataPath}/{subfolder}/{key}.{fileExtention}";

            bool hasSavedFile = File.Exists(path);
            
            if (!hasSavedFile)
            {
                if (debug)
                {
                    Debug.LogWarning($"[!]Load '{key}': no file found at: '{path}'. Returning input value");
                }
                return data;
            }

            FileStream stream = new(path, FileMode.Open);
            data = (T)GetBinaryFormatter().Deserialize(stream);
            stream.Close();

            if (debug)
            {
                Debug.Log($"Load '{key}' from: '{path}'");
            }

            return data;
        }
        #endregion

        // REFERENCE:
        // Game Dev Guide - How to Build a Save System in Unity: https://www.youtube.com/watch?v=5roZtuqZyuw (06:10)
        static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new();
            SurrogateSelector selector = new();
            
            // Custom DataTypes
            SurrogateVector2 vector2 = new();
            selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2);

            SurrogateVector3 vector3 = new();
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3);

            SurrogateVector4 vector4 = new();
            selector.AddSurrogate(typeof(Vector4), new StreamingContext(StreamingContextStates.All), vector4);

            SurrogateQuaternion quaternion = new();
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternion);

            SurrogateColor color = new();
            selector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), color);

            SurrogateDateTime dateTime = new();
            selector.AddSurrogate(typeof(DateTime), new StreamingContext(StreamingContextStates.All), dateTime);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}