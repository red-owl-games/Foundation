using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RedOwl.Core
{
    [Serializable]
    public struct SaveGameItem
    {
        public string key;
        public byte[] value;
    }
    
    [Serializable]
    public class SaveGameDatafile : IRedOwlFile
    {
        [SerializeField] private List<SaveGameItem> data;
        
        private Dictionary<string, byte[]> Data { get; set; } = new Dictionary<string, byte[]>();

        public void OnBeforeSerialize()
        {
            data = new List<SaveGameItem>(Data.Count);
            foreach (var kvp in Data)
            {
                data.Add(new SaveGameItem{ key = kvp.Key, value = kvp.Value });
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
        
        public void Pull(ISaveData value)
        {
            if (!Data.TryGetValue(value.SaveDataId, out var bytes)) return;
            using (var stream = new MemoryStream(bytes, 0, bytes.Length))
            {
                using (var reader = new SaveReader(stream))
                {
                    value.LoadData(reader);
                }
            }
        }
        
        public void Push(ISaveData value)
        {
            using (var stream = new MemoryStream(value.SaveDataLength))
            {
                using (var writer = new SaveWriter(stream))
                {
                    value.SaveData(writer);
                    Data[value.SaveDataId] = stream.GetBuffer();
                }
            }
        }
    }
}