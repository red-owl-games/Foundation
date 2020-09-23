using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace RedOwl.Core
{
    [Serializable]
    public class GoogleSheetsRegistration
    {
        [HorizontalGroup("Main")]
        [OnValueChanged("Poll")]
        [LabelWidth(45)]
        public string Id;

        [HorizontalGroup("More")]
        [OnValueChanged("SheetTypeChanged")]
        [ShowIf("HasId"), LabelText("Type"), LabelWidth(45)]
        public SheetTypes SheetType;

        [HorizontalGroup("More")]
        [ShowIf("HasId"), ValueDropdown("PossibleClasses"), LabelWidth(70)]
        public string DataClass;
        
        [ShowIf("HasId"), FolderPath(ParentFolder = "Assets"), LabelWidth(45)]
        public string Folder;

        [HorizontalGroup("PerRow")]
        [ShowIf("HasId"), ShowIf("SheetType", SheetTypes.AssetPerRow), ValueDropdown("PossibleSheets"), LabelWidth(45)]
        public int Sheet;
        
        [HorizontalGroup("PerRow")]
        [ShowIf("HasId"), ShowIf("SheetType", SheetTypes.AssetPerRow), ValueDropdown("PossibleHeaders"), LabelWidth(75)]
        public int IdColumn;
        
        [HorizontalGroup("PerRow")]
        [ShowIf("HasId"), ShowIf("SheetType", SheetTypes.AssetPerRow), ValueDropdown("PossibleHeaders"), LabelWidth(90)]
        public int NameColumn;

        private GoogleSheetsManager _manager;

        public GoogleSheetsManager Manager => _manager ?? (_manager = string.IsNullOrEmpty(Id) ? new GoogleSheetsManager() : new GoogleSheetsManager(Id));

        public bool HasId()
        {
            return !string.IsNullOrEmpty(Id);
        }

        private void SheetTypeChanged()
        {
            DataClass = "";
        }

        public void Poll()
        {
            if (string.IsNullOrEmpty(Id)) return;
            Manager.Read(Id);
        }

        private IList<ValueDropdownItem<int>> PossibleSheets
        {
            get
            {
                var output = new ValueDropdownList<int>();
                for (int i = 0; i < Manager.Count; i++)
                {
                    var sheet = Manager[i];
                    output.Add(sheet.Title, i);
                }
                return output;
            }
        }
        
        private IList<ValueDropdownItem<int>> PossibleHeaders
        {
            get
            {
                var output = new ValueDropdownList<int>();
                if (HasId())
                {
                    var row = Manager[Sheet][0];
                    for (int i = 0; i < row.Count; i++)
                    {
                        output.Add(row[i].Key, i);
                    }
                }
                return output;
            }
        }
        
        private IList<ValueDropdownItem<string>> PossibleClasses
        {
            get
            {
                var output = new ValueDropdownList<string>();
                foreach (string type in GoogleSheetsDataClassCache.Get(SheetType))
                {
                    output.Add(type);
                }
                return output;
            }
        }
    }
}