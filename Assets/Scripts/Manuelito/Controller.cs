using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MANUELITO
{
    public class Controller : MonoBehaviour
    {
        List<PiledItem> piledItems = new List<PiledItem>();
        [SerializeField] float weightBalance = 0; //-1 - 1
        [SerializeField] float totalWeight;
        [SerializeField] Image weightSlider;
        [SerializeField] TextMeshProUGUI sliderText;
        float accel;
        float hInput;

        #region COMPONENTS
        Rigidbody2D rb;
        [SerializeField] Transform piledObjectsParent;
        [SerializeField] PiledItem piledObjectPrefab;
        #endregion

        [SerializeField] float movSpeed = 20;
        const float defaultObjScale = 40;
        const float maxSpeed = 400;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            CreatePiledObjects(4);
        }

        private void Update()
        {
            ProcessInputs();
            UpdateMovement();
            UpdateWeightBalance();
            UpdatePiledItems();
        }

        private void ProcessInputs()
        {
            hInput = Input.GetAxis("Horizontal");
            accel = Mathf.Lerp(accel, hInput != 0 ? 1 : 0, Time.deltaTime);
        }

        private void UpdateWeightBalance()
        {
            weightBalance = Mathf.Clamp(weightBalance + hInput * 0.3f , - 1, 1);
            float ratio = Mathf.InverseLerp(-1, 1, weightBalance);
            weightSlider.fillAmount = ratio;
            weightSlider.color = Color.Lerp(Color.yellow, Color.green, ratio);
            sliderText.text = weightBalance.ToString();
        }

        private void UpdateMovement()
        {
            Vector3 currentVelocity = rb.velocity;
            rb.AddForce(Vector2.right * (weightBalance * movSpeed));
            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
            rb.velocity = currentVelocity;
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

        [ContextMenu("CreateTestItems")]
        private void CreateTestItems() 
        {
            CreatePiledObjects(5);
        }

        private void CreatePiledObjects(int quantity)
        {
            PiledItem newObj;
            Transform previousObj = piledObjectsParent;
            float scale;
            for (int i = 0; i < quantity; i++)
            {
                scale = Random.Range(0.75f, 1.25f);
                newObj = Instantiate(piledObjectPrefab, previousObj);
                newObj.transform.localScale = scale * Vector3.one;
                newObj.transform.localPosition = Vector3.zero;
                newObj.transform.position += Vector3.up * scale * defaultObjScale; ;
                previousObj = newObj.transform;
                piledItems.Add(newObj);
            }
        }
    }
}