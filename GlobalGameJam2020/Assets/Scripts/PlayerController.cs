using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] ConveyorController m_playerConvoyer;
    [SerializeField] float m_moveSpeed = 5;

    [Header("Test")]
    [Range(1, 2)] public int m_playerId;

    bool m_followPot = false;
    Vector3 m_startPlayerPos;
    float m_targetZPos;

    void Start()
    {
        m_playerConvoyer.SetPlayerToConveyor(this);
        m_startPlayerPos = m_playerConvoyer.m_startPlayerPos.position;
    }

    void FixedUpdate()
    {
        FollowPot();
    }

    void FollowPot()
    {
        if(m_followPot && m_playerConvoyer.GetRepairPot() != null)
        {
            m_targetZPos = m_playerConvoyer.GetRepairPot().transform.position.z;
            transform.position = new Vector3(transform.position.x, transform.position.y, m_targetZPos);
        }
    }

    IEnumerator MovePlayer(Vector3 toPos)
    {
        Vector3 fromPos = transform.position;
        float fracJourney = 0;
        float distance = Vector3.Distance(fromPos, toPos);
        Vector3 actualValue = fromPos;

        while (actualValue != toPos)
        {
            fracJourney += (Time.deltaTime) * m_moveSpeed / distance;
            actualValue = Vector3.Lerp(fromPos, toPos, fracJourney);

            transform.position = actualValue;

            yield return null;
        }
        On_StartToFollowPot(false);
        m_playerConvoyer.On_SpawnPot();
    }

    public void On_StartToFollowPot(bool followPot)
    {
        m_followPot = followPot;
    }
    public void MoveToStartPos()
    {
        StartCoroutine(MovePlayer(m_startPlayerPos));
    }
    
}
