using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Transform[] focusObjects;

    [Header("Zoom")]
    [SerializeField, Tooltip("Cam z pos when players are furthest apart")] float maxZPos = -4;
    [SerializeField, Tooltip("Cam z pos when players are closest")] float minZPos = 6;
    [SerializeField, Tooltip("Margin on the side of each player")] float hMargin = 3;
    [SerializeField] AnimationCurve zPosCurve;
    [SerializeField, Tooltip("Max Distance possible between players")] float maxXDistance = 28;
    [SerializeField, Tooltip("Min Distance possible between players")] float minXDistance = 5;

    [Header("Y pos")]
    [SerializeField] float maxYpos = 2.37f;
    [SerializeField] float minYpos = -0.77f;

    [Header("Misc"), Space(20)]
    [SerializeField] GameObject foreGroundLayer;
    [SerializeField, Tooltip("Camera tracking speed")] float trackingSpeed = 4;

    const float movSpeed = 20;
    [SerializeField] float maxXPos = 9;
    const float foregroundMaxXPos = 2;

    private void Update()
    {
        KeepObjectsInFocus();
    }

    void KeepObjectsInFocus()
    {
        Vector3 focusCenter = Vector3.zero;

        float currentHDistance;
        float distanceRatio;
        float xPosRatio;
        Vector3 furthestLeft = focusObjects[0].position;
        Vector3 furthestRight = focusObjects[1].position;
        Vector3 finalCameraPos = transform.localPosition;
        Vector3 finalForegroundPos = foreGroundLayer.transform.localPosition;

        foreach (Transform t in focusObjects)
        {
            focusCenter += t.position;
            if (t.position.x < furthestLeft.x)
            {
                furthestLeft = t.position;
            }
            else if (t.position.x > furthestRight.x)
            {
                furthestRight = t.position;
            }
        }

        currentHDistance = Vector3.Distance(furthestLeft, furthestRight) + hMargin; 
        distanceRatio = Mathf.InverseLerp(minXDistance, maxXDistance, currentHDistance); //1 = furthest apart, 0 = closest
        focusCenter = focusCenter / focusObjects.Length;
        focusCenter.x = Mathf.Clamp(focusCenter.x, -maxXPos, maxXPos);
        xPosRatio = Mathf.InverseLerp(-maxXPos, maxXPos, focusCenter.x);

        finalForegroundPos.x = Mathf.Lerp(foregroundMaxXPos, -foregroundMaxXPos, xPosRatio);
        foreGroundLayer.transform.localPosition = finalForegroundPos;

        finalCameraPos.x = Mathf.Lerp(focusCenter.x, 0, distanceRatio);
        finalCameraPos.y = Mathf.Lerp(minYpos, maxYpos, zPosCurve.Evaluate(distanceRatio));
        finalCameraPos.z = Mathf.Lerp(minZPos, maxZPos, zPosCurve.Evaluate(distanceRatio));
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalCameraPos, Time.deltaTime * trackingSpeed);
    }
}
