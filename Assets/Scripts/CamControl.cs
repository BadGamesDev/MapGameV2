using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Camera mainCamera;

    public Vector3 cameraPosition;

    public Vector2 minBounds;
    public Vector2 maxBounds;

    public float minOrthographicSize;
    public float maxOrthographicSize;

    public float speed;

    private void Awake()
    {
        cameraPosition = transform.position; //temporary solution

        minBounds = new Vector2(-1.5f, -2); //maybe I shouldn't hardcode this stuff?
        maxBounds = new Vector2(81, 36);
        minOrthographicSize = 2;
        maxOrthographicSize = 14;
    }

    private void Update()
    {
        speed = 10 + (mainCamera.orthographicSize); //There will be a scrool speed option
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (horizontal != 0 || vertical != 0)
        {
            cameraPosition.x += horizontal * Time.deltaTime * speed;
            cameraPosition.y += vertical * Time.deltaTime * speed;
            speed += scroll * 10; //not ideal
        }

        if (scroll < 0 && mainCamera.orthographicSize < maxOrthographicSize)
        {
            mainCamera.orthographicSize -= scroll * 10;
        }
        else if (scroll > 0 && mainCamera.orthographicSize > minOrthographicSize)
        {
            mainCamera.orthographicSize -= scroll * 10;
        }

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        Vector2 minCamBounds = minBounds + new Vector2(camWidth, camHeight);
        Vector2 maxCamBounds = maxBounds - new Vector2(camWidth, camHeight);

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, minCamBounds.x, maxCamBounds.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, minCamBounds.y, maxCamBounds.y);

        transform.position = cameraPosition;
    }

    public void CenterCamera(GameObject point)
    {
        cameraPosition.x = point.transform.position.x;
        cameraPosition.y = point.transform.position.y;

        mainCamera.orthographicSize = 2;

        mainCamera.transform.position = cameraPosition;
    }
}