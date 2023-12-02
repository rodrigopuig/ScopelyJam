using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MANUELITO
{
    public class PiledItem : MonoBehaviour
    {
        public float weight = 1;
        float weightRotFactor = 0.05f;
        float weightMovFactor = 0.005f;

        const float maxRot = 15;
        private void Awake()
        {
            
        }

        public void ApplyPhysics(float direction, float factor)
        {
            Vector3 finalRot = transform.localEulerAngles + direction * Vector3.forward * factor * weightRotFactor;
            finalRot.z = finalRot.z % 360;
            //finalRot.z = Mathf.Clamp(finalRot.z, -maxRot, maxRot);
            transform.localEulerAngles = finalRot;
            transform.localPosition = transform.localPosition - direction * Vector3.right * factor * weightMovFactor;
            GetComponent<Image>().color = finalRot.z > 200 || finalRot.z < 160 ? Color.red : Color.white;
        }
    }
}
