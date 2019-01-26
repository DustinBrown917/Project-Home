using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject followTarget;
    [SerializeField] float smoothTime = 1.0f;
    private float newX;
    private float vel;
    private Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        newX = Mathf.SmoothDamp(transform.position.x, followTarget.transform.position.x, ref vel, smoothTime);
        newPos.x = newX;
        newPos.y = transform.position.y;
        newPos.z = transform.position.z;

        transform.position = newPos;
    }
}
