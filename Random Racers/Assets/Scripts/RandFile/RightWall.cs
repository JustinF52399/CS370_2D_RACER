using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RightWall : MonoBehaviour
{
    public Vector3[] rVerts;
    public int[] triangles;
    public MeshFilter filter;
    public MeshRenderer render;
    public MeshCollider collider;

    //generates the right-side wall for the road using the verticies from the road mesh
    public void generateRightWall(Vector3[] pnts, bool isClosed, float roadWidth)
    {
        rVerts = new Vector3[pnts.Length * 6];
        triangles = new int[(rVerts.Length + (rVerts.Length / 2)) * 3];
        int l = 0;
        int t = 0;
        Vector2[] uvs = new Vector2[rVerts.Length];

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
            uvs[l + 1] = new Vector2(1, completionPercent);
            Vector3 right = -(new Vector3(-forward.z, 0, forward.x));

            rVerts[l] = pnts[i] + right * roadWidth * .6f - new Vector3(0, 20f, 0);
            rVerts[l + 1] = pnts[i] + right * roadWidth * .6f + new Vector3(0, 1f, 0);
            rVerts[l + 2] = pnts[i] + right * roadWidth * .6f + new Vector3(0, 1f, 0);
            rVerts[l + 3] = pnts[i] + right * roadWidth * .5f + new Vector3(0, 1f, 0);
            rVerts[l + 4] = pnts[i] + right * roadWidth * .5f + new Vector3(0, 1f, 0);
            rVerts[l + 5] = pnts[i] + right * roadWidth * .5f - new Vector3(0, 20f, 0);

            triangles[t] = (l+7 ) % rVerts.Length;
            triangles[t + 1] = (l + 6) % rVerts.Length;
            triangles[t + 2] = (l + 0) % rVerts.Length;
            triangles[t + 3] = (l+1) % rVerts.Length;
            triangles[t + 4] = (l + 7) % rVerts.Length;
            triangles[t + 5] = (l + 0) % rVerts.Length;
            triangles[t + 6] = (l + 2) % rVerts.Length;
            triangles[t + 7] = (l + 9) % rVerts.Length;
            triangles[t + 8] = (l + 8) % rVerts.Length;
            triangles[t + 9] = (l + 2) % rVerts.Length;
            triangles[t + 10] = (l + 3) % rVerts.Length;
            triangles[t + 11] = (l + 9) % rVerts.Length;
            triangles[t + 12] = (l + 4) % rVerts.Length;
            triangles[t + 13] = (l + 11) % rVerts.Length;
            triangles[t + 14] = (l + 10) % rVerts.Length;
            triangles[t + 15] = (l + 5) % rVerts.Length;
            triangles[t + 16] = (l + 11) % rVerts.Length;
            triangles[t + 17] = (l + 4) % rVerts.Length;

            t += 18;
            l += 6;



            //GetComponent<MeshFilter>().mesh = wallMesh;
            //GetComponent<MeshCollider>().sharedMesh = wallMesh;


        }
        Mesh wallMesh = new Mesh();
        wallMesh.vertices = rVerts;
        wallMesh.triangles = triangles;
        wallMesh.RecalculateNormals();
        wall.AddComponent<MeshFilter>();
        wall.GetComponent<MeshFilter>().mesh = wallMesh;
        wall.AddComponent<MeshCollider>().sharedMesh = wallMesh;
        wall.AddComponent<MeshRenderer>();
        //wall.AddComponent<Renderer>();
        wall.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
        wall.GetComponent<Renderer>().material.color = Color.gray;
        ///= Color.gray;
        //wall.GetComponent<Renderer>().material.shader = Shader.
        //wall.GetComponent<Renderer>().material = Resources.Load("DEV_Gray", typeof(Material)) as Material;


    }
}
