using System.Collections.Generic;
using System.Text;

namespace RedOwl.Engine
{
    // TODO: Listing Save Files
    // TODO: Picking a Save File
    // TODO: Events
    
    [Command("SaveSystem.Save", "Triggers a Save of the Game")]
    public class SaveCommand : ICommand
    {
        public void Invoke(string[] args) => Game.SaveSystem.Save();
    }

    [Command("SaveSystem.Load", "Triggers a Load of the Game")]
    public class LoadCommand : ICommand
    {
        public void Invoke(string[] args) => Game.SaveSystem.Load();
    }
    
    [Command("SaveSystem.Write", "Triggers a Write of the Game")]
    public class WriteCommand : ICommand
    {
        public void Invoke(string[] args) => Game.SaveSystem.Write();
    }
    
    [Command("SaveSystem.Read", "Triggers a Read of the Game")]
    public class ReadCommand : ICommand
    {
        public void Invoke(string[] args) => Game.SaveSystem.Read();
    }

    public class SaveSystem
    {
        private static ulong _ids;

        private DataFile data;
        private DataFile meta;
        private DataFile prefs;
        private readonly Dictionary<ulong, ISaveable> saveables = new Dictionary<ulong, ISaveable>();
        private string current;

        private DataFile GetFile(SaveableTypes type)
        {
            switch (type)
            {
                case SaveableTypes.Data:
                    if (data == null)
                    {
                        Log.Always("Creating Data File");
                        data = new DataFile(100);
                    }

                    return data;
                case SaveableTypes.Meta:
                    if (meta == null)
                    {
                        Log.Always("Creating Meta File");
                        meta = new DataFile(100);
                    }

                    return meta;
                case SaveableTypes.Pref:
                    if (prefs == null)
                    {
                        Log.Always("Creating Prefs File");
                        prefs = new DataFile(100);
                    }
                    
                    return prefs;
            }
            Log.Always($"WTF {type}");
            return null;
        }
        
        private void WriteTo(ISaveable value)
        {
            var file = GetFile(value.Type);
            if (!file.Get(value.Key, out var stream))
            {
                stream = file.Allocate(value.Key);
            };
            using (var w = new SaveWriter(stream, Encoding.UTF8, true))
            {
                Log.Always($"Saving Data For: {value.Key}");
                w.BaseStream.Position = 0;
                value.Write(w);
            }
        }

        private void ReadFrom(ISaveable value)
        {
            var file = GetFile(value.Type);
            if (!file.Get(value.Key, out var stream)) return;
            using (var r = new SaveReader(stream, Encoding.UTF8, true))
            {
                Log.Always($"Loading Data For: {value.Key}");
                r.BaseStream.Position = 0;
                value.Read(r);
            }
        }

        public ulong Subscribe(ISaveable value)
        {
            _ids += 1;
            //Log.Always($"Subscribing: {_ids} = {value.Key} | {value.Type}");
            saveables.Add(_ids, value);
            // TODO: Should this ReadFrom be 1 frame delay?
            ReadFrom(value);
            return _ids;
        }

        public void Unsubscribe(ulong id)
        {
            //Log.Always($"Unsubscribe: {id} = {saveables[id].Key}");
            WriteTo(saveables[id]);
            saveables.Remove(id);
        }
        
        public void Save()
        {
            foreach (var item in saveables.Values)
            {
                WriteTo(item);
            }
        }

        public void Load()
        {
            foreach (var item in saveables.Values)
            {
                ReadFrom(item);
            }
        }

        public void Write() => Write(current);
        public void Write(string name)
        {
            current = name;
            Log.Always($"Writing Save: {current}");
            Save();
            FileController.Write("prefs.data", prefs);
            FileController.Write($"saves/{current}.meta", meta);
            FileController.Write($"saves/{current}.data", data);
        }

        public void Read() => Read(current);
        public void Read(string name)
        {
            current = name;
            Log.Always($"Reading Save: {current}");
            FileController.Read("prefs.data", out prefs);
            FileController.Read($"saves/{current}.meta", out meta);
            FileController.Read($"saves/{current}.data", out data);
            Load();
        }
    }


    public partial class Game
    {
        public static readonly SaveSystem SaveSystem = new SaveSystem();
    }

}