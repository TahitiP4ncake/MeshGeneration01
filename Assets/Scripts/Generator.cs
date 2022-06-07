using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MeshType
{
    Quad,
    Rect,
    Cube,
    SmoothCube,
    Triangle,
    Circle,
    Disc,
    Cone,
    Cylinder

}

public enum ColorMode
{
    Uniform,
    Gradient
}


public class Generator : MonoBehaviour
{

    public MeshType meshType;

    [Space] [Space] 
    
    public int resolution = 8;
    public int section = 1;

    public AnimationCurve sizeCurve;
    
    public float size = 1;
    public float thickness = .5f;
    [Space]
    public float xSize = 1;
    public float ySize = 1;
    public float zSize = 1;
    [Space]
    public Vector2 uvTile = Vector2.one;
    [Space] 
    public bool flipFaces;
    
    public bool createUV;
    public bool setColor;
    public bool deletePrevious;
    [Space] 
    
    public ColorMode colorMode;
    
    public Color color;
    public Gradient gradientColor;
    [Space]
    public Material mat;
    [Space]
    public GameObject lastMesh;

    [Space]
    public bool showVertices;
    public Vector3[] vertex;
    public int[] triangles;


    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer renderer;
    


    void OnValidate()
    {
        GenerateMesh();
    }

    IEnumerator DestroyEndOfFrame(GameObject _objectToDestroy)
    {
        yield return null;
        DestroyImmediate(_objectToDestroy);
    }
    
    [ContextMenu("Create Mesh")]
    public void GenerateMesh()
    {
        if (deletePrevious)
        {
            if (lastMesh != null)
            {
                StartCoroutine(DestroyEndOfFrame(lastMesh));
            }
        }

        

        if (lastMesh == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.position = transform.position;
            gameObject.transform.rotation = transform.rotation;
            lastMesh = gameObject;
            lastMesh.transform.SetParent(transform);
            
            if (meshFilter == null)
            {
                meshFilter = lastMesh.AddComponent<MeshFilter>();
            }

            if (renderer == null)
            {
                renderer = lastMesh.AddComponent<MeshRenderer>();
                renderer.material = mat;
            }
        }

        

        switch (meshType)
        {
            case MeshType.Quad:
                lastMesh = GenerateQuad();
                break;
            case MeshType.Rect:
                lastMesh = GenerateRect();
                break;
            case MeshType.SmoothCube:
                lastMesh = GenerateCubeSmooth();
                break;
            case MeshType.Cube:
                lastMesh = GenerateCube();
                break;
            case MeshType.Triangle:
                lastMesh = GenerateTriangle();
                break;
            case MeshType.Disc:
                lastMesh = GenerateDisc();
                break;
            case MeshType.Circle:
                lastMesh = GenerateCircle();
                break;
            case MeshType.Cylinder:
                GenerateCylinderSubdiv();
                break;
            case MeshType.Cone:
                lastMesh = GenerateCone();
                break;
        }
        
        lastMesh.transform.SetParent(transform);
        
    }

    public GameObject GenerateTriangle()
    {
        print("fais un triangle stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[3];
        int[] triangles = new int[3];

        vert[0] =  new Vector3(0, ySize/2, 0) ;
        vert[1] =  new Vector3(xSize/2, -ySize/2, 0) ;
        vert[2] =  new Vector3(-xSize/2, -ySize/2, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
     

        mesh.vertices = vert;
        mesh.triangles = triangles;

        if (createUV)
        {
            Vector2[] uv = new Vector2[3];
            
            uv[0] = new Vector2(0.5f,1);
            uv[1] = new Vector2(1,0);
            uv[2] = new Vector2(0,0);

            mesh.uv = uv;
        }
        
        mesh.SetColors(SetVertexColor(vert.Length));
        
        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "quad";

        return gameObject;
    }
    
    public GameObject GenerateQuad()
    {
        print("fais un quad stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[4];
        int[] triangles = new int[6];

        vert[0] =  new Vector3(-1, 1, 0) * size/2;
        vert[1] =  new Vector3(1, 1, 0) * size / 2;
        vert[2] =  new Vector3(-1, -1, 0) * size / 2;
        vert[3] =  new Vector3(1, -1, 0) * size / 2;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.vertices = vert;
        mesh.triangles = triangles;

        if (createUV)
        {
            Vector2[] uv = new Vector2[4];
            
            uv[0] = new Vector2(0,uvTile.y);
            uv[1] = new Vector2(uvTile.x,uvTile.y);
            uv[2] = new Vector2(0,0);
            uv[3] = new Vector2(uvTile.x,0);

            mesh.uv = uv;
        }
        
        mesh.SetColors(SetVertexColor(vert.Length));
        
        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "quad";

        return gameObject;
    }
    
    public GameObject GenerateRect()
    {
        print("fais un rect stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[4];
        int[] triangles = new int[6];

        vert[0] =  new Vector3(-xSize/2, ySize/2, 0)  ;
        vert[1] =  new Vector3(xSize/2, ySize/2, 0) ;
        vert[2] =  new Vector3(-xSize/2, -ySize/2, 0) ;
        vert[3] =  new Vector3(xSize/2, -ySize/2, 0) ;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.vertices = vert;
        mesh.triangles = triangles;

        if (createUV)
        {
            Vector2[] uv = new Vector2[4];
            
            uv[0] = new Vector2(0,uvTile.y);
            uv[1] = new Vector2(uvTile.x,uvTile.y);
            uv[2] = new Vector2(0,0);
            uv[3] = new Vector2(uvTile.x,0);

            mesh.uv = uv;
        }
        
        mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "rect";
        return gameObject;
    }
    
    public GameObject GenerateCubeSmooth()
    {
        print("fais un cube stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[8];
        int[] triangles = new int[36];

        vert[0] =  new Vector3(1, 1, -1) * size/2;
        vert[1] =  new Vector3(1, 1, 1) * size / 2;
        vert[2] =  new Vector3(1, -1, -1) * size / 2;
        vert[3] =  new Vector3(1, -1, 1) * size / 2;
        vert[4] =  new Vector3(-1, 1, 1) * size/2;
        vert[5] =  new Vector3(-1, -1, 1) * size / 2;
        vert[6] =  new Vector3(-1, 1, -1) * size / 2;
        vert[7] =  new Vector3(-1, -1, -1) * size / 2;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;
        triangles[6] = 1;
        triangles[7] = 4;
        triangles[8] = 3;
        triangles[9] = 4;
        triangles[10] = 5;
        triangles[11] = 3;
        triangles[12] = 4;
        triangles[13] = 6;
        triangles[14] = 5;
        triangles[15] = 6;
        triangles[16] = 7;
        triangles[17] = 5;
        triangles[18] = 6;
        triangles[19] = 0;
        triangles[20] = 7;
        triangles[21] = 0;
        triangles[22] = 2;
        triangles[23] = 7;
        triangles[24] = 0;
        triangles[25] = 6;
        triangles[26] = 1;
        triangles[27] = 6;
        triangles[28] = 4;
        triangles[29] = 1;
        triangles[30] = 3;
        triangles[31] = 5;
        triangles[32] = 2;
        triangles[33] = 5;
        triangles[34] = 7;
        triangles[35] = 2;
        
        mesh.vertices = vert;
        mesh.triangles = triangles;

        if (createUV)
        {
            Vector2[] uv = new Vector2[8];
            
            uv[0] = new Vector2(0,uvTile.y);
            uv[1] = new Vector2(uvTile.x,uvTile.y);
            uv[2] = new Vector2(0,0);
            uv[3] = new Vector2(uvTile.x,0);
            uv[4] = new Vector2(0,uvTile.y);
            uv[5] = new Vector2(uvTile.x,uvTile.y);
            uv[6] = new Vector2(0,0);
            uv[7] = new Vector2(uvTile.x,0);
            
            mesh.uv = uv;
        }
        
        mesh.SetColors(SetVertexColor(vert.Length));
        

        mesh.RecalculateNormals();
        
        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "smooth cube";

        return gameObject;
    }
    
    public GameObject GenerateCube()
    {
        print("fais un cube stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[24];
        int[] triangles = new int[36];

        vert[0] =  new Vector3(1, 1, -1) * size/2;
        vert[1] =  new Vector3(1, 1, 1) * size / 2;
        vert[2] =  new Vector3(1, -1, -1) * size / 2;
        vert[3] =  new Vector3(1, -1, 1) * size / 2;
        vert[4] =  new Vector3(1, 1, 1) * size/2;
        vert[5] =  new Vector3(-1, 1, 1) * size / 2;
        vert[6] =  new Vector3(1, -1, 1) * size / 2;
        vert[7] =  new Vector3(-1, -1, 1) * size / 2;
        vert[8] =  new Vector3(1, 1, -1) * size/2;
        vert[9] =  new Vector3(-1, 1, -1) * size / 2;
        vert[10] =  new Vector3(1, 1, 1) * size / 2;
        vert[11] =  new Vector3(-1, 1, 1) * size / 2;
        vert[12] =  new Vector3(-1, 1, 1) * size/2;
        vert[13] =  new Vector3(-1, 1, -1) * size / 2;
        vert[14] =  new Vector3(-1, -1, 1) * size / 2;
        vert[15] =  new Vector3(-1, -1, -1) * size / 2;
        vert[16] =  new Vector3(-1, 1, -1) * size/2;
        vert[17] =  new Vector3(1, 1, -1) * size / 2;
        vert[18] =  new Vector3(-1, -1, -1) * size / 2;
        vert[19] =  new Vector3(1, -1, -1) * size / 2;
        vert[20] =  new Vector3(1, -1, 1) * size/2;
        vert[21] =  new Vector3(-1, -1, 1) * size / 2;
        vert[22] =  new Vector3(1, -1, -1) * size / 2;
        vert[23] =  new Vector3(-1, -1, -1) * size / 2;

        for (int i = 0; i < 6; i++)
        {
            int t = i * 6;
            int v = i * 4;
            triangles[t] = v;
            triangles[t+1] = v+1;
            triangles[t+2] = v+2;
            triangles[t+3] = v+1;
            triangles[t+4] = v+3;
            triangles[t+5] = v+2;
        }
        
        mesh.vertices = vert;
        mesh.triangles = triangles;

        if (createUV)
        {
            Vector2[] uv = new Vector2[24];
            
            for (int i = 0; i < 6; i++)
            {
                int t = i * 4;
                uv[t] = new Vector2(0,uvTile.y);
                uv[t+1] =new Vector2(uvTile.x,uvTile.y);
                uv[t+2] = new Vector2(0,0);
                uv[t+3] = new Vector2(uvTile.x,0);

            }
            
            mesh.uv = uv;
        }

        mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();
        
        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "cube";

        return gameObject;
    }
    
    public GameObject GenerateDisc()
    {
        print("fais un disc stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[resolution * 2 + 1];
        int[] triangles = new int[resolution*3];
        Vector2[] uv = new Vector2[vert.Length];

        //edge
        for (int i = 0; i <= resolution; i++)
        {
            float angle = (float) i / (resolution) * Mathf.PI*2;

            int index = i * 2;
            
            vert[index] = new Vector3(size * Mathf.Cos(angle),0, size * Mathf.Sin(angle));
            uv[index] = new Vector2( (float) i / (resolution) * uvTile.x ,0);
        }
        
        //center
        for (int i = 0; i < resolution; i++)
        {
            int index = i * 2 +1;

            vert[index] = Vector3.zero;
            
            float angle = (float) i / (resolution);
            
            uv[index] = new Vector2( (angle +  1f/resolution/2f )* uvTile.x ,1 * uvTile.y);
        }



        int step = 0;
        for (int i = 0; i < resolution; i++)
        {
            int t = i * 3;
            
            triangles[t] = step;
            triangles[t+1] = step+1;
            triangles[t+2] = step+2;

            step += 2;
        }

        mesh.vertices = vert;
        mesh.triangles = triangles;
        mesh.uv = uv;
        
        // if (createUV)
        // {
        //     Color[] colors = new Color[vert.Length];
        //     Vector2[] uv = new Vector2[vert.Length];
        //
        //     uv[0] = new Vector2(.5f,0);
        //     colors[0] = Color.black;
        //     
        //     for (int i = 0; i < uv.Length-1; i++)
        //     {
        //         float xPos = (float) (i) / (uv.Length - 1);
        //         //print(xPos);
        //         uv[i+1] = new Vector2(xPos,1);
        //         
        //         colors[i+1] = new Color(xPos,xPos,xPos,1);
        //     }
        //     
        //     mesh.colors = colors;
        //     mesh.uv = uv;
        // }


        //mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "disc";
        return gameObject;
    }
    
    public GameObject GenerateCircle()
    {
        print("fais un cercle stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[resolution * 2 + 2];
        int[] triangles = new int[resolution*2*3];
        Vector2[] uv = new Vector2[vert.Length];

        //edge
        for (int i = 0; i <= resolution; i++)
        {
            float angle = (float) i / (resolution) * Mathf.PI*2;

            int index = i * 2;
            
            vert[index] = new Vector3((size +thickness) * Mathf.Cos(angle),0, (size +thickness) * Mathf.Sin(angle));
            uv[index] = new Vector2( (float) i / (resolution) * uvTile.x,uvTile.y);
        }
        
        //center
        for (int i = 0; i <= resolution; i++)
        {
            float angle = (float) i / (resolution) * Mathf.PI*2;

            int index = i * 2+1;
            
            vert[index] = new Vector3(size * Mathf.Cos(angle),0, size * Mathf.Sin(angle));
            uv[index] = new Vector2( (float) i / (resolution) * uvTile.x ,0);
        }



        int step = 0;
        for (int i = 0; i < resolution*2; i+=2)
        {
            int t = i * 3;
            
            triangles[t] = step;
            triangles[t+1] = step+1;
            triangles[t+2] = step+2;
            
            
            triangles[t+3] = step+2;
            triangles[t+4] = step+1;
            triangles[t+5] = step+3;

            step += 2;
        }

        mesh.vertices = vert;
        mesh.triangles = triangles;
        mesh.uv = uv;
        
        
        mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "disc";
        return gameObject;
    }
    
    public GameObject GenerateCone()
    {
        print("fais un disc stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[resolution * 2 + 1];
        int[] triangles = new int[resolution*3];
        Vector2[] uv = new Vector2[vert.Length];

        //edge
        for (int i = 0; i <= resolution; i++)
        {
            float angle = (float) i / (resolution) * Mathf.PI*2;

            int index = i * 2;
            
            vert[index] = new Vector3(size * Mathf.Cos(angle),0, size * Mathf.Sin(angle));
            uv[index] = new Vector2( (float) i / (resolution) * uvTile.x,0);
        }
        
        //center
        for (int i = 0; i < resolution; i++)
        {
            int index = i * 2 +1;

            vert[index] = Vector3.up * ySize;
            
            float angle = (float) i / (resolution);
            
            uv[index] = new Vector2( (angle +  1f/resolution/2f) * uvTile.x , uvTile.y);
        }



        int step = 0;
        for (int i = 0; i < resolution; i++)
        {
            int t = i * 3;
            
            triangles[t] = step;
            triangles[t+1] = step+1;
            triangles[t+2] = step+2;

            step += 2;
        }

        mesh.vertices = vert;
        mesh.triangles = triangles;
        mesh.uv = uv;
        
        // if (createUV)
        // {
        //     Color[] colors = new Color[vert.Length];
        //     Vector2[] uv = new Vector2[vert.Length];
        //
        //     uv[0] = new Vector2(.5f,0);
        //     colors[0] = Color.black;
        //     
        //     for (int i = 0; i < uv.Length-1; i++)
        //     {
        //         float xPos = (float) (i) / (uv.Length - 1);
        //         //print(xPos);
        //         uv[i+1] = new Vector2(xPos,1);
        //         
        //         colors[i+1] = new Color(xPos,xPos,xPos,1);
        //     }
        //     
        //     mesh.colors = colors;
        //     mesh.uv = uv;
        // }


        //mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "disc";
        return gameObject;
    }
    
    public GameObject GenerateCylinder()
    {
        print("fais un cylindre stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[resolution * 2 + 2];
        int[] triangles = new int[resolution*2*3];
        Vector2[] uv = new Vector2[vert.Length];

        //edge
        for (int i = 0; i <= resolution; i++)
        {
            float angle = (float) i / (resolution) * Mathf.PI*2;

            int index = i * 2;
            
            vert[index] = new Vector3((size ) * Mathf.Cos(angle),flipFaces?ySize:0, (size ) * Mathf.Sin(angle));
            uv[index] = new Vector2( (float) i / (resolution) * uvTile.x ,1 * uvTile.y);
        }
        
        //center
        for (int i = 0; i <= resolution; i++)
        {
            float angle = (float) i / (resolution) * Mathf.PI*2;

            int index = i * 2+1;
            
            vert[index] = new Vector3(size * Mathf.Cos(angle),flipFaces?0:ySize, size * Mathf.Sin(angle));
            uv[index] = new Vector2( (float) i / (resolution) * uvTile.x,0* uvTile.y);
        }



        int step = 0;
        for (int i = 0; i < resolution*2; i+=2)
        {
            int t = i * 3;
            
            triangles[t] = step;
            triangles[t+1] = step+1;
            triangles[t+2] = step+2;
            
            
            triangles[t+3] = step+2;
            triangles[t+4] = step+1;
            triangles[t+5] = step+3;

            step += 2;
        }

        mesh.vertices = vert;
        mesh.triangles = triangles;
        mesh.uv = uv;
        
        
        mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "disc";
        return gameObject;
    }
    
    public GameObject GenerateCylinderSubdiv()
    {
        //print("fais un cylindre stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[resolution * (section+1) + (section+1)];
        triangles = new int[resolution*2*3 * section];
        Vector2[] uv = new Vector2[vert.Length];
        Vector3[] normals = new Vector3[vert.Length];
        
        Color[] colors = new Color[vert.Length];
        
        vertex = new Vector3[vert.Length];
        
        for (int j = 0; j <= section; j++)
        {

            for (int i = 0; i <= resolution; i++)
            {
                float angle = (float) i / (resolution) * Mathf.PI * 2;


                int index = i +  j *(resolution+1)  ;

                float height = ((float) j / section);
                
                vert[index] = new Vector3((sizeCurve.Evaluate(height) * size * xSize) * Mathf.Cos(angle), height*ySize, (sizeCurve.Evaluate(height) * size * zSize) * Mathf.Sin(angle));
                uv[index] = new Vector2( (float) i / (resolution) * uvTile.x ,height * uvTile.y);
                vertex[index] = vert[index];

                if (colorMode == ColorMode.Uniform)
                {
                    colors[index] = color;
                }
                else
                {
                    colors[index] = gradientColor.Evaluate(height);
                }
                
            }
        
        }
        
        for (int j = 0; j < section; j++)
        {
            for (int i = 0; i < resolution; i++)
            {
                //print(i);
                int triangleIndex = i * 3*2 +  (resolution*2 )*3 * j;

                int step = i + j * (resolution+1);
                
                //print(triangleIndex);
                //print(triangleIndex);

                if (flipFaces)
                {
                    triangles[triangleIndex] = step;
                    triangles[triangleIndex+2] = step+resolution+1;
                    triangles[triangleIndex+1] = step+1;
                
                    triangles[triangleIndex+3] = step+1;
                    triangles[triangleIndex+5] = step+resolution+1;
                    triangles[triangleIndex+4] = step+resolution+2;
                }
                else
                {
                    triangles[triangleIndex] = step;
                    triangles[triangleIndex+1] = step+resolution+1;
                    triangles[triangleIndex+2] = step+1;
                
                    triangles[triangleIndex+3] = step+1;
                    triangles[triangleIndex+4] = step+resolution+1;
                    triangles[triangleIndex+5] = step+resolution+2;
                }
            }
        }
        

        mesh.vertices = vert;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.colors = colors;

        //mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();
        normals = mesh.normals;
        
        for (int j = 0; j <= section; j++)
        {
            
            
            int indexA = 0 +  j *(resolution+1)  ;
            int indexB = resolution +  j *(resolution+1)  ;

            Vector3 newNormal = (normals[indexB] + normals[indexA])/2;
            normals[indexA] =newNormal;
            normals[indexB] =newNormal;
        }
        
        mesh.normals = normals;

        meshFilter.mesh = mesh;
        mesh.name = "cylindre";
        lastMesh.name = "cylindre";
        return gameObject;
    }
    
    public GameObject GenerateDiscBadUV()
    {
        print("fais un disc stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[resolution+1];
        int[] triangles = new int[resolution*3];


        vert[0] = Vector3.zero;
        
        for (int i = 0; i < vert.Length-1; i++)
        {
            float angle = (float) i / (vert.Length-1) * Mathf.PI*2;
            //print(angle);
               vert[i+1] = new Vector3(size * Mathf.Cos(angle),0, size * Mathf.Sin(angle)); 
        }
        
        
        for (int i = 0; i < resolution; i++)
        {
            int t = i * 3;
            
            int lastVert = Mathf.Max((i+2)%(resolution+1),1);
            //print("triangle " + 0 + ", " + lastVert + ", " + (i+1) );
            
            triangles[t] = 0;
            triangles[t+1] = lastVert;
            triangles[t+2] = (i+1);
        }

        

        mesh.vertices = vert;
        mesh.triangles = triangles;
        
        if (createUV)
        {
            Vector2[] uv = new Vector2[resolution+1];

            uv[0] = new Vector2(.5f,0);
            
            for (int i = 0; i < uv.Length-1; i++)
            {
                uv[i+1] = new Vector2((float) (i) /(uv.Length-1),1);
            }

            mesh.uv = uv;
        }

        // if (createUV)
        // {
        //     Vector2[] uv = new Vector2[4];
        //     
        //     uv[0] = new Vector2(0,1);
        //     uv[1] = new Vector2(1,1);
        //     uv[2] = new Vector2(0,0);
        //     uv[3] = new Vector2(1,0);
        //
        //     mesh.uv = uv;
        // }
        
        mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "disc";
        return gameObject;
    }

     public GameObject GenerateDiscBetterUVButStillNotThere()
    {
        print("fais un disc stp");
        
        Mesh mesh = new Mesh();
        
        Vector3[] vert = new Vector3[resolution+2];
        int[] triangles = new int[resolution*3];


        vert[0] = Vector3.zero;
        
        for (int i = 0; i < vert.Length-1; i++)
        {
            float angle = (float) i / (vert.Length-2) * Mathf.PI*2;
            //print(angle);
            vert[i+1] = new Vector3(size * Mathf.Cos(angle),0, size * Mathf.Sin(angle)); 
        }
        
        //vert[vert.Length-1] = vert[1]; 

        int step = 0;
        for (int i = 1; i <= resolution; i++)
        {
            
            int t = step * 3;
            print("triangle");
            //int lastVert = Mathf.Max((i+2)%(resolution+1),1);
            //int lastVert = Mathf.Max((i+2)%(resolution+1),1);
            //print("triangle " + 0 + ", " + lastVert + ", " + (i+1) );
            
            triangles[t] = 0;
            triangles[t+1] = i+1;
            triangles[t+2] = i;
            step++;
        }

        

        mesh.vertices = vert;
        mesh.triangles = triangles;
        
        if (createUV)
        {
            Color[] colors = new Color[vert.Length];
            Vector2[] uv = new Vector2[vert.Length];

            uv[0] = new Vector2(.5f,0);
            colors[0] = Color.black;
            
            for (int i = 0; i < uv.Length-1; i++)
            {
                float xPos = (float) (i) / (uv.Length - 1);
                //print(xPos);
                uv[i+1] = new Vector2(xPos,1);
                
                colors[i+1] = new Color(xPos,xPos,xPos,1);
            }
            
            mesh.colors = colors;
            mesh.uv = uv;
        }


        //mesh.SetColors(SetVertexColor(vert.Length));

        mesh.RecalculateNormals();

        GameObject gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        gameObject.name = "disc";
        return gameObject;
    }

     private void OnDrawGizmos()
     {
         if (showVertices)
         {
             for (int i = 0; i < vertex.Length; i++)
             {
                 Handles.Label(vertex[i], i.ToString());
             }
         }
     }

     Color[] SetVertexColor(int vertexAmount)
    {
        if (setColor)
        {
            Color[] colors = new Color[vertexAmount];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }

            return colors;
        }

        return null;
    }
}
