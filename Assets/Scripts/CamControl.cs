using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    Camera cam;

    public float speed = 10;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        speed = 10 + (cam.orthographicSize);
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

        if (scroll < 0 && cam.orthographicSize < 30)
        {
            cam.orthographicSize -= scroll * 10;
        }

        else if (scroll > 0 && cam.orthographicSize > 2)
        {
            cam.orthographicSize -= scroll * 10;
        }

        transform.position = pos;
    }
}
