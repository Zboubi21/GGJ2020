using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] CameraAnimator m_inGameCamera;

    [System.Serializable] class CameraAnimator
    {
        [Header("Movements")]
        public Vector3 m_toPos;
        public float m_moveSpeed = 5;
        public AnimationCurve m_moveCurve;

        [Header("Rotations")]
        public Vector3 m_toRot;
        public float m_rotateSpeed = 5;
        public AnimationCurve m_rotateCurve;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            SwitchCameraToGameMode();
        }
    }

    IEnumerator MovePosition(Vector3 toPos, float speed, AnimationCurve curve)
    {
        Vector3 fromPos = transform.position;
        float fracJourney = 0;
        float distance = Vector3.Distance(fromPos, toPos);
        Vector3 actualValue = fromPos;

        while (actualValue != toPos)
        {
            fracJourney += (Time.deltaTime) * speed / distance;
            actualValue = Vector3.Lerp(fromPos, toPos, curve.Evaluate(fracJourney));
            transform.position = actualValue;
            yield return null;
        }
    }

    public void SwitchCameraToGameMode()
    {
        StopAllCoroutines();
        StartCoroutine(MovePosition(m_inGameCamera.m_toPos, m_inGameCamera.m_moveSpeed, m_inGameCamera.m_moveCurve));
    }

}
