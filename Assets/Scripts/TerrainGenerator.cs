using UnityEngine;
using Unity.AI.Navigation; // Para el NavMesh

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int width = 50;  
    [SerializeField] private int depth = 50;  
    [SerializeField] private float scale = 20f;  
    [SerializeField] private float heightMultiplier = 5f; 
    [SerializeField] private Material terrainMaterial;
    
    private NavMeshSurface navMeshSurface;
    private GameObject currentTerrain;  // Referencia al terreno actual

    // Tipos de biomas (se generará aleatoriamente)
    private enum TerrainType { Plains, Mountains, River, MountainsAndRiver }
    private TerrainType selectedTerrain;

    public void mainAction()
    {
        GenerateTerrain();
    }
    
    private void GenerateTerrain()
    {
        // Si ya existe un terreno, lo destruimos antes de generar uno nuevo
        if (currentTerrain != null)
        {
            Destroy(currentTerrain);
        }
        selectedTerrain = (TerrainType)Random.Range(0, 4);
        // Crear un nuevo GameObject para el terreno
        currentTerrain = new GameObject("Procedural Terrain");
        currentTerrain.tag = "Ground";

        // Agregar componentes al nuevo terreno
        MeshFilter meshFilter = currentTerrain.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = currentTerrain.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = currentTerrain.AddComponent<MeshCollider>();

        // Crear NavMeshSurface para la navegación
        NavMeshSurface navMeshSurface = currentTerrain.AddComponent<NavMeshSurface>();
        navMeshSurface.collectObjects = CollectObjects.Children;

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Crear los vértices del terreno
        Vector3[] vertices = new Vector3[(width + 1) * (depth + 1)];
        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float y = GenerateHeight(x, z);  // Genera la altura de cada vértice
                vertices[z * (width + 1) + x] = new Vector3(x, y, z);
            }
        }

        // Crear los triángulos para la malla
        int[] triangles = new int[width * depth * 6];
        int vert = 0, tris = 0;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // Asignar los vértices y triángulos al mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();  // Calcular las normales para las sombras
        meshCollider.sharedMesh = mesh;  // Asignar la malla al colisionador
        meshRenderer.material = terrainMaterial;  // Asignar el material al terreno

        // Generar el NavMesh después de crear el terreno
        navMeshSurface.BuildNavMesh();
    }

    // Función para generar la altura basada en el tipo de terreno
    private float GenerateHeight(int x, int z)
    {
        float y = Mathf.PerlinNoise(x * scale / width, z * scale / depth) * heightMultiplier;

        switch (selectedTerrain)
        {
            case TerrainType.Plains:
                // Terreno plano con pequeñas variaciones
                y *= 0.3f;
                break;

            case TerrainType.Mountains:
                // Montañas altas con más irregularidad
                y *= 1.5f;
                break;

            case TerrainType.River:
                // Río: bajar la altura en el centro del mapa
                float river = Mathf.Sin((x / (float)width) * Mathf.PI) * 5f;
                y -= river;
                break;

            case TerrainType.MountainsAndRiver:
                // Mezcla de montañas y ríos
                float mountain = Mathf.PerlinNoise(x * scale / width, z * scale / depth) * 2f;
                float riverEffect = Mathf.Sin((x / (float)width) * Mathf.PI) * 5f;
                y = (y * 1.5f) - riverEffect + mountain;
                break;
        }

        return Mathf.Clamp(y, 0, heightMultiplier * 2);
    }
}
