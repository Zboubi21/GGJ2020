using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] CameraAnimator m_inGameCamera;
    [SerializeField] CameraAnimator m_endGameCamera;

    [System.Serializable] class CameraAnimator
    {
        [Header("Movements")]
        public Vector3 m_toPos;
        public float m_timeToMove = 5;
        public AnimationCurve m_moveCurve;

        [Header("Rotations")]
        public Vector3 m_toRot;
        public float m_timeToRotate = 5;
        public AnimationCurve m_rotateCurve;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            SwitchCameraToEndGame();
        }
    }

    IEnumerator MovePosition(Vector3 toPos, float timToMove, AnimationCurve curve)
    {
        Vector3 fromPos = transform.position;
        float fracJourney = 0;
        float distance = Vector3.Distance(fromPos, toPos);
        float speed = distance / timToMove;
        Vector3 actualValue = fromPos;

        while (actualValue != toPos)
        {
            fracJourney += (Time.deltaTime) * speed / distance;
            actualValue = Vector3.Lerp(fromPos, toPos, curve.Evaluate(fracJourney));
            transform.position = actualValue;
            yield return null;
        }
    }
    IEnumerator ChangeRotation(Vector3 roRot, float timeToRot, AnimationCurve curve)
    {
        Vector3 fromPos = transform.eulerAngles;
        float fracJourney = 0;
        float distance = Vector3.Distance(fromPos, roRot);
        float speed = distance / timeToRot;
        Vector3 actualValue = fromPos;

        while (actualValue != roRot)
        {
            fracJourney += (Time.deltaTime) * speed / distance;
            actualValue = Vector3.Lerp(fromPos, roRot, curve.Evaluate(fracJourney));
            transform.eulerAngles = actualValue;
            yield return null;
        }
    }

    public void SwitchCameraToGameMode()
    {
        StopAllCoroutines();
        StartCoroutine(MovePosition(m_inGameCamera.m_toPos, m_inGameCamera.m_timeToMove, m_inGameCamera.m_moveCurve));
        StartCoroutine(ChangeRotation(m_inGameCamera.m_toRot, m_inGameCamera.m_timeToRotate, m_inGameCamera.m_rotateCurve));
    }
    public void SwitchCameraToEndGame()
    {
        StopAllCoroutines();
        StartCoroutine(MovePosition(m_endGameCamera.m_toPos, m_endGameCamera.m_timeToMove, m_endGameCamera.m_moveCurve));
        StartCoroutine(ChangeRotation(m_endGameCamera.m_toRot, m_endGameCamera.m_timeToRotate, m_endGameCamera.m_rotateCurve));
    }

}
