using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class RouteDrawer : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("NavMesh Settings")]
    public NavMeshSurface navMeshSurface; // opcional si quieres rebake en runtime

    public LineRenderer line;
    public NavMeshPath path;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        // Opcional: Rebake dinámico si tu mesh cambia
        // if (navMeshSurface != null)
        //     navMeshSurface.BuildNavMesh();
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
            return;
        
        // 0) Debug visual
        Debug.DrawRay(startPoint.position + Vector3.up, Vector3.down, Color.red);

        // 1) Raycast al mesh
        RaycastHit rc;
        if (Physics.Raycast(startPoint.position + Vector3.up * 5, Vector3.down, out rc, 10, LayerMask.GetMask("micasa")))
        {
            Debug.DrawLine(startPoint.position + Vector3.up * 5, rc.point, Color.green);
            startPoint.position = rc.point + Vector3.up * 0.1f;
        }
        else
        {
            Debug.LogError("Raycast no impacta mesh micasa");
        }

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(startPoint.position, out hit, 2f, NavMesh.AllAreas))
        {
            Debug.LogError("Start fuera de NavMesh aun con SampleRadius=2");
            line.positionCount = 0;
            return;
        }

        /*
        // 1) Snap ambos al NavMesh
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(startPoint.position, out hit, 0.5f, NavMesh.AllAreas))
        {
            Debug.LogError("Start fuera de NavMesh");
            line.positionCount = 0;
            return;
        }
        */
        Vector3 src = hit.position;

        if (!NavMesh.SamplePosition(endPoint.position, out hit, 0.5f, NavMesh.AllAreas))
        {
            Debug.LogError("End fuera de NavMesh");
            line.positionCount = 0;
            return;
        }
        Vector3 dst = hit.position;

        // 2) Calcula la ruta
        if (!NavMesh.CalculatePath(src, dst, NavMesh.AllAreas, path))
        {
            Debug.LogWarning("CalculatePath devolvió false");
            line.positionCount = 0;
            return;
        }
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogWarning("Path incompleto: " + path.status);
            line.positionCount = 0;
            return;
        }

        // 3) Dibuja
        line.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i] + Vector3.up * 0.05f);
        }


        /*
        // Calcula la ruta (esquiva obstáculos)
        bool found = NavMesh.CalculatePath(
            startPoint.position,
            endPoint.position,
            NavMesh.AllAreas,
            path   
        );
        Debug.Log("Paso por calculatepath");
        if (!found || path.status != NavMeshPathStatus.PathComplete)
        {
            line.positionCount = 0;
            Debug.Log("no calculo nada we");
            return;
        }

        // Dibuja la línea siguiendo cada esquina del path
        Vector3[] corners = path.corners;
        line.positionCount = corners.Length;
        for (int i = 0; i < corners.Length; i++)
        {
            line.SetPosition(i, corners[i] + Vector3.up * 0.05f);
            Debug.Log("dibujo la linea");
            // sube un poco en Y para que la línea no quede zanjada en el suelo
        }
        */
    }
}