using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    public bool isFreazeCamera;
    private bool isDragMove = false;
    private bool isEdgeScroll = true;
    Vector3 lastMousePos;
    public float speed = 20f;


    private float minZoom = 0.8f, maxZoom = 2f, zoomMultiplier = 4f, zoomTime= 0.25f, velocity = 0f;
    private float zoomSize;

    private float MapMinX, MapMinY, MapMaxX, MapMaxY, cameraWidth, cameraHeight;

    private float edgeSize = 10f, scrollSpeed = 5f;
    private float scroll;


    [SerializeField] private Camera mainCamera;
    [SerializeField] private Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        isFreazeCamera = false;
        zoomSize = mainCamera.orthographicSize;
    
        Vector3 cellsize = tilemap.cellSize;
        Vector3 topRight = tilemap.CellToWorld(new Vector3Int(5, 45, 0)) + new Vector3(cellsize.x,cellsize.y,0);
        Vector3 bottomLeft = tilemap.CellToWorld(new Vector3Int(-46, -6, 0));
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
        {
            isDragMove = true;
            isEdgeScroll = false;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isDragMove = false;
            isEdgeScroll = true;

        }

        if (isDragMove)
        {
            Vector3 mouseMovementDelta = Input.mousePosition - lastMousePos;
            transform.position-= mouseMovementDelta * Time.deltaTime* speed * zoomSize;
            //LimitCameraMovement();
            lastMousePos = Input.mousePosition;
        }
    }

    void ZoomCamera()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        zoomSize -= scroll * zoomMultiplier;
        zoomSize = Mathf.Clamp(zoomSize, minZoom, maxZoom);
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoomSize, ref velocity, zoomTime);
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
         transform.position += scrollDirection * Time.deltaTime * scrollSpeed;
        //LimitCameraMovement();
    }

    void LimitCameraMovement()
    {
        cameraWidth = mainCamera.aspect * mainCamera.orthographicSize;
        cameraHeight = mainCamera.orthographicSize;
        float minX = MapMinX + cameraWidth;
        float minY = MapMinY + cameraHeight;
        float maxX = MapMaxX - cameraWidth;
        float maxY = MapMaxY - cameraHeight;

        Vector3 pos=transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y =Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
