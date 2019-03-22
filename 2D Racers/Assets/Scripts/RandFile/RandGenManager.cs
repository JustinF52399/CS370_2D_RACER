using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandGenManager : MonoBehaviour
{
    float xRand;
    float yRand;
    float zRand;
    List<Vector3> points = new List<Vector3>();
    List<Vector3> hull = new List<Vector3>();
    List<Vector3> sortedPoints = new List<Vector3>();
    public CatmullRom spline;
    List<Transform> sortedTrans = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        int seed = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        Random.InitState(seed);
        Debug.Log(Random.seed.ToString());
        int randVal = Random.RandomRange(10, 20);
        //for rand iterations create rand new points of random positions and put them in a list
        for (int i = 0; i < randVal; i++)
        {
            //get a random x, y, and z from -150 to 150
            xRand = Random.Range(-150f, 150f);
            yRand = 0;
            zRand = Random.Range(-150f, 150f);

            //Print those values to debug
            Debug.Log("(Point " + i + ") x: " + xRand.ToString() + " z: " + zRand.ToString());

            //create a temporary point
            Vector3 temp = new Vector3(xRand, yRand, zRand);

            //adds it to list
            points.Add(temp);
        }

        //sort list of Vector3
        sortedPoints = sortByXZ(points);

        //Generate the convex hull based on points
        hull = convexHull(sortedPoints, 0, randVal);

        //for loop to push several points apart
        int pushIterator = 3;
        for (int i = 0; i < pushIterator; i++)
        {
            pullPointsApart(hull);
        }

        //add more points to the convex hull
        List<Vector3> rSet = new List<Vector3>(sortedPoints.Capacity * 2);
        Vector3 disp = new Vector3();
        float diff = 1f;
        float maxDisp = 20f;
        for (int i = 0; i < sortedPoints.Count; ++i)
        {
            float dispLen = (float)Mathf.Pow(Random.Range(0.0f, 1.0f), diff) * maxDisp;
            disp.Set(0, 0, 1);
            disp = Quaternion.AngleAxis(Random.Range(0.0f, 1.0f) * 360, new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f))) * disp;
            disp.Scale(new Vector3(dispLen, 0, dispLen));
            rSet[i * 2] = sortedPoints[i];
            rSet[(i * 2) + 1] = new Vector3(sortedPoints[i].x, sortedPoints[i].y, sortedPoints[i].z);
            rSet[(i * 2) + 1] = (rSet[(i + 2) + 1] + sortedPoints[(i + 1) % sortedPoints.Count] / 2) + disp;
        }
        sortedPoints = rSet;

        //push apart again
        for (int i = 0; i < pushIterator; i++)
        {
            pullPointsApart(hull);
        }

        //fixes angle issues and pushes 
        for (int i = 0; i < 10; i++)
        {
            fixAngles(hull);
            pullPointsApart(hull);
        }

        //adds curves to track
        splineCode(sortedPoints);

        //lifts y coordinates
        for (int i = 0; i < sortedPoints.Count; i++)
        {
            float xVal = sortedPoints[i].x;
            float yVal = Random.Range(0f, 10f);
            float zVal = sortedPoints[i].z;

            sortedPoints[i].Set(xVal, yVal, zVal);
        }
    }

    //creates a spline for lines
    private void splineCode(List<Vector3> p)
    {
        
    }

    //generates a convex hull
    private List<Vector3> convexHull(List<Vector3> p, int offset, int count)
    {
        List<Vector3> newHull = new List<Vector3>(p);
        int end = offset + count;
        int size = newHull.Count;

        newHull.Clear();

        //Lower Hull
        for(int i = offset; i < end; i++)
        {
            Vector3 point = p[i];
            while (size >= 3 && ccw(point.x, point.z) <= 0)
            {
                size -= 1;
            }
            hull.Add(point);
        }

        //upper hull
        for(int i = end - 2, t = size + 1; i >= offset; i--)
        {
            Vector3 point = p[i];
            while (size >= t && ccw(point.x, point.z) <= 0)
            {
                size -= 1;
            }
            hull.Add(point);
        }

        return newHull;
    }

    //returns > 0 if counter clockwise, if clockwise return < 0, if colinear returns = 0
    private float ccw(float p3x, float p3z)
    {
        List<Vector3> newHull = new List<Vector3>(hull);
        int size = hull.Count;
        float p1x = hull[size - 4].x;
        float p1z = hull[size - 3].z;
        float p2x = hull[size - 2].x;
        float p2z = hull[size - 1].z;
        return (p2x - p1x) * (p3z - p1z) - (p2z - p1z) * (p3x - p1x);
    }

    //sort method - insertion sort
    public List<Vector3> sortByXZ(List<Vector3> p)  
    {
        List<Vector3> sortedP = new List<Vector3>(p);
        int n = sortedP.Count;

        for(int i = 0; i < n; i++)
        {
            Vector3 key = sortedP[i];
            int j = i - 1;

            while (j >= 0 && sortedP[j].x > key.x)
            {
                sortedP[j + 1] = sortedP[j];
                j--;
            }
            sortedP[j + 1] = key;
        }

        for(int i = 0; i < n; i++)
        {
                if(sortedP[i+1] != null)
                {
                    if(sortedP[i].x == sortedP[i+1].x)
                    {
                        if(sortedP[i].z > sortedP[i+1].x)
                        {
                        Vector3 temp = sortedP[i + 1];
                        sortedP[i + 1] = sortedP[i];
                        sortedP[i] = temp;
                        }
                    }
                }
            }
        
        return sortedP;
    }

    //Pushes points apart based on difficulty
    private void pullPointsApart(List<Vector3> p)
    {
        float dist = 15;
        float distSquared = Mathf.Pow(dist, 2);
        for (int i = 0; i < p.Count; i++)
        {
            for (int j = i+1; j < p.Count; j++)
            {
                if (dstSquared(p[i], p[j]) < distSquared) 
                {
                    float hx = p[j].x - p[i].x;
                    float hz = p[j].z - p[i].z;
                    float hl = (float)Mathf.Sqrt(hx * hx + hz * hz);
                    hx /= hl;
                    hz /= hl;
                    float dif = dist - hl;
                    hx *= dif;
                    hz *= dif;
                    float xjTemp = p[j].x + hx;
                    float zjTemp = p[j].z + hz;
                    float xiTemp = p[i].x - hx;
                    float ziTemp = p[i].z - hz;
                    p[j].Set(xjTemp, 0, zjTemp);
                    p[i].Set(xiTemp, 0, ziTemp);
                }
            }
        }
    }

    //fixes the angles in the code to make curves nicer
    private void fixAngles(List<Vector3> p)
    {
        for (int i = 0; i < p.Count; i++)
        {
            int prev = (i - 1 < 0) ? p.Count - 1 : i - 1;
            int next = (i + 1) % p.Count;
            float px = p[i].x - p[prev].x;
            float pz = p[i].z - p[prev].z;
            float pl = (float)Mathf.Sqrt(px * px + pz * pz);
            px /= pl;
            pz /= pl;

            float nx = p[i].x - p[next].x;
            float nz = p[i].z - p[next].z;
            nx = -nx;
            nz = -nz;
            float nl = (float)Mathf.Sqrt(nx * nx + nz * nz);
            nx /= nl;
            nz /= nl;

            float temp = (float)Mathf.Atan2(px * nz - pz * nx, px * nx + pz * nz);

            if (Mathf.Abs(temp * (180f / Mathf.PI)) <= 100) continue;

            float nTemp = 100 * Mathf.Sign(temp) * (Mathf.PI / 180);
            float diff = nTemp - temp;
            float cos = (float)Mathf.Cos(diff);
            float sin = (float)Mathf.Sin(diff);
            float newX = nx * cos - nz * sin;
            float newZ = nx * sin + nz * cos;
            newX *= nl;
            newZ *= nl;
            float xVal = p[next].x + newX;
            float zVal = p[next].z + newZ;
            p[next].Set(xVal, 0, zVal);
        }
    }

    //squares the diff of x and z on vector
    public float dstSquared(Vector3 v1, Vector3 v2)
    {
        float xd = v1.x - v2.x;
        float zd = v1.z - v2.z;
        return (xd * xd) + (zd * zd);
    }
}

