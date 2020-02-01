using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorRotator : MonoBehaviour
{
    
    [SerializeField] float m_rotationSpeedMultiplier = 1;

    ConveyorController m_conveyor;
    public ConveyorController Conveyor { get => m_conveyor; set => m_conveyor = value; }

    // Vector3 m_offset;
    // [SerializeField] float m_delta = 0.1f ;
    // [SerializeField] float m_changeDeltaSpeed = 1;

    // float delta;

    // void Start()
    // {
    //     m_offset = transform.position;
    // }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * (m_conveyor.ActualSpeed * m_rotationSpeedMultiplier));

        // delta = Mathf.PingPong(m_delta, Time.deltaTime * m_changeDeltaSpeed);

        // transform.position = new Vector3(m_offset.x + delta, transform.position.y, transform.position.z);
    }
    
}
