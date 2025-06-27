using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ARPathfinder : MonoBehaviour
{
    public NavMeshSurface surface;   // tu NavMeshSurface
    public Transform startAnchor;    // objeto AR anclado en tu posición
    public Transform endAnchor;      // objeto AR anclado en el punto fijo

    NavMeshPath path;

    void Start()
    {
        path = new NavMeshPath();
    }

    public void CalculateRoute()
    {
        Vector3 start = startAnchor.position;
        Vector3 end = endAnchor.position;

        // Calcula la ruta en la NavMesh
        bool found = NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path);
        if (!found || path.corners.Length < 2)
        {
            Debug.LogWarning("Ruta no encontrada");
            return;
        }

        // Dibuja la ruta con una línea (opcional)
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.cyan, 10f);
        }

        /*
        // Si quieres instanciar waypoints en AR:
        foreach (var corner in path.corners)
        {
            Instantiate(puntoRA, corner, Quaternion.identity);
        }
        */
    }
}
