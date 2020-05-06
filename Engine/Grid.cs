using System;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public class Grid<T> where T : struct
    {
        public event Action<int, int, T> OnCellChanged;

        public int Width { get; }
        public int Height { get; }
        public int Size { get; }
        public float3 Origin { get; }

        private int2 _min;
        private int2 _max;
        private readonly T[,] _array;
        
        public Grid(int width, int height, float3 origin, int size = 1, Func<int, int, T> cellConstructor = null)
        {
            Width = width;
            Height = height;
            Size = size;
            Origin = origin;
            _min = new int2(0, 0);
            _max = new int2(width - 1, height - 1);
            _array = new T[width, height];

            if (cellConstructor != null) ForEach(cellConstructor);
        }

        public void ForEach(Action<int, int, T> cellAction)
        {
            
            for (int yIndex = 0; yIndex < Height; yIndex++)
            {
                for (int xIndex = 0; xIndex < Width; xIndex++)
                {
                    cellAction(xIndex, yIndex, _array[xIndex, yIndex]);
                }
            }
        }
        
        public void ForEach(Func<int, int, T> cellAction)
        {
            for (int yIndex = 0; yIndex < Height; yIndex++)
            {
                for (int xIndex = 0; xIndex < Width; xIndex++)
                {
                    _array[xIndex, yIndex] = cellAction(xIndex, yIndex);
                }
            }
        }
        
        public int2 GetRandomCell() =>
            new int2(
                RedOwl.Random.NextInt(Width),
                RedOwl.Random.NextInt(Height));

        public int2 GetRandomCellEdge()
        {
            int side = RedOwl.Random.NextInt(4);
            switch (side)
            {
                case 0: // Bottom
                    return new int2(RedOwl.Random.NextInt(Width),-1);
                case 1: // Left
                    return new int2(-1, RedOwl.Random.NextInt(Height));
                case 2: // Right
                    return new int2(Width, RedOwl.Random.NextInt(Height));
                case 3: // Top
                    return new int2(RedOwl.Random.NextInt(Width),Height);
            }
            return new int2(0, 0);
        }

        public float3 GetWorldPosition(int2 cell, int z = 0)
        {
            return new float3(cell.x, cell.y, z) * Size + Origin;
        }

        public float3 GetWorldPosition(int x, int y, int z = 0) => GetWorldPosition(new int2(x, y), z);

        public int2 GetGridCoordinates(float3 position) =>
            new int2(
                Mathf.FloorToInt((position - Origin).x / Size),
                Mathf.FloorToInt((position - Origin).y / Size));
        public int2 GetGridCoordinates(float x, float y, float z = 0) => GetGridCoordinates(new float3(x, y, z));
        
        public int2 ClampToGrid(int2 position) => math.clamp(position, _min, _max);

        public T GetValue(int2 cell)
        {
            return cell.x >= 0 && cell.y >= 0 && cell.x < Width && cell.y < Height ? _array[cell.x, cell.y] : default;
        }

        public T GetValue(int x, int y) => GetValue(new int2(x, y));
        
        public T GetValue(float3 worldPosition) => GetValue(GetGridCoordinates(worldPosition));

        public void SetValue(int2 cell, T value)
        {
            _array[cell.x, cell.y] = value;
            OnCellChanged?.Invoke(cell.x, cell.y, value);
        }

        public void SetValue(int x, int y, T value) => SetValue(new int2(x, y), value);

        public void SetValue(float3 worldPosition, T value) => SetValue(GetGridCoordinates(worldPosition), value);
    }
}