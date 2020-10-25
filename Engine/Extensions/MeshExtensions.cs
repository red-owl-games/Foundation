using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public class MeshBuilder
    {
        private readonly Mesh _mesh;
        
        protected bool Dirty;
        protected readonly Vector3[] Vertices;
        protected readonly Vector2[] Uvs;
        protected readonly int[] Triangles;
        protected readonly Color32[] Colors;

        public bool IsDirty => Dirty;
        
        public MeshBuilder(Vector3[] vertices, int[] triangles, Vector2[] uvs, Color32[] colors)
        {
            _mesh = new Mesh();
            _mesh.MarkDynamic();
            Dirty = true;
            Vertices = vertices;
            Uvs = uvs;
            Triangles = triangles;
            Colors = colors;
            
        }
        
        private Mesh Build()
        {
            _mesh.vertices = Vertices;
            _mesh.uv = Uvs;
            _mesh.triangles = Triangles;
            _mesh.colors32 = Colors;
            _mesh.Optimize();
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();
            Dirty = false;
            return _mesh;
        }
        
        public static implicit operator Mesh(MeshBuilder builder)
        {
            return builder.IsDirty ? builder.Build() : builder._mesh;
        }
        
        #region Static Contructors

        public static Mesh Triangle(float size = 1f)
        {
            size = math.clamp(size, 0, float.MaxValue);
            return new MeshBuilder(
                new[] {
                    new Vector3(0, 0, 0),
                    new Vector3(0, size, 0),
                    new Vector3(size, size, 0),
                },
                new[] {
                    0, 1, 2,
                },
                new[]{
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                },
                null
            );
        }
        
        public static Mesh Quad() => QuadBuilder.Create().CreateQuad(new float3(0));
        public static Mesh Quad(float4 uvs) => QuadBuilder.Create().CreateQuad(new float3(0), new float3(0), new float3(1), uvs);

        public static Mesh BarGraph(params float[] values) => BarGraph(1, 1, 0.5f, values);
        public static Mesh BarGraph(float width, float height, float spacing, params float[] values)
        {
            width = math.clamp(width, 0, float.MaxValue);
            height = math.clamp(height, 0, float.MaxValue);
            spacing = math.clamp(spacing, 0, float.MaxValue);
            float offset = width + spacing;
            var builder = QuadBuilder.Create(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                builder.CreateQuad(width, math.lerp(0f, height, values[i]), new float3(i * offset, 0, 0));
            }
            return builder;
        }

        public static Mesh Radial(int steps = 4, float radius = 1f, float angle = 360f, float angleOffset = 0f) =>
            Radial(steps, radius, angle, angleOffset, new float2(0, 0));
        public static Mesh Radial(int steps, float radius, float angle, float angleOffset, float2 offset)
        {
            steps = math.clamp(steps, 3, 500);
            radius = math.clamp(radius, 0, float.MaxValue);
            angle = math.clamp(angle, 1f, 360f);
            angleOffset = math.clamp(angleOffset, 0f, 360f);
            
            var origin = new Vector3(offset.x, offset.y, 0f);
            var start = Quaternion.Euler(0, 0, angleOffset) * new Vector3(radius + offset.x, offset.y, 0);
            Color32 white = Color.white;

            var vertices = new Vector3[2 + steps];
            var uvs = new Vector2[2 + steps];
            var triangles = new int[3 * steps];
            var colors = new Color32[2 + steps];
            
            vertices[0] = origin;
            vertices[1] = start;
            uvs[0] = origin;
            uvs[1] = start.normalized;
            colors[0] = white;
            colors[1] = white;
            for (int i = 0; i < steps; i++)
            {
                int index = (i * 3);
                var next = Quaternion.Euler(0, 0, math.lerp(0, angle, (i + 1) / (float)steps)) * start;
                vertices[i + 2] = next;
                uvs[i + 2] = next.normalized;
                colors[i + 2] = white;
                triangles[index] = 0;
                triangles[index + 1] = i + 2;
                triangles[index + 2] = i + 1;
            }
            return new MeshBuilder(vertices, triangles, uvs, colors);
        }

        public static Mesh RadialGraph(params float[] values) => RadialGraph(1f, values);
        public static Mesh RadialGraph(float radius, params float[] values)
        {
            if (values.Length < 4) throw new ArgumentException($"values must have at least 4 datapoints");
            return new MeshBuilder(null, null, null, null);
        }
        
        #endregion
    }

    public class QuadBuilder : MeshBuilder
    {
        private const int MaxQuadCount = 15000;

        private int _quadIndex;
        private int[] _quadIndexes;

        public int CurrentQuad => _quadIndex;

        private QuadBuilder(int quadCount) : base(new Vector3[4 * quadCount], new int[6 * quadCount], new Vector2[4 * quadCount], new Color32[4 * quadCount])
        {
            _quadIndex = 0;
            _quadIndexes = new int[quadCount];
            _quadIndexes[0] = -1;
            for (int i = 1; i < quadCount; i++)
            {
                _quadIndexes[i] = i;
            }
        }
        
        public static QuadBuilder Create(int quadCount = 1) => new QuadBuilder(math.clamp(quadCount, 0, MaxQuadCount));

        private int GetNextQuadIndex()
        {
            int count = _quadIndexes.Length;
            for (int i = 0; i < count; i++)
            {
                if (_quadIndexes[i] <= 0) continue;
                _quadIndexes[i] = -1;
                return i;
            }

            return -1;
        }

        public QuadBuilder CreateQuad(float width, float height, float3 position) =>
            CreateQuad(position, new float3(0), new float3(width, height, 0), new float4(0, 0, 1, 1));
        public QuadBuilder CreateQuad(float3 position) =>
            CreateQuad(position, new float3(0), new float3(1), new float4(0, 0, 1, 1));

        public QuadBuilder CreateQuad(float3 position, float3 rotation) =>
            CreateQuad(position, rotation, new float3(1), new float4(0, 0, 1, 1));

        public QuadBuilder CreateQuad(float3 position, float3 rotation, float3 scale, float4 uvs)
        {
            if (_quadIndex == -1)
            {
                _quadIndex = GetNextQuadIndex();
                if (_quadIndex == -1)
                    return this; // No more quads can be spawned until one is destroyed
            }
            
            UpdateQuad(_quadIndex, position, rotation, scale)
                .UpdateQuad(_quadIndex, uvs)
                .UpdateQuad(_quadIndex, Color.white);

            var i = new QuadVert(_quadIndex);
            int tIndex = _quadIndex * 6;

            Triangles[tIndex] = i.V0;
            Triangles[tIndex + 1] = i.V1;
            Triangles[tIndex + 2] = i.V2;

            Triangles[tIndex + 3] = i.V0;
            Triangles[tIndex + 4] = i.V2;
            Triangles[tIndex + 5] = i.V3;

            _quadIndex = GetNextQuadIndex();

            Dirty = true;
            return this;
        }

        public QuadBuilder UpdateQuad(int quadIndex, float3 position) =>
            UpdateQuad(quadIndex, position, Quaternion.identity, new float3(1));
        public QuadBuilder UpdateQuad(int quadIndex, float3 position, float3 rotation) =>
            UpdateQuad(quadIndex, position, Quaternion.Euler(rotation), new float3(1));
        public QuadBuilder UpdateQuad(int quadIndex, float3 position, float3 rotation, float3 scale) =>
            UpdateQuad(quadIndex, position, Quaternion.Euler(rotation), scale);
        public QuadBuilder UpdateQuad(int quadIndex, float3 position, quaternion rotation, float3 scale) =>
            UpdateQuad(quadIndex, position, (Quaternion)rotation, scale);
        public QuadBuilder UpdateQuad(int quadIndex, float3 position, Quaternion rotation, float3 scale)
        {
            if (quadIndex > _quadIndexes.Length || quadIndex < 0) return this;
            
            float widthOffset = scale.x + position.x;
            float heightOffset = scale.y + position.y;

            var i = new QuadVert(quadIndex);

            Vertices[i.V0] = rotation * new Vector3(position.x, position.y, position.z);
            Vertices[i.V1] = rotation * new Vector3(position.x, heightOffset, position.z);
            Vertices[i.V2] = rotation * new Vector3(widthOffset, heightOffset, position.z);
            Vertices[i.V3] = rotation * new Vector3(widthOffset, position.y, position.z);

            Dirty = true;
            return this;
        }

        public QuadBuilder UpdateQuad(int quadIndex, float4 uvs)
        {
            if (quadIndex > _quadIndexes.Length || quadIndex < 0) return this;
            
            var i = new QuadVert(quadIndex);

            Uvs[i.V0] = new Vector2(uvs.x, uvs.y);
            Uvs[i.V0] = new Vector2(uvs.x, uvs.w);
            Uvs[i.V0] = new Vector2(uvs.z, uvs.w);
            Uvs[i.V0] = new Vector2(uvs.z, uvs.y);

            Dirty = true;
            return this;
        }

        public QuadBuilder UpdateQuad(int quadIndex, Color color)
        {
            if (quadIndex > _quadIndexes.Length || quadIndex < 0) return this;
            
            var i = new QuadVert(quadIndex);

            Colors[i.V0] = color;
            Colors[i.V1] = color;
            Colors[i.V2] = color;
            Colors[i.V3] = color;

            Dirty = true;
            return this;
        }

        public QuadBuilder DestroyQuad(int quadIndex)
        {
            if (quadIndex > _quadIndexes.Length || quadIndex < 0) return this;
            
            var i = new QuadVert(quadIndex);

            Vertices[i.V0] = Vector3.zero;
            Vertices[i.V1] = Vector3.zero;
            Vertices[i.V2] = Vector3.zero;
            Vertices[i.V3] = Vector3.zero;

            _quadIndexes[quadIndex] = quadIndex;

            Dirty = true;
            return this;
        }
        
        private struct QuadVert
        {
            public readonly int V0;
            public readonly int V1;
            public readonly int V2;
            public readonly int V3;
            
            public QuadVert(int quadIndex)
            {
                V0 = quadIndex * 4;
                V1 = V0 + 1;
                V2 = V0 + 2;
                V3 = V0 + 3;
            }
        }
    }
}