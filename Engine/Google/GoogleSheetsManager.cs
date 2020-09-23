using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Unity.Mathematics;

namespace RedOwl.Core
{
    public struct SheetRowItemData
    {
        public string Key;
        public object Value;

        public static implicit operator string(SheetRowItemData self)
        {
            return (string) self.Value;
        }

        public override string ToString()
        {
            return (string) Value;
        }
    }

    public struct SheetRowData : IEnumerable<SheetRowItemData>
    {
        private readonly SheetRowItemData[] _array;
        private readonly Dictionary<string, int> _dict;

        public SheetRowItemData this[int index] => _array[index];
        public SheetRowItemData this[string key]
        {
            get
            {
                key = key.ToLower();
                return _dict.TryGetValue(key, out int index)
                    ? _array[index]
                    : new SheetRowItemData {Key = key, Value = null};
            }
        }

        public SheetRowData(string[] keys, IList<object> row)
        {
            int count = keys.Length;
            _array = new SheetRowItemData[count];
            _dict = new Dictionary<string, int>(count);
            for (int i = 0; i < count; i++)
            {
                string key = keys[i].ToLower();
                _array[i] = new SheetRowItemData {Key = keys[i], Value = row[i]};
                _dict[keys[i].ToLower()] = i;
            }
        }

        public int Count => _array.Length;

        public IEnumerator<SheetRowItemData> GetEnumerator()
        {
            foreach (var item in _array)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[ ");
            foreach (var item in this)
            {
                builder.Append($"{item.Key}:{item.Value} ");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }

    public struct SheetData : IEnumerable<SheetRowData>
    {
        public readonly string Title;
        
        private readonly SheetRowData[] _array;
        
        public SheetRowData this[int index] => _array[index];
        
        public SheetData(string title, IList<IList<object>> data = default)
        {
            Title = title.ToLower();
            int rowCount = data.Count;
            _array = new SheetRowData[rowCount - 1];
            if (rowCount <= 0) return;
            var headerRow = data[0];
            int headersCount = headerRow.Count;
            var keys = new string[headersCount];
            for (int i = 0; i < headersCount; i++)
            {
                keys[i] = ((string)headerRow[i]).ToLower();
            }
            if (rowCount == 1)
            {
                for (int i = 1; i < rowCount; i++)
                {
                    _array[i-1] = new SheetRowData(keys, new object[headersCount]);
                }
            }
            else
            {
                for (int i = 1; i < rowCount; i++)
                {
                    _array[i-1] = new SheetRowData(keys, data[i]);
                }
            }
        }

        public int Count => _array.Length;

        public IEnumerator<SheetRowData> GetEnumerator()
        {
            foreach (var item in _array)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"{Title}(\n");
            foreach (var item in this)
            {
                builder.Append($"\t{item} \n");
            }
            builder.Append(")");
            return builder.ToString();
        }
    }

    public class GoogleSheetsManager : IEnumerable<SheetData>
    {
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        public string Title => _spreadsheet != null ? _spreadsheet.Properties.Title : "";
        public SheetData this[int index] => _array[index];
        public SheetData this[string key] => _dict.TryGetValue(key.ToLower(), out int index) ? _array[index] : new SheetData(key);

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
            int count = _spreadsheet.Sheets.Count;
            _array.Clear();
            _dict.Clear();
            for (int i = 0; i < count; i++)
            {
                string title = _spreadsheet.Sheets[i].Properties.Title;
                _array.Add(new SheetData(title, GetSheetData(_service, _spreadsheet, i)));
                _dict.Add(title, i);
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
        private static string ColumnFromIndex(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

        /// <summary>
        /// A -> 1<br/>
        /// B -> 2<br/>
        /// C -> 3<br/>
        /// ...
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static int IndexFromColumn(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)math.pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }
        
        private static string GetSheetRange(Sheet sheet)
        {
            int columnCount = sheet.Properties.GridProperties.ColumnCount ?? 1;
            int rowCount = sheet.Properties.GridProperties.RowCount ?? 1;
            return $"{sheet.Properties.Title}!A1:{ColumnFromIndex(columnCount)}{rowCount}";
        }

        private static IList<IList<object>> GetSheetData(SheetsService service, Spreadsheet spreadsheet, int index)
        {
            try
            {
                return service.Spreadsheets.Values.Get(spreadsheet.SpreadsheetId, GetSheetRange(spreadsheet.Sheets[index])).Execute().Values;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}