using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class InputManager : MonoBehaviour
{
    private Vector3 lastPosition;
    
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask placementLayerMask;

    /** This method returns the coordinates of the point pointed (hover) by mouse */
    public Vector3 GetMouseCursorPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = camera.nearClipPlane; // avoids selecting not rendered objects

        Ray ray = camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point; // update if a collision occours
        }

        return lastPosition;
    }
    
}
