using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreate : MonoBehaviour
{
    [SerializeField]
    private Texture m_texture = null;
    List<Vector3> list_v = new List<Vector3>();
    Vector3[] vertices = new Vector3[4];
    int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

    private void Start()
    {
        list_v.Add(new Vector3(-1f, 1f, 1f));
        list_v.Add(new Vector3(1f, 1f, 1f));
        list_v.Add(new Vector3(1f, -1f, 1f));
        list_v.Add(new Vector3(-1f, -1f, 1f));

        vertices = list_v.ToArray();

        Mesh mesh = new Mesh();
        Vector2[] uvs = new Vector2[] { new Vector2(0f, 1f),
                                    new Vector2(1f, 1f),
                                    new Vector2(1f, 0f),
                                    new Vector2(0f, 0f) };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;

        Material material = new Material(Shader.Find("Standard"));
        material.SetTexture("_MainTex", m_texture);
        GetComponent<MeshRenderer>().material = material;
    }
}
