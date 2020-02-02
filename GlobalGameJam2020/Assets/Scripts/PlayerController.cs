using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [Header("Player ID")]
    [Range(1, 2)] public int m_playerId;
    [Space]
    [SerializeField] ConveyorController m_playerConvoyer;
    [SerializeField] float m_moveSpeed = 5;
    [SerializeField] float m_rotateSpeed = 5;
    [SerializeField] Animator m_animator;

    bool m_followPot = false;
    Vector3 m_startPlayerPos;
    Vector3 m_endGamePlayerPos;
    Vector3 m_endGamePlayerRot;
    float m_targetZPos;

    void Start()
    {
        m_playerConvoyer.SetPlayerToConveyor(this);
        m_startPlayerPos = m_playerConvoyer.m_startPlayerPos.position;
        m_endGamePlayerPos = m_playerConvoyer.m_endGamePlayerPos.position;
        m_endGamePlayerRot = m_playerConvoyer.m_endGamePlayerPos.eulerAngles;
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

            m_animator.SetBool("Run", true);
            // m_animator.SetBool("IsRepair", true);
            MoveAccordingToPlayerId();
            // m_animator.SetLayerWeight(1, 1);
        }
        else
        {
            // m_animator.SetBool("IsRepair", false);
            // m_animator.SetLayerWeight(1, 0);
        }
    }

    IEnumerator MovePlayer(Vector3 toPos)
    {
        m_animator.SetBool("Run", true);
        // if(m_playerId == 1)
        // {
        //     m_animator.SetBool("GoToRight", false);
        //     m_animator.SetBool("GoToLeft", true);
        // }
        // else
        // {
        //     m_animator.SetBool("GoToRight", true);
        //     m_animator.SetBool("GoToLeft", false);
        // }
        MoveAccordingToPlayerId();
            
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

        m_animator.SetBool("Run", false);
    }

    IEnumerator RotatePlayerToEndGamePos()
    {
        Vector3 fromRot = transform.eulerAngles;
        Vector3 toRot = m_endGamePlayerRot;
        float fracJourney = 0;
        float distance = Vector3.Distance(fromRot, toRot);
        Vector3 actualValue = fromRot;

        while (actualValue != toRot)
        {
            fracJourney += (Time.deltaTime) * m_rotateSpeed / distance;
            actualValue = Vector3.Lerp(fromRot, toRot, fracJourney);
            transform.eulerAngles = actualValue;
            yield return null;
        }
        StartCoroutine(MovePlayerToEndGamePos());
    }
    IEnumerator MovePlayerToEndGamePos()
    {
        Vector3 fromPos = transform.position;
        Vector3 toPos = m_endGamePlayerPos;
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
        m_animator.SetTrigger("StartPose");
    }

    void MoveAccordingToPlayerId()
    {
        if(m_playerId == 1)
        {
            m_animator.SetBool("GoToRight", false);
            m_animator.SetBool("GoToLeft", true);
        }
        else
        {
            m_animator.SetBool("GoToRight", true);
            m_animator.SetBool("GoToLeft", false);
        }
    }

    public void On_StartToFollowPot(bool followPot)
    {
        m_followPot = followPot;
    }
    public void MoveToStartPos()
    {
        StartCoroutine(MovePlayer(m_startPlayerPos));
    }
    public void MoveToEndGamePos()
    {
        StartCoroutine(RotatePlayerToEndGamePos());
    }
    
}
