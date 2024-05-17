using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace UnityHelper
{
    public static class SerializationUtility
    {
        private static string FilePath => $"{Application.persistentDataPath}/Save.txt";

        public static void Save<T>(T data)
        {
            using (FileStream stream = File.Open(FilePath, FileMode.Create))
            {
                BinaryFormatter formater = new BinaryFormatter();
                formater.Serialize(stream, data);
            }
        }

        public static T Load<T>()
        {
            if (!File.Exists(FilePath))
            {
                Debug.LogError("File not found: " + FilePath);
                return default;
            }

            using (FileStream stream = File.Open(FilePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
