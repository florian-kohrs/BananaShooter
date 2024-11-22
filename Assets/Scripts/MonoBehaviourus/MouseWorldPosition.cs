using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{

    Plane plane = new Plane(Vector3.up,Vector3.zero);

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(GetPosition());
        }
    }

    public Vector3 GetPosition()
    {
        Ray mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(mouseCameraRay, out float distance))
        {
            return mouseCameraRay.GetPoint(distance);
        }
        throw new System.Exception();
    }

}
