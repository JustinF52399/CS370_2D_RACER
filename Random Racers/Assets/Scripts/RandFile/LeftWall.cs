using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class LeftWall : MonoBehaviour
{
    public Vector3[] lVerts;
    public int[] triangles;
    public MeshFilter filter;
    public MeshRenderer render;
    public MeshCollider collider;
    
    //creates a wall for the left side of the road using the verticies of the road mesh
    public void generateLeftWall(Vector3[] pnts, bool isClosed, float roadWidth)
    {
        lVerts = new Vector3[pnts.Length * 6];
        triangles = new int[(lVerts.Length + (lVerts.Length /2))*3];
        int l = 0;
        int t = 0;
        Vector2[] uvs = new Vector2[lVerts.Length];
        
        GameObject wall = new GameObject();
        
        for (int i = 0; i < pnts.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < pnts.Length - 1 || isClosed)
            {
                forward += pnts[(i + 1) % pnts.Length] - pnts[i];
            }
            if (i > 0 || isClosed)
            {
                forward += pnts[i] - pnts[(i - 1 + pnts.Length) % pnts.Length];
            }
            forward.Normalize();

            float completionPercent = i / (float)(pnts.Length - 1);
            uvs[l] = new Vector2(0, completionPercent);
            uvs[l+1] = new Vector2(1, completionPercent);
            Vector3 left = new Vector3(-forward.z, 0, forward.x);

            lVerts[l] = pnts[i] + left * roadWidth * .6f - new Vector3(0, 20f, 0);
            lVerts[l + 1] = pnts[i] + left * roadWidth * .6f + new Vector3(0, 1f, 0);
            lVerts[l + 2] = pnts[i] + left * roadWidth * .6f + new Vector3(0, 1f, 0);
            lVerts[l + 3] = pnts[i] + left * roadWidth * .5f + new Vector3(0, 1f, 0);
            lVerts[l + 4] = pnts[i] + left * roadWidth * .5f + new Vector3(0, 1f, 0);
            lVerts[l + 5] = pnts[i] + left * roadWidth * .5f - new Vector3(0, 20f, 0);

            triangles[t] = l;
            triangles[t + 1] = (l + 6) % lVerts.Length;
            triangles[t + 2] = (l + 7) % lVerts.Length;
            triangles[t + 3] = (l) % lVerts.Length;
            triangles[t + 4] = (l +7) % lVerts.Length;            
            triangles[t + 5] = (l+1) % lVerts.Length;
            triangles[t + 6] = (l + 8) % lVerts.Length;
            triangles[t + 7] = (l + 9) % lVerts.Length;
            triangles[t + 8] = (l + 2) % lVerts.Length;
            triangles[t + 9] = (l + 9) % lVerts.Length;
            triangles[t + 10] = (l + 3) % lVerts.Length;
            triangles[t + 11] = (l + 2) % lVerts.Length;
            triangles[t + 12] = (l + 10) % lVerts.Length;
            triangles[t + 13] = (l + 11) % lVerts.Length;
            triangles[t + 14] = (l + 4) % lVerts.Length;
            triangles[t + 15] = (l + 4) % lVerts.Length;
            triangles[t + 16] = (l + 11) % lVerts.Length;
            triangles[t + 17] = (l + 5) % lVerts.Length;

            t += 18;
            l += 6;
        }
        Mesh wallMesh = new Mesh();
        wallMesh.vertices = lVerts;
        wallMesh.triangles = triangles;
        wallMesh.RecalculateNormals();
        wall.AddComponent<MeshFilter>();
        wall.GetComponent<MeshFilter>().mesh = wallMesh;
        wall.AddComponent<MeshCollider>().sharedMesh = wallMesh;
        wall.AddComponent<MeshRenderer>();        
        wall.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
        wall.GetComponent<Renderer>().material.color = Color.gray; 
        


    }
}
