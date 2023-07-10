using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Camera cam;

    public float speed;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float minOrthographicSize;
    public float maxOrthographicSize;

    private void Start()
    {        
        minBounds = new Vector2(-1.5f, -2); //maybe I shouldn't hardcode this stuff?
        maxBounds = new Vector2(81, 36);
        minOrthographicSize = 2;
        maxOrthographicSize = 18;
    }

    private void Update()
    {
        speed = 10 + (cam.orthographicSize); //There will be a scrool speed option
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        if (horizontal != 0 || vertical != 0)
        {
            pos.x += horizontal * Time.deltaTime * speed;
            pos.y += vertical * Time.deltaTime * speed;
            speed += scroll * 10; //not ideal
        }

        if (scroll < 0 && cam.orthographicSize < maxOrthographicSize)
        {
            cam.orthographicSize -= scroll * 10;
        }
        else if (scroll > 0 && cam.orthographicSize > minOrthographicSize)
        {
            cam.orthographicSize -= scroll * 10;
        }

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector2 minCamBounds = minBounds + new Vector2(camWidth, camHeight);
        Vector2 maxCamBounds = maxBounds - new Vector2(camWidth, camHeight);

        pos.x = Mathf.Clamp(pos.x, minCamBounds.x, maxCamBounds.x);
        pos.y = Mathf.Clamp(pos.y, minCamBounds.y, maxCamBounds.y);

        transform.position = pos;
    }
}