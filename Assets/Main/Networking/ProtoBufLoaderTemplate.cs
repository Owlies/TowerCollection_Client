using UnityEngine;
using System.IO;

public static class ProtoBufLoaderTemplate {

    private static ProtoBufDataSerializerTemplate m_serializer = new ProtoBufDataSerializerTemplate();

    public static T LoadObjectFromResources<T>(string resourcePath) {
        TextAsset objectAsset = Resources.Load(resourcePath, typeof(TextAsset)) as TextAsset;

        if (objectAsset == null) {
            return default(T);
        }

        T deserializedObject = default(T);

        using (MemoryStream m = new MemoryStream(objectAsset.bytes)) {
            deserializedObject = (T)m_serializer.Deserialize(m, null, typeof(T));
        }

        return deserializedObject;
    }

    public static T LoadObjectFromPath<T>(string path) {
        if (!File.Exists(path)) {
            return default(T);
        }

        T deserializedObject = default(T);

        using (FileStream f = new FileStream(path, FileMode.Open)) {
            deserializedObject = (T)m_serializer.Deserialize(f, null, typeof(T));    
        }

        return deserializedObject;
    }

    public static void saveObjectToPath<T>(string objectPath, string fileName, T serializeObject) {
        if (!Directory.Exists(objectPath)) {
            Directory.CreateDirectory(objectPath);
        }

        using (FileStream f = new FileStream(objectPath + fileName, FileMode.OpenOrCreate)) {
            m_serializer.Serialize(f, serializeObject);
        }
    }

    public static byte[] serializeProtoObject<T>(T obj) {
        using (MemoryStream m = new MemoryStream()) {
            m_serializer.Serialize(m, obj);
            return m.ToArray();
        }
    }

    public static T deserializeProtoObject<T>(byte[] bytes) {
        using (MemoryStream m = new MemoryStream(bytes)) {
            return (T)m_serializer.Deserialize(m, null, typeof(T));
        }
    }
}
