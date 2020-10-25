using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class GoogleSheetsDatabase : ScriptableObject
    {
        [SerializeField, ShowInInspector, ReadOnly, DisplayAsString, HideLabel]
        public string Id;
        [ReadOnly, ListDrawerSettings(Expanded = true), LabelText("Tracked Objects")]
        public List<ScriptableObject> Objects = new List<ScriptableObject>();
    }
}