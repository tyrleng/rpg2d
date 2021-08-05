using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame

    public int speed;
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(xAxis * Time.deltaTime * speed, yAxis * Time.deltaTime * speed, 0.0f));
    }
}
