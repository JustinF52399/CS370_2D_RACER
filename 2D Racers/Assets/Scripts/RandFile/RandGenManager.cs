using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandGenManager
{
    float xRand;
    float yRand;
    float zRand;
    List<Vector3> points = new List<Vector3>();
    public List<Vector3> hull = new List<Vector3>();
    public List<Vector3> sortedPoints = new List<Vector3>();
    public CatmullRom spline;
    List<Transform> sortedTrans = new List<Transform>();
    float easy = 1f;
    float med = .5f;
    float hard = .25f;
    Path path;
    PathEditor editor;

    // Start is called before the first frame update
    public void Start()
    {
        points.Clear();
        hull.Clear();
        sortedPoints.Clear();
        sortedTrans.Clear();

        int seed = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        Random.InitState(seed);
        //Debug.Log(Random.seed.ToString());
        int randVal = Random.RandomRange(10, 20);
        //for rand iterations create rand new points of random positions and put them in a list
        for (int i = 0; i < randVal; i++)
        {
            //get a random x, y, and z from -150 to 150
            xRand = Random.Range(-150f, 150f);
            yRand = 0;
            zRand = Random.Range(-150f, 150f);

            //Print those values to debug
            //Debug.Log("(Point " + i + ") x: " + xRand.ToString() + " z: " + zRand.ToString());

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
        List<Vector3> rSet = new List<Vector3>(hull.Capacity * 2);
        Vector3 disp = new Vector3();
        float diff = easy;
        float maxDisp = 20f;
        //for (int i = 0; i < hull.Count; i++)
        //{
        //    float dispLen = (float)Mathf.Pow(Random.Range(0.0f, 1.0f), diff) * maxDisp;
        //    disp.Set(0, 0, 1);
        //    disp = Quaternion.AngleAxis(Random.Range(0.0f, 1.0f) * 360, new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f))) * disp;
        //    disp.Scale(new Vector3(dispLen, 0, dispLen));
        //    rSet.Insert(i * 2, hull[i]);
        //    rSet.Insert((i*2)+1, new Vector3(hull[i].x, hull[i].y, hull[i].z));
        //    rSet[(i * 2) + 1] += (rSet[i + 1] + hull[(i + 1) % hull.Count] / 2f) + disp;
        //}

        //hull = rSet;

        //debugs
        //for (int i = 0; i < rSet.Count; i++)
        //{
        //    //Debug.Log("(Points " + i + ") " + rSet[i].x + " & " + rSet[i].z);
        //}

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
        //splineCode(hull);

        //lifts y coordinates
        for (int i = 0; i < hull.Count; i++)
        {
            float xVal = hull[i].x;
            float yVal = Random.Range(0f, 10f);
            float zVal = hull[i].z;

            hull[i].Set(xVal, yVal, zVal);
        }

        //debugs
        //for (int i = 0; i < rSet.Count; i++)
        //{
        //    //Debug.Log("(Points " + i + ") " + rSet[i].x + " & " + rSet[i].y + " & " + rSet[i].z);
        //}
        ClockwiseVector3Comparer sorter = new ClockwiseVector3Comparer();
        hull.Sort(sorter);

       
        for (int i= hull.Count - 1; i >0; i--)
        {
            if (hull[i] == hull[i - 1])
            {
                hull.RemoveAt(i);
            }
        }
        for (int i = 0; i < hull.Count; i++)
        {
            Debug.Log(hull[i]);
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
        int size = p.Count;
        Vector3 origin = new Vector3(0, 0, 0);

        newHull.Clear();

        //Lower Hull
        for (int i = offset; i < end; i++)
        {
            Vector3 point = p[i];
            if (i != 0)
            {
                while (size >= 3 && ccw(p[i], p[i - 1], origin))
                {
                    size -= 1;
                }
                newHull.Add(point);
            }

        }

        //upper hull
        for (int i = end - 2, t = size + 1; i >= offset; i--)
        {
            Vector3 point = p[i];
            if (i != 0)
            {
                while (size >= t && ccw(p[i], p[i - 1], origin))
                {
                    size -= 1;
                }
                newHull.Add(point);
            }
        }

        return newHull;
    }

    //returns > 0 if counter clockwise, if clockwise return < 0, if colinear returns = 0
    private float ccw(float p3x, float p3z, List<Vector3> p)
    {
        List<Vector3> newHull = new List<Vector3>(p);
        int size = p.Capacity;
        float p1x = newHull[size - 2].x;
        float p1z = newHull[size - 2].z;
        float p2x = newHull[size - 1].x;
        float p2z = newHull[size - 1].z;
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
                if(i+1 < sortedP.Count)
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

    private bool ccw(Vector3 first, Vector3 second, Vector3 origin)
    {
        if (first == second)
        {
            return false;
        }

        Vector3 fOffest = first - origin;
        Vector3 sOffest = second - origin;

        float angle1 = Mathf.Atan2(fOffest.x, fOffest.z);
        float angle2 = Mathf.Atan2(sOffest.x, sOffest.z);

        if (angle1 < angle2)
        {
            return true;
        }
        else if (angle1 > angle2)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
}

public class ClockwiseVector3Comparer : IComparer<Vector3>
{
    public int Compare(Vector3 v1, Vector3 v2)
    {
        if (v1.x >= 0)
        {
            if (v2.x < 0)
            {
                return -1;
            }
            return -Comparer<float>.Default.Compare(v1.z, v2.z);
        }
        else
        {
            if (v2.x >= 0)
            {
                return 1;
            }
            return Comparer<float>.Default.Compare(v1.z, v2.z);
        }
    }
}