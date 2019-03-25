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
            manager = new RandGenManager();
            path = null;           

        }
        if (GUILayout.Button("Toggle Closed"))
        {
            Undo.RecordObject(creator, "Toggle closed");
            path.ToggleClosed();

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
            Undo.RecordObject(creator, "Add segment");
            path.AddSegment(mousePos);
        }
    }

    void Draw()
    {
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector3[] points = path.GetPointsInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
        }
        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector3 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, .1f, new Vector3(0, 1000000000000000000, 0), Handles.CylinderHandleCap);
            if (path[i] != newPos)
            {
                Undo.RecordObject(creator, "Move Point");
                path.MovePoint(i, newPos);
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
            for (int i = 1; i <manager.hull.Count; i++)
            {
               // Debug.Log(manager.hull.Count);
                path.AddSegment(manager.hull[i]);
                SceneView.RepaintAll();
            }
        }
    }
}
