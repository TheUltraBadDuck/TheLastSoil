using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    public bool isFreazeCamera;
    private bool isDragMove = false;
    private bool isEdgeScroll = true;
    Vector3 lastMousePos;
    public float speed = 20f;


    private float minZoom = 0.8f, maxZoom = 3.5f, zoomMultiplier = 4f, zoomTime= 0.25f, velocity = 0f;
    private float zoomSize;

    private float MapMinX, MapMinY, MapMaxX, MapMaxY;

    private float edgeSize = 10f, scrollSpeed = 5f;
    private float scroll;
    private Vector3 originCoord;


    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera post_proccessCamera;
    [SerializeField] private Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap.CompressBounds();
        isFreazeCamera = false;
        zoomSize = mainCamera.orthographicSize;

        BoundsInt bounds = tilemap.cellBounds;

        Vector3 bottomLeft = tilemap.GetCellCenterWorld(new Vector3Int(bounds.x, bounds.y, 0));
        Vector3 topRight = tilemap.GetCellCenterWorld(new Vector3Int(bounds.x + bounds.size.x - 1, bounds.y + bounds.size.y - 1, 0));
        MapMinX =bottomLeft.x;
        MapMinY =bottomLeft.y;
        MapMaxX =topRight.x;
        MapMaxY =topRight.y;
    }

    // Update is called once per frame
    void Update()
    {

        panCamera();
    }

    private void FixedUpdate()
    {
        if (isFreazeCamera)
            return;
        ZoomCamera();
        if (isEdgeScroll)
            edgeScrolling();
    }
    void panCamera()
    {
       if (Input.GetMouseButtonDown(1))
            originCoord = post_proccessCamera.ScreenToWorldPoint(Input.mousePosition);

       if (Input.GetMouseButton(1))
        {
            Vector3 diff_coord = originCoord - post_proccessCamera.ScreenToWorldPoint(Input.mousePosition);
            post_proccessCamera.transform.position = ClampCamera(post_proccessCamera.transform.position + diff_coord);
        }
    }

    void ZoomCamera()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        zoomSize -= scroll * zoomMultiplier;
        zoomSize = Mathf.Clamp(zoomSize, minZoom, maxZoom);
        post_proccessCamera.orthographicSize = Mathf.SmoothDamp(post_proccessCamera.orthographicSize, zoomSize, ref velocity, zoomTime);
        mainCamera.orthographicSize = post_proccessCamera.orthographicSize;
        post_proccessCamera.transform.position = ClampCamera(post_proccessCamera.transform.position);
    }


    void edgeScrolling()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.mousePosition.x < edgeSize)
            horizontalInput = -1f;
        else if (Input.mousePosition.x > Screen.width - edgeSize)
            horizontalInput = 1f;

        if (Input.mousePosition.y < edgeSize)
            verticalInput = -1f;
        else if (Input.mousePosition.y > Screen.height - edgeSize)
            verticalInput = 1f;

        Vector3 scrollDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
         post_proccessCamera.transform.position = ClampCamera(post_proccessCamera.transform.position + scrollDirection * Time.deltaTime * scrollSpeed);
    }

    Vector3 ClampCamera(Vector3 target)
    {
        float camHeight = post_proccessCamera.orthographicSize;
        float camWidth = post_proccessCamera.orthographicSize * post_proccessCamera.aspect;

        float minX = MapMinX + camWidth;
        float maxX = MapMaxX - camWidth;
        float minY = MapMinY + camHeight;
        float maxY = MapMaxY - camHeight;

        float newX = Mathf.Clamp(target.x, minX, maxX);
        float newY = Mathf.Clamp(target.y, minY, maxY);



        return new Vector3(newX, newY,target.z);
    }
}
