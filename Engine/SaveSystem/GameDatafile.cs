using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RedOwl.Core
{
    [Serializable]
    public struct PersistenceItem
    {
        public string key;
        public byte[] value;
    }
    
    [Serializable]
    public class GameDatafile : IRedOwlFile
    {
        [SerializeField] private List<PersistenceItem> data;
        
        private Dictionary<string, byte[]> Data { get; set; } = new Dictionary<string, byte[]>();

        public void OnBeforeSerialize()
        {
            data = new List<PersistenceItem>(Data.Count);
            foreach (var kvp in Data)
            {
                data.Add(new PersistenceItem{ key = kvp.Key, value = kvp.Value });
            }
        }

        public void OnAfterDeserialize()
        {
            Data = new Dictionary<string, byte[]>(data.Count);
            foreach (var item in data)
            {
                Data.Add(item.key, item.value);
            }
        }
        
        public void Pull(IPersistData value)
        {
            if (!Data.TryGetValue(value.SaveDataId, out var bytes)) return;
            using (var stream = new MemoryStream(bytes, 0, bytes.Length))
            {
                using (var reader = new PersistenceReader(stream))
                {
                    value.LoadData(reader);
                }
            }
        }
        
        public void Push(IPersistData value)
        {
            using (var stream = new MemoryStream(value.SaveDataLength))
            {
                using (var writer = new PersistenceWriter(stream))
                {
                    value.SaveData(writer);
                    Data[value.SaveDataId] = stream.GetBuffer();
                }
            }
        }
    }
}