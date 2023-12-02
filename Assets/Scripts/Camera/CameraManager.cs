using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform[] focusObjects;
    [SerializeField] float maxHDistance = 180;
    [SerializeField] float minHDistance = 130;
    [SerializeField] float trackingSpeed = 1;
    [SerializeField] float hMargin = 40;
    [SerializeField] AnimationCurve zPosCurve;

    [Header("Misc"), Space(20)]
    [SerializeField] RectTransform foreGroundLayer;
    [SerializeField] float foregroundMaxXPos = 160;
    [SerializeField] float foregroundMaxZPos = -250;
    [SerializeField] float foregroundMinZPos = -100;

    const float movSpeed = 20;
    const float maxFocusDistance = 0;
    const float minFocusDistance = 40;
    const float maxXPos = 33;

    private void Update()
    {
        TestMoveObjects();
        KeepObjectsInFocus();
    }

    void TestMoveObjects()
    {
        float hInput = Input.GetAxis("Horizontal");
        float hInputAlt = Input.GetAxis("Horizontal2");
        focusObjects[0].position += Vector3.right * hInput * movSpeed * Time.deltaTime;
        focusObjects[1].position += Vector3.right * hInputAlt * movSpeed * Time.deltaTime;
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
        Vector3 finalForegroundPos = foreGroundLayer.localPosition;

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
        distanceRatio = Mathf.InverseLerp(minHDistance, maxHDistance, currentHDistance); //1 = furthest apart, 0 = closest

        focusCenter = focusCenter / focusObjects.Length;
        focusCenter.x = Mathf.Clamp(focusCenter.x, -maxXPos, maxXPos);
        xPosRatio = Mathf.InverseLerp(-maxXPos, maxXPos, focusCenter.x);
        finalForegroundPos.z = Mathf.Lerp(foregroundMinZPos, foregroundMaxZPos, distanceRatio);
        finalForegroundPos.x = Mathf.Lerp(foregroundMaxXPos, -foregroundMaxXPos, xPosRatio);
        foreGroundLayer.localPosition = finalForegroundPos;

        finalCameraPos.x = Mathf.Lerp(focusCenter.x, 0, distanceRatio);
        finalCameraPos.z = Mathf.Lerp(minFocusDistance, maxFocusDistance, zPosCurve.Evaluate(distanceRatio));
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalCameraPos, Time.deltaTime * trackingSpeed);
    }
}
