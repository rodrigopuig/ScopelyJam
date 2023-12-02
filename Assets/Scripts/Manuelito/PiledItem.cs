using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MANUELITO
{
    public class PiledItem : MonoBehaviour
    {
        public float weight = 1;
        float weightRotFactor = 0.5f;

        [SerializeField] float maxRot = 20;

        public void ApplyPhysics(float direction, float factor)
        {
            Vector3 finalRot = transform.localEulerAngles + direction * Vector3.forward * factor * weightRotFactor;
            finalRot.z = Mathf.Clamp(finalRot.z, 180 -maxRot, 180 + maxRot);
            transform.localEulerAngles = finalRot;
        }
    }
}
