using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

namespace MANUELITO
{
    public class Controller : MonoBehaviour
    {
        public List<PiledItem> piledItems = new List<PiledItem>();
        [SerializeField] float weightBalance = 0; //-1 - 1
        [SerializeField] float totalWeight;
        // [SerializeField] Image weightSlider;
        // [SerializeField] TextMeshProUGUI sliderText;
        

        #region COMPONENTS
        Transform piledObjectsParent;
        #endregion

        void Start()
        {
            piledObjectsParent = gameObject.transform.parent;
            // foreach (Transform child in piledObjectsParent)
            // {
            //     PiledItem piledItem = child.GetComponent<PiledItem>();
            //     if (piledItem != null)
            //     {
            //         piledItems.Add(piledItem);
            //         totalWeight += piledItem.weight;
            //     }
            // }
        }


        private void Update()
        {
            UpdateWeightBalance();
            UpdatePiledItems();
        }


        private void UpdateWeightBalance()
        {
            weightBalance = (piledObjectsParent.eulerAngles.z)/ 90;
            // float ratio = Mathf.InverseLerp(-1, 1, weightBalance);
            // weightSlider.fillAmount = ratio;
            // weightSlider.color = Color.Lerp(Color.yellow, Color.green, ratio);
            // sliderText.text = weightBalance.ToString();
        }

        private void UpdatePiledItems()
        {
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