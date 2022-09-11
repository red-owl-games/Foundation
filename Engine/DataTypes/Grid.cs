using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class GridSettings
    {
        [SerializeField]
        private float3 cellSize;
        [SerializeField]
        private float3 offset;

        public float3 CellSize => cellSize;
        public float3 Offset => offset;
        
        public GridSettings(float3 cellSize, float3 offset)
        {
            this.cellSize = cellSize;
            this.offset = offset;
        }

        public GridSettings() : this(new float3(1), float3.zero) {}
        
        public GridSettings(float cellSizeX = 1f, float cellSizeY = 1f, float cellSizeZ = 1f, float offsetX = 0f, float offsetY = 0f, float offsetZ = 0f) : 
            this(new float3(cellSizeX, cellSizeY, cellSizeZ), new float3(offsetX, offsetY, offsetZ)) {}
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int3 GetCellFrom(float3 worldPosition) => 
            (int3)math.floor((worldPosition / cellSize)- offset);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float3 GetWorldPositionFrom(int cellX, int cellY, int cellZ) =>
            GetWorldPositionFrom(new int3(cellX, cellY, cellZ));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float3 GetWorldPositionFrom(int3 cell) => cell * cellSize + offset;

        public GridSettings Clone() => new(cellSize, offset);
    }
    
    [Serializable]
    public class GridData<T> : ISerializationCallbackReceiver where T : new()
    {
        [Serializable]
        public struct GridItem
        {
            public int3 cell;
            public T data;
        }

        public event Action<int3, T> OnCellChanged;

        private Dictionary<int3, T> _store;
        [SerializeField]
        private GridSettings settings;
        [SerializeField]
        private GridItem[] data;
        
        public GridData(GridSettings settings)
        {
            _store = new Dictionary<int3, T>();
            this.settings = settings;
        }
        
        public GridData() : this(new GridSettings()) {}

        public T Get(int3 cell)
        {
            try
            {
                return _store[cell];
            }
            catch (Exception e)
            {
                if (e is KeyNotFoundException)
                {
                    return _store[cell] = new T();
                }
                throw;
            }
        }

        public T Get(int cellX, int cellY, int cellZ) => Get(new int3(cellX, cellY, cellZ));

        public T Get(float3 worldPosition) => Get(settings.GetCellFrom(worldPosition));

        public void Set(int3 cell, T item)
        {
            try
            {
                if (_store[cell].Equals(item)) return;
            }
            catch (Exception e)
            {
                if (e is not KeyNotFoundException)
                {
                    throw;
                }
            }

            _store[cell] = item;
            OnCellChanged?.Invoke(cell, item);
        }

        public void Set(int cellX, int cellY, int cellZ, T item) => Set(new int3(cellX, cellY, cellZ), item);

        public void Set(float3 worldPosition, T item) => Set(settings.GetCellFrom(worldPosition), item);

        public void Set(int3 start, int3 end, T item)
        {
            var min = math.min(start, end);
            var max = math.max(start, end);
            for (var x = min.x; x < max.x; x++)
            {
                for (var y = min.y; y < max.y; y++)
                {
                    for (var z = min.z; z < max.z; z++)
                    {
                        Set(new int3(x,y,z), item);
                    }
                }
            }
        }

        public void Set(float3 worldPositionStart, float3 worldPositionEnd, T item) => 
            Set(settings.GetCellFrom(worldPositionStart), settings.GetCellFrom(worldPositionEnd), item);

        public void OnBeforeSerialize()
        {
            data = new GridItem[_store.Count];
            var i = 0;
            foreach (var kvp in _store)
            {
                data[i] = new GridItem()
                {
                    cell = kvp.Key,
                    data = kvp.Value
                };
                i++;
            }
        }

        public void OnAfterDeserialize()
        {
            if (data == null) return;
            _store.Clear();
            foreach (var item in data)
            {
                _store[item.cell] = item.data;
            }
        }
    }

    public partial class Game
    {
        [FoldoutGroup("Red Owl"), BoxGroup("Red Owl/Grid"), HideLabel, InlineProperty]
        public Parameter<GridSettings> GridSettings = new(new GridSettings());
    }
}