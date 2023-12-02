using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rodrigo
{

    public class Character : MonoBehaviour
    {
        [Header("Info. Dont tpuch")]
        public float value; // -1 a 1

        [Header("Control metods")]
        public KeyCode left;
        public KeyCode right;

        public float turnSpeed = 10;
        public float baseSpeed = 1;

        public Transform trBasket;
        public Transform trElements;

        public float maxAngle = 30;

        [Header("control methods. Auxiliar")]
        public AnimationCurve turnRecoveryCurve;
        public AnimationCurve movementRecoveryCurve; //inversamente proporcional

        public float minTimeToChangeExternalForce = 0.5f;
        public float maxTimeToChangeExternalForce = 3;

        public Basket basket;

        float minExternalForce = 5;
        float maxExternalForce = 10;
       

        float currentAngle = 0;
       
        float externalTurnForce = 30;

        float balancingForce_extra;

        float externalForceSign;
        float counterToChangeExternalForce;
        float timeToChangeExternalForce;

        bool finished;

        private void Awake()
        {
            ChangeExternalForce();
            basket.Init();
        }

        public void Update()
        {
            if (finished)
                return;

            float percentge = Mathf.Clamp01(Mathf.Abs(currentAngle) / maxAngle);
            value = percentge;
            float _externalForce = 0;
            float balancingSign = 0;

            if (percentge < 0.2f)
                _externalForce = externalForceSign * externalTurnForce;
            else
            {
                balancingSign = Mathf.Sign(Mathf.Sin(currentAngle * Mathf.Deg2Rad));

                balancingForce_extra = balancingSign * Mathf.Lerp(0.7f, 1, (percentge - 0.5f) / 0.5f) * 25f;
            }

            float _sign = 0;
            if(Input.GetKey(left))
            {
                _sign = -1;
            }

            if(Input.GetKey(right))
            {
                _sign = 1;
            }

            if (Mathf.Abs(currentAngle) > maxAngle)
            {
                finished = true;
                basket.Activate();
            }

            UpdateExternalForce();

            currentAngle += (_sign * turnRecoveryCurve.Evaluate(percentge) * turnSpeed + _externalForce + balancingForce_extra) * Time.deltaTime * 1.5f;
            transform.position += Vector3.right * _sign * /*movementRecoveryCurve.Evaluate(percentge) */ baseSpeed * Time.deltaTime * 1.5f;
            trBasket.localRotation = Quaternion.Euler(0, 0, 180 + currentAngle);
        }

        void UpdateExternalForce()
        {
            counterToChangeExternalForce += Time.deltaTime;
            if(counterToChangeExternalForce > timeToChangeExternalForce)
            {
                counterToChangeExternalForce = 0;
                ChangeExternalForce();
            }
        }

         void ChangeExternalForce()
        {
            externalTurnForce = Random.Range(minExternalForce, maxExternalForce);
            timeToChangeExternalForce = Random.Range(minTimeToChangeExternalForce, maxTimeToChangeExternalForce);
            externalForceSign = Random.Range(0, 2) == 1 ? 1 : -1;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(trElements.position, trElements.position + new Vector3(-Mathf.Cos(maxAngle * Mathf.Deg2Rad), -Mathf.Sin(maxAngle * Mathf.Deg2Rad), 0));
            Gizmos.DrawLine(trElements.position, trElements.position + new Vector3(Mathf.Cos(maxAngle * Mathf.Deg2Rad), -Mathf.Sin(maxAngle * Mathf.Deg2Rad), 0));
        }

        [System.Serializable]
        public class Basket
        {
            public Collider2D[] elements;
            public Collider2D[] colliders;

            Rigidbody2D[] rbs;

            public void Init()
            {
                rbs = new Rigidbody2D[elements.Length];
                for (int i = 0; i < rbs.Length; i++)
                    rbs[i] = elements[i].GetComponent<Rigidbody2D>();

            }

            public void Activate()
            {
                foreach (var _triggers in elements)
                    _triggers.isTrigger = false;

                foreach (var _collider in colliders)
                    _collider.enabled = false;

                foreach (var rb in rbs)
                {
                    Vector2 _force = new Vector3(Random.Range(0f, 1f), Random.Range(0f, -1f), 0).normalized;
                    rb.AddForce(_force * Random.Range(3, 8f), ForceMode2D.Impulse);
                }

            }
        }
    }
}