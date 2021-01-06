using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridCreator : MonoBehaviour
{
    public float CellRadius;
    public List<Vector3> CellPositions;

    private Dictionary<Vector3, int> _verticesPoints = new Dictionary<Vector3, int>();
    private Dictionary<Vector2, int> _uvPoints = new Dictionary<Vector2, int>();
    private Mesh _mesh;
    
    private void Awake () {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        
        Generate();
    }
    
    private void Generate ()
    {
        var count = 0;
        foreach (Vector3 point in from centerPoint in CellPositions from point in new[] {
            new Vector3(centerPoint.x, 0, centerPoint.z),
            new Vector3(centerPoint.x, 0, centerPoint.z + CellRadius),
            new Vector3(centerPoint.x, 0, centerPoint.z - CellRadius),
            new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z),
            new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z + CellRadius),
            new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z - CellRadius),
            new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z),
            new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z + CellRadius),
            new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z - CellRadius)
        } where _verticesPoints.ContainsKey(point) == false select point)
        {
            _verticesPoints.Add(point, count);
            _uvPoints.Add(new Vector2(point.x, point.z), count);
            count++;
        }
        _mesh.vertices = _verticesPoints.Keys.ToArray();
        _mesh.uv = _uvPoints.Keys.ToArray();
        _mesh.tangents = Enumerable.Repeat<Vector4>(new Vector4(1f, 0f, 0f, -1f), _mesh.vertices.Length).ToArray();

        List<Color> colors = new List<Color>();
        foreach (Vector3 point in _verticesPoints.Keys)
        {
            if (_verticesPoints.ContainsKey(new Vector3(point.x, 0, point.z + CellRadius)) &&
                _verticesPoints.ContainsKey(new Vector3(point.x, 0, point.z - CellRadius)) &&
                _verticesPoints.ContainsKey(new Vector3(point.x + CellRadius, 0, point.z + CellRadius)) &&
                _verticesPoints.ContainsKey(new Vector3(point.x + CellRadius, 0, point.z)) &&
                _verticesPoints.ContainsKey(new Vector3(point.x + CellRadius, 0, point.z - CellRadius)) &&
                _verticesPoints.ContainsKey(new Vector3(point.x - CellRadius, 0, point.z + CellRadius)) &&
                _verticesPoints.ContainsKey(new Vector3(point.x - CellRadius, 0, point.z)) &&
                _verticesPoints.ContainsKey(new Vector3(point.x - CellRadius, 0, point.z - CellRadius)))
            {
                colors.Add(Color.black);
            }
            else
            {
                colors.Add(Color.white);
            }
        }
        _mesh.colors = colors.ToArray();

        List<int> trianglesList = new List<int>();
        foreach (Vector3 centerPoint in CellPositions)
        {
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z + CellRadius)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z + CellRadius)]);

            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z + CellRadius)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z)]);

            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z - CellRadius)]);

            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x + CellRadius, 0, centerPoint.z - CellRadius)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z - CellRadius)]);

            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z - CellRadius)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z - CellRadius)]);

            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z - CellRadius)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z)]);

            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z + CellRadius)]);

            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x - CellRadius, 0, centerPoint.z + CellRadius)]);
            trianglesList.Add(_verticesPoints[new Vector3(centerPoint.x, 0, centerPoint.z + CellRadius)]);
            
        }
        _mesh.triangles = trianglesList.ToArray();
        
        _mesh.RecalculateNormals();
    }
}
