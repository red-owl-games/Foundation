using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    [Serializable]
    public class SheetRowItem
    {
        [SerializeField, HideInInspector]
        private int x;
        [SerializeField, HideInInspector]
        private int y;
        [SerializeField, HideInInspector]
        private string value;

        public int X => x;
        public int Y => y;
        [ShowInInspector]
        public string Value => value;
        
        public SheetRowItem(int x, int y, string value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }

        public override string ToString()
        {
            return $"[{x},{y}|{value}]";
        }
    }
    
    [Serializable]
    public class SheetRow : List<SheetRowItem>
    {
        public SheetRow(int width) : base(width) {}
        
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var item in this)
            {
                builder.Append($"{item}, ");
            }
            return builder.ToString();
        }
    }
    
    public class SheetData : IEnumerable<SheetRow>
    {
        public string Title { get; }
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        private List<SheetRow> Data { get; }
        
        public SheetRow this[int index] => Data[index];

        public SheetData(string title, int width, int height, IList<IList<object>> data)
        {
            Title = title;
            Width = width;
            Height = height;
            Data = new List<SheetRow>(height);

            for (int y = 0; y < height; y++)
            {
                var row = new SheetRow(width);
                for (int x = 0; x < width; x++)
                {
                    row.Add(new SheetRowItem(x, y, (string)data[y].SafeGet(x, "")));
                }
                Data.Add(row);
            }
        }

        public IEnumerator<SheetRow> GetEnumerator()
        {
            for (int i = 0; i < Height; i++)
            {
                yield return Data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var builder = new StringBuilder($"{Title}[\n");
            foreach (var row in Data)
            {
                builder.Append($"\t{row}\n");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }

    public class GoogleSheetsManager : IEnumerable<SheetData>
    {
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        public string Title => _spreadsheet != null ? _spreadsheet.Properties.Title : "";
        public SheetData this[int index] => _array[index];
        public SheetData this[string key] => _dict.TryGetValue(key.ToLower(), out int index) ? _array[index] : null;

        private readonly List<SheetData> _array;
        private readonly Dictionary<string, int> _dict;
        private readonly SheetsService _service;
        private Spreadsheet _spreadsheet;

        public GoogleSheetsManager()
        {
            _array = new List<SheetData>(1);
            _dict = new Dictionary<string, int>(1);
            _service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GoogleCredential.FromJson(GoogleSheetsSettings.Creds).CreateScoped(Scopes),
                ApplicationName = "unity",
            });
        }

        public GoogleSheetsManager(string sheetId) : this()
        {
            if (!GoogleSheetsSettings.HasCreds) return;
            Read(sheetId);
        }

        public int Count => _array.Count;
        
        public IEnumerator<SheetData> GetEnumerator()
        {
            foreach (var sheetData in _array)
            {
                yield return sheetData;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Read(string sheetId)
        {
            _spreadsheet = _service.Spreadsheets.Get(sheetId).Execute();
            Refresh();
        }

        public void Refresh()
        {
            _array.Clear();
            _dict.Clear();
            int i = 0;
            var lookup = new Dictionary<int, string>(_spreadsheet.Sheets.Count);
            foreach (var sheet in _spreadsheet.Sheets)
            {
                lookup.Add((int)sheet.Properties.SheetId, sheet.Properties.Title);
            }
            foreach (var range in _spreadsheet.NamedRanges)
            {
                int sheetId = range.Range.SheetId ?? 0;
                int width =  (range.Range.EndColumnIndex ?? 1) - (range.Range.StartColumnIndex ?? 0);
                int height = (range.Range.EndRowIndex ?? 1) - (range.Range.StartRowIndex ?? 0);
                Log.Always($"{range.Name} | {width} | {height}");
                var values = _service.Spreadsheets.Values.Get(_spreadsheet.SpreadsheetId, GetSheetRange(lookup[sheetId], range.Range)).Execute().Values;
                var data = new SheetData(range.Name, width, height, values);
                _array.Add(data);
                _dict.Add(range.Name, i);
                i++;
            }
        }

        /// <summary>
        /// 1 -> A<br/>
        /// 2 -> B<br/>
        /// 3 -> C<br/>
        /// ...
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string ColumnToLetter(int column)
        {
            decimal temp;
            string letter = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                temp = (columnNumber - 1) % 26;
                letter = ((char)(temp + 65)) + letter;
                columnNumber = (columnNumber - temp - 1) / 26;
            }
            return letter;
        }

        /// <summary>
        /// A -> 1<br/>
        /// B -> 2<br/>
        /// C -> 3<br/>
        /// ...
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static int LetterToColumn(string letter)
        {
            int column = 0;
            string letters = letter.ToUpper();
            var length = letter.Length;
            for (var i = 0; i < length; i++)
            {
                var index = ((char)letters[i]) - 64;
                column += index * (int)math.pow(26, length - i - 1);
            }
            return column;
        }
        
        private static string GetSheetRange(string spreadsheet, GridRange range)
        {
            int startColumn = range.StartColumnIndex + 1 ?? 1;
            int startRow = range.StartRowIndex + 1 ?? 1;
            int endColumn = range.EndColumnIndex ?? 1;
            int endRow = range.EndRowIndex ?? 1;
            return $"{spreadsheet}!{ColumnToLetter(startColumn)}{startRow}:{ColumnToLetter(endColumn)}{endRow}";
        }
    }
}