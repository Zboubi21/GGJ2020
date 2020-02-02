using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorRotator : MonoBehaviour
{
    
    [SerializeField] float m_rotationSpeedMultiplier = 1;

    ConveyorController m_conveyor;
    public ConveyorController Conveyor { get => m_conveyor; set => m_conveyor = value; }

    bool m_conveyorIsStopped = false;

    void FixedUpdate()
    {
        if(!m_conveyorIsStopped)
            transform.Rotate(Vector3.forward * (m_conveyor.ActualSpeed * m_rotationSpeedMultiplier));
    }

    public void On_ConveyorIsStopped()
    {
        m_conveyorIsStopped = true;
    }
    
}
