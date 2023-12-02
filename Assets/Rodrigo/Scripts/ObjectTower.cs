using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTower : MonoBehaviour
{
    public float rotation;
    public Transform trBase;
    public Transform[] elements;

    public int elementsPerLine;
    public float distanceBetweenLines;

    public float width;

    private void Awake()
    {
        
    }

    private void OnValidate()
    {
        rotation = transform.localRotation.eulerAngles.z;
        UpdatePosition();
    }

    void UpdatePosition()
    {
        int _amountOfLines = elements.Length / elementsPerLine;
        for(int _line = 0; _line < _amountOfLines; _line++)
        {
            for (int i = 0; i < elementsPerLine && (i + _line * _amountOfLines)<elements.Length; i++)
            {
                float _percentage = (float)i / (float)(elementsPerLine - 1);
                float _auxWidth = (width - ((float)_line / (float)_amountOfLines - 1) * 0.5f * width) * 0.5f;
                Vector3 _pos = trBase.position - trBase.right * _auxWidth + trBase.right * _percentage * 2 * _auxWidth + Vector3.up * _line * distanceBetweenLines;
                elements[(i + _line * elementsPerLine)].position = _pos;
            }
        }
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(trBase.position, trBase.position + new Vector3(-Mathf.Cos(rotation * Mathf.Deg2Rad), -Mathf.Sin(rotation * Mathf.Deg2Rad), 0).normalized * width);
        Gizmos.DrawLine(trBase.position, trBase.position - new Vector3(-Mathf.Cos(rotation * Mathf.Deg2Rad), -Mathf.Sin(rotation * Mathf.Deg2Rad), 0).normalized * width);
    }
}
