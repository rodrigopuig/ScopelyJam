using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

namespace MANUELITO
{
    public class PiledItemsController : MonoBehaviour
    {
        public List<PiledItem> piledItems = new List<PiledItem>();
        [SerializeField] float weightBalance = 0; //-1 - 1
        [SerializeField] float totalWeight;
        // [SerializeField] Image weightSlider;
        // [SerializeField] TextMeshProUGUI sliderText;
        

        #region COMPONENTS
        Transform piledObjectsParent;
        #endregion

        private void Update()
        {
            UpdatePiledItems();
        }

        private void UpdatePiledItems()
        {
            weightBalance = ((transform.localEulerAngles.z - 180)) / 90f;
            int iter = 0;
            float ratio;
            foreach (PiledItem item in piledItems)
            {
                ratio = Mathf.Lerp(1, 0, (float)iter / (float)piledItems.Count);
                item.ApplyPhysics(weightBalance, ratio);
                iter++;
            }
        }


    }
}