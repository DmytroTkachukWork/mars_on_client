using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectionDrawer : MonoBehaviour
{
  [SerializeField] private Material renderer_material = null;
  [SerializeField] private Transform[] points = null;

  private GameObject trail_object = null;
  private Mesh trail_mesh         = null;
  private int count = 10;

  private void Start()
  {
    if ( trail_object == null )
      init();
  }

  public void init()
  {
    trail_object = new GameObject( string.Format( "SelectionLine" ) );
    trail_object.transform.SetParent( transform, false );
    trail_object.transform.SetPositionAndRotation( Vector3.zero, Quaternion.identity );

    trail_object.AddComponent( typeof( MeshFilter ) );
    trail_object.AddComponent( typeof( MeshRenderer ) );
    trail_object.GetComponent<Renderer>().material = renderer_material;

    trail_mesh = new Mesh();
    trail_mesh.name = name + "SelectionMesh";
    trail_object.GetComponent<MeshFilter>().mesh = trail_mesh;
    generateMesh();
  }

  public void generateMesh()
  {
    if ( trail_mesh == null )
      return;

    Vector3[] new_vertices = new Vector3[points.Length];
    Vector2[] new_uv       = new Vector2[points.Length];
    int[] new_triangles    = new int[points.Length  * 6];
    int tris_length = 0;

    for ( int n = 0; n < points.Length; n+=2 )
    {
      new_vertices[n]   = points[n].position;
      new_vertices[n+1] = points[n+1].position;

      new_uv[n]   = new Vector2( 0, 0 );
      new_uv[n+1] = new Vector2( 0, 1 );

      if ( n+3 < points.Length )
      {
        new_triangles[tris_length++] = n;
        new_triangles[tris_length++] = n+1;
        new_triangles[tris_length++] = n+2;

        new_triangles[tris_length++] = n+1;
        new_triangles[tris_length++] = n+2;
        new_triangles[tris_length++] = n+3;
      }
      else
      {
        new_triangles[tris_length++] = n;
        new_triangles[tris_length++] = n+1;
        new_triangles[tris_length++] = 0;

        new_triangles[tris_length++] = n+1;
        new_triangles[tris_length++] = 0;
        new_triangles[tris_length++] = 1;
      }
    }

    trail_mesh.Clear();
    trail_mesh.vertices  = new_vertices;
    trail_mesh.uv        = new_uv;
    trail_mesh.triangles = new_triangles;
  }
}
