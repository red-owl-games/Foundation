using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    [RequireComponent (typeof (MeshCollider))]
    public class OneWayMeshCollider : MonoBehaviour
    {
        public bool topCollision = true;
        public float maxAngle = 45.0f;

        private void Start ()
        {
            float cos = Mathf.Cos(maxAngle);
            var mc = GetComponent<MeshCollider>();
            var mesh = new Mesh();
            var sharedMesh = mc.sharedMesh;
            var verts = sharedMesh.vertices;
            var triangles = new List<int>(sharedMesh.triangles);
            for (int i = triangles.Count-1; i >=0 ; i -= 3)
            {
                Vector3 p1 = transform.TransformPoint(verts[triangles[i-2]]);
                Vector3 p2 = transform.TransformPoint(verts[triangles[i-1]]);
                Vector3 p3 = transform.TransformPoint(verts[triangles[i  ]]);
                Vector3 faceNormal = Vector3.Cross(p3-p2,p1-p2).normalized;
                if ((!topCollision || !(Vector3.Dot(faceNormal, Vector3.up) <= cos)) &&
                    (topCollision || !(Vector3.Dot(faceNormal, -Vector3.up) <= cos))) continue;
                triangles.RemoveAt(i);
                triangles.RemoveAt(i-1);
                triangles.RemoveAt(i-2);
            }
            mesh.vertices = verts;
            mesh.triangles = triangles.ToArray();
            mc.sharedMesh = mesh;
        }
    }
}