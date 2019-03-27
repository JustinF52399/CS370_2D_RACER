using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path;
    RandGenManager manager = new RandGenManager();

    const float segemntSelectDstThreshold = .1f;
    int selectedSegmentIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Flatten"))
        {
            Undo.RecordObject(creator, "Flatten");
            path.SetFlat();
        }
        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(creator, "Create new");
            path.ClearPoints();
            manager.Start();
            path = null;
            Restart();

        }
        
        bool isClosed = GUILayout.Toggle(path.IsClosed, "Closed");
        if(isClosed != path.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle closed");
            path.IsClosed = isClosed;

        }
        
        bool autoSetControlPoints = GUILayout.Toggle(path.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != path.AutoSetControlPoints)
        {
            Undo.RecordObject(creator, "Toggle auto set controls");
            path.AutoSetControlPoints = autoSetControlPoints;
        }
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
        
    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

        float drawPlaneHeight = 0;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        // Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        Vector3 mousePos = mouseRay.GetPoint(dstToDrawPlane);
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split segment");
                path.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else if(!path.IsClosed)
            {

                Undo.RecordObject(creator, "Add segment");
                path.AddSegment(mousePos);
            }
        }
        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDstToAnchor = creator.anchorDiameter *.5f;
            int closestAnchorIndex = -1;
            for (int i = 0; i < path.NumPoints; i+=3)
            {
                float dst = Vector3.Distance(mousePos, path[i]);
                if(dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }
            if(closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Delete Segment");
                path.DeleteSegment(closestAnchorIndex);
            }
        }
        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = segemntSelectDstThreshold;
            int newSelectedSegmentIndex = -1;
            for (int i = 0; i < path.NumSegments; i++)
            {
                Vector3[] points = path.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }
            if(newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }
    }

    void Draw()
    {
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector3[] points = path.GetPointsInSegment(i);
            if (creator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }
            Color segmentColor = (i == selectedSegmentIndex && Event.current.shift) ? creator.selectedSegmentColor : creator.segmentColor;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2);
        }
        
        for (int i = 0; i < path.NumPoints; i++)
        {
            if (i % 3 == 0 || creator.displayControlPoints)
            {
                Handles.color = (i % 3 == 0) ? creator.anchorCol : creator.controlColor;
                float handleSize = (i % 3 == 0) ? creator.anchorDiameter : creator.controlDiameter;
                Vector3 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, handleSize, new Vector3(0, 1000000000000000000, 0), Handles.CylinderHandleCap);
                if (path[i] != newPos)
                {
                    Undo.RecordObject(creator, "Move Point");
                    path.MovePoint(i, newPos);
                }
            }
        }
    }

    void OnEnable()
    {
        creator = (PathCreator)target;
        
        
        if (creator.path == null)
        {
            manager.Start();
            Vector3 temp = manager.hull[0];
            Vector3 temp2 = manager.hull[1]; 
            creator.CreatePath(temp,temp2);
        }
       // Debug.Log(manager.hull.Count);

        path = creator.path;
        if (path.NumSegments < 10)
        {
            for (int i = 2; i <manager.hull.Count; i++)
            {
               // Debug.Log(manager.hull.Count);
                path.AddSegment(manager.hull[i]);
                SceneView.RepaintAll();
                //path.AutoSetAllControlPoints();
                //path.ToggleClosed();
            }
        }
    }
    void Restart()
    {
        Debug.Log(creator.path == null);
        Vector3 temp = manager.hull[0];
        Vector3 temp2 = manager.hull[1];
        creator.CreatePath(temp, temp2);
        

        path = creator.path;
        if (path.NumSegments < 10)
        {
            for (int i = 2; i < manager.hull.Count; i++)
            {
                // Debug.Log(manager.hull.Count);
                path.AddSegment(manager.hull[i]);
                SceneView.RepaintAll();            
                
            }
        }
        path.IsClosed = true;
        path.AutoSetAllControlPoints();
    }
}
