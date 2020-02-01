using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    [Header("Meshes")]
    [SerializeField] GameObject m_neutralMesh;
    [SerializeField] GameObject m_repairMesh;
    [SerializeField] GameObject m_breakMesh;
    [Space]
    [SerializeField] float m_waitTimeToFall = 0.1f;
    [SerializeField] float m_fallSpeed = 3;
    [SerializeField] float m_waitTimeToDestroyPot = 0.25f;

    enum MeshType
    {
        Neutral,
        Repair,
        Break,
    }

    ConveyorController m_conveyor;
    Vector3 m_fromPos;
    Vector3 m_toPos;
    float m_timeToMove;

    // End pot anim
    float m_moveSpeed;
    Vector3 m_endAnimZPos;
    Vector3 m_endAnimYPos;

    void Start()
    {
        SetVisibleMesh(MeshType.Neutral);
    }

    void GetConveyorValues()
    {
        m_fromPos = m_conveyor.m_startPos.position;
        m_toPos = m_conveyor.m_endPos.position;
        m_timeToMove = m_conveyor.m_timeToMovePot;
        m_moveSpeed = m_conveyor.Speed;

        m_endAnimZPos = m_conveyor.m_endAnimZPos.position;
        m_endAnimYPos = m_conveyor.m_endAnimYPos.position;
    }

    IEnumerator MovePotOnConveyor()
    {
        float fracJourney = 0;
        float distance = Vector3.Distance(m_fromPos, m_toPos);
        float speed = new float();
        Vector3 actualValue = m_fromPos;

        speed = distance / m_timeToMove;

        while (actualValue != m_toPos)
        {
            fracJourney += (Time.deltaTime) * speed / distance;
            actualValue = Vector3.Lerp(m_fromPos, m_toPos, fracJourney);
            transform.position = actualValue;
            yield return null;
        }
        m_conveyor.On_PotArrivedAtTheEndOfConveyor(this);
        StartCoroutine(MovePotOnLastAnim());
    }
    IEnumerator MovePotOnLastAnim()
    {
        Vector3 fromPos = transform.position;
        float fracJourney = 0;
        float distance = Vector3.Distance(fromPos, m_endAnimZPos);
        float speed = m_moveSpeed;
        Vector3 actualValue = fromPos;

        while (actualValue != m_endAnimZPos)
        {
            fracJourney += (Time.deltaTime) * speed / distance;
            actualValue = Vector3.Lerp(fromPos, m_endAnimZPos, fracJourney);
            transform.position = actualValue;
            yield return null;
        }
        StartCoroutine(FallOnBox());
    }
    IEnumerator FallOnBox()
    {
        yield return new WaitForSeconds(m_waitTimeToFall);
        Vector3 fromPos = transform.position;
        float fracJourney = 0;
        float distance = Vector3.Distance(fromPos, m_endAnimYPos);
        float speed = m_fallSpeed;
        Vector3 actualValue = fromPos;

        while (actualValue != m_endAnimYPos)
        {
            fracJourney += (Time.deltaTime) * speed / distance;
            actualValue = Vector3.Lerp(fromPos, m_endAnimYPos, fracJourney);
            transform.position = actualValue;
            yield return null;
        }
        yield return new WaitForSeconds(m_waitTimeToDestroyPot);
        DestroyPot();
    }

    void SetVisibleMesh(MeshType mesh)
    {
        switch (mesh)
        {
            case MeshType.Neutral:
                m_neutralMesh.gameObject.SetActive(true);
                m_repairMesh.gameObject.SetActive(false);
                m_breakMesh.gameObject.SetActive(false);
            break;
            case MeshType.Repair:
                m_neutralMesh.gameObject.SetActive(false);
                m_repairMesh.gameObject.SetActive(true);
                m_breakMesh.gameObject.SetActive(false);
            break;
            case MeshType.Break:
                m_neutralMesh.gameObject.SetActive(false);
                m_repairMesh.gameObject.SetActive(false);
                m_breakMesh.gameObject.SetActive(true);
            break;
        }
    }

    public void StartToMovePot(ConveyorController conveyor)
    {
        m_conveyor = conveyor;
        GetConveyorValues();
        StartCoroutine(MovePotOnConveyor());
    }

    public void On_PotIsRepair()
    {
        SetVisibleMesh(MeshType.Repair);
    }
    public void On_PotIsBreak()
    {
        SetVisibleMesh(MeshType.Break);
    }

    public void DestroyPot()
    {
        Destroy(gameObject);
    }
    
}
