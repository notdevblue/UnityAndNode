using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string frontAxisName = "Vertical";
    public string rightAxisName = "Horizontal";
    public string fireButtonName = "Fire1";

    public float frontMove { get; private set; }
    public float rightMove { get; private set; }
    public bool fire { get; private set; }

    public Vector3 mousePos { get; private set; }

    // Update is called once per frame
    void Update()
    {
        frontMove = Input.GetAxis(frontAxisName);
        rightMove = Input.GetAxis(rightAxisName);
        fire = Input.GetButtonDown(fireButtonName);

        // 2D 카메라에서만 가능. 3D 는 이렇게하면 안됨
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mp.z = 0;
        mousePos = mp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, 0.1f);
    }
}
