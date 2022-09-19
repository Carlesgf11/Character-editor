using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float rotSpeed;
    public AppControl appControl;
    private void OnMouseDrag()
    {
        if (appControl.currentCameraPos < 8)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * Input.GetAxis("Mouse X") * rotSpeed);
        }
    }

    private void Update()
    {
        if (appControl.currentCameraPos >= 8)
        {
            transform.rotation = Quaternion.Euler(1.34549487f, 332.162537f, 0.75341326f);
        }
    }
}

//Vector3(1.34549487, 332.162537, 0.75341326)