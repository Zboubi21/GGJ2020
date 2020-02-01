using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AleaRotation : MonoBehaviour
{
    
    [SerializeField] float m_minYRot = 0; 
    [SerializeField] float m_maxYRot = 360; 
    
    void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(m_minYRot, m_maxYRot), transform.eulerAngles.z);
    }

}
