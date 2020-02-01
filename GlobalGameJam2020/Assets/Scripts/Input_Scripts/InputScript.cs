using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    public Image m_childImage;
    public Image m_whiteBackground;
    [Space]
    public Sprite[] m_feedBack;

    public string input;

    [Space]
    [Header("Animation Input FeedBack")]
    public AnimationCurve animationSelection;
    public float timeOfAnim = 1f;
    public float maxScale;
    float minScale;
    [Space]
    [Header("FadeWhite Input FeedBack")]
    public AnimationCurve animationFadeWhite;
    public float timeOfFadeWhiteAnim = 1f;
    [Space]
    [Header("Animation Wrong Input FeedBack")]
    public AnimationCurve animationWrongInput;
    public float timeOfWrongAnim = 1f;
    public float maxEulerRotation = 10f;
    float minEulerRotation;
    [Space]
    [Header("Animation Succes Or Failure FeedBack")]
    public AnimationCurve animationSuccesOrFailure;
    public float timeOfSuccesOrFailureAnim = 1f;
    public float childMaxScale = 1f;
    float childMinScale;
    [Space]
    public AudioSource source;
    public AudioClip[] inputSound;
    string _currentInput;
    List<string> allWrongInputs;
    int _playerID;

    ProceduralInputGenerationController controller;
    bool _isCheckable;

    public ProceduralInputGenerationController Controller { get => controller; set => controller = value; }
    public bool IsCheckable { get => _isCheckable; set => _isCheckable = value; }
    public int PlayerID { get => _playerID; set => _playerID = value; }

    ConveyorController m_convoyer;
    public ConveyorController Convoyer { get => m_convoyer; set => m_convoyer = value; }

    private void Start()
    {

        m_childImage.gameObject.SetActive(false);
        ResetBoolArray();
        minScale = transform.localScale.x;
        minEulerRotation = transform.localEulerAngles.z;
        childMinScale = m_childImage.transform.localScale.z;
        //allWrongInputs = new List<string>{ "a", "b", "x", "y", "up", "right", "down", "left" };

        //for (int i = 0, l = allWrongInputs.Count; i < l; ++i)
        //{
        //    if(allWrongInputs[i] == input)
        //    {
        //        allWrongInputs.Remove(allWrongInputs[i]);
        //        break;
        //    }
        //}
    }

    bool[] isPressed = new bool[8];

    //bool test(string input)
    //{
    //    return Input.GetButtonDown(input);
    //}
    bool BoolArray(int i0, int i1, int i2, int i3, int i4, int i5, int i6)
    {
        if(isPressed[i0] && isPressed[i1] && isPressed[i2] && isPressed[i3] && isPressed[i4] && isPressed[i5] && isPressed[i6])
        {
            return true;
        }
        return false;
    }


    private void Update()
    {
        if (IsCheckable)
        {
            if ((Input.GetButtonDown("A_P" + PlayerID) && BoolArray(1, 2, 3, 4, 5, 6, 7)) || !isPressed[0] )
            {
                _currentInput = "a";
                isPressed[0] = false;
                CheckValidation(_currentInput);

                if(Input.GetButtonUp("A_P" + PlayerID) && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[0] = true;
                }
            }
            else if ((Input.GetButtonDown("B_P" + PlayerID) && BoolArray(0, 2, 3, 4, 5, 6, 7)) || !isPressed[1])
            {
                _currentInput = "b";
                isPressed[1] = false;
                CheckValidation(_currentInput);

                if (Input.GetButtonUp("B_P" + PlayerID) && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[1] = true;
                }
            }
            else if ((Input.GetButtonDown("X_P" + PlayerID) && BoolArray(0, 1, 3, 4, 5, 6, 7)) || !isPressed[2])
            {
                _currentInput = "x";
                isPressed[2] = false;
                CheckValidation(_currentInput);

                if (Input.GetButtonUp("X_P" + PlayerID) && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[2] = true;
                }
            }
            else if ((Input.GetButtonDown("Y_P" + PlayerID) && BoolArray(0, 1, 2, 4, 5, 6, 7)) || !isPressed[3] )
            {
                _currentInput = "y";
                isPressed[3] = false;
                CheckValidation(_currentInput);

                if (Input.GetButtonUp("Y_P" + PlayerID) && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[3] = true;
                }
            }
            else if((Input.GetAxisRaw("DPAD_v_P" + PlayerID) > 0f && Input.GetAxisRaw("DPAD_h_P" + PlayerID) == 0f && BoolArray(0, 1, 2, 3, 5, 6, 7)) || !isPressed[4] )
            {
                _currentInput = "up";
                isPressed[4] = false;
                CheckValidation(_currentInput);

                if (Input.GetAxisRaw("DPAD_v_P" + PlayerID) == 0f && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[4] = true;
                }
            }
            else if ((Input.GetAxisRaw("DPAD_v_P" + PlayerID) < 0f && Input.GetAxisRaw("DPAD_h_P" + PlayerID) == 0f && BoolArray(0, 1, 2, 3, 4, 6, 7)) || !isPressed[5])
            {
                _currentInput = "down";
                isPressed[5] = false;
                CheckValidation(_currentInput);

                if (Input.GetAxisRaw("DPAD_v_P" + PlayerID) == 0f && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[5] = true;
                }
            }
            else if ((Input.GetAxisRaw("DPAD_h_P" + PlayerID) > 0f && Input.GetAxisRaw("DPAD_v_P" + PlayerID) == 0f && BoolArray(0, 1, 2, 3, 4, 5, 7)) || !isPressed[6])
            {
                _currentInput = "right";
                isPressed[6] = false;
                CheckValidation(_currentInput);

                if (Input.GetAxisRaw("DPAD_h_P" + PlayerID) == 0f && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[6] = true;
                }
            }
            else if ((Input.GetAxisRaw("DPAD_h_P" + PlayerID) < 0f && Input.GetAxisRaw("DPAD_v_P" + PlayerID) == 0f && BoolArray(0, 1, 2, 3, 4, 5, 6)) || !isPressed[7])
            {
                _currentInput = "left";
                isPressed[7] = false;
                CheckValidation(_currentInput);

                if (Input.GetAxisRaw("DPAD_h_P" + PlayerID) == 0f && CheckPressedInput(_currentInput))
                {
                    InputSucces();
                    isPressed[7] = true;
                }
            }
            if (!go)
            {
                Color m_color = m_whiteBackground.color;
                m_color.a = 0;
                m_whiteBackground.color = m_color;
                go = true;
            }
        }
    }

    bool go;
    #region Private Methods

    void CheckValidation(string pressedInput)
    {
        m_childImage.gameObject.SetActive(true);

        StartCoroutine(InputFeedBack(animationSelection, timeOfAnim));
        StartCoroutine(InputFadeWhiteFeedBack(animationFadeWhite, timeOfFadeWhiteAnim));
        StartCoroutine(FeedBackFailOrSucces(animationSuccesOrFailure, timeOfSuccesOrFailureAnim, m_childImage.gameObject.transform));

        if(source != null && inputSound[0] != null)
        {
            int random = Random.Range(0, inputSound.Length);
            source.clip = inputSound[random];
            source.Play();
        }

        if (CheckPressedInput(pressedInput))
        {
            m_childImage.sprite = m_feedBack[0];
        }
        else
        {
            m_childImage.sprite = m_feedBack[1];
            StartCoroutine(InputWrongFeedBack(animationWrongInput, timeOfWrongAnim));
            StartCoroutine(EchecAnimation());
        }
    }

    IEnumerator EchecAnimation()
    {
        ResetBoolArray();
        IsCheckable = false;
        yield return new WaitForSeconds(GameManager.s_instance.punishTimeForFaillingPatern);
        InputFailure();
    }

    IEnumerator InputFeedBack(AnimationCurve curve, float timeOfAnimation)
    {
        float _currentTimeOfAnimation = 0;
        while (_currentTimeOfAnimation / timeOfAnimation <= 1)
        {
            yield return new WaitForSeconds(0.01f);
            _currentTimeOfAnimation += Time.deltaTime;

            float height = curve.Evaluate(_currentTimeOfAnimation / timeOfAnimation);
            transform.localScale = new Vector3(Mathf.Lerp(minScale, maxScale, height), Mathf.Lerp(minScale, maxScale, height), Mathf.Lerp(minScale, maxScale, height));
        }
        _currentTimeOfAnimation = 0;
    }

    IEnumerator InputFadeWhiteFeedBack(AnimationCurve curve, float timeOfAnimation)
    {
        float _currentTimeOfAnimation = 0;
        while (_currentTimeOfAnimation / timeOfAnimation <= 1)
        {
            yield return new WaitForSeconds(0.01f);
            _currentTimeOfAnimation += Time.deltaTime;

            float height = curve.Evaluate(_currentTimeOfAnimation / timeOfAnimation);

            Color m_color = m_whiteBackground.color;
            m_color.a = Mathf.Lerp(0, 1, height);
            m_whiteBackground.color = m_color;
        }
        _currentTimeOfAnimation = 0;
    }

    IEnumerator InputWrongFeedBack(AnimationCurve curve, float timeOfAnimation)
    {
        float _currentTimeOfAnimation = 0;
        while (_currentTimeOfAnimation / timeOfAnimation <= 1)
        {
            yield return new WaitForSeconds(0.01f);
            _currentTimeOfAnimation += Time.deltaTime;

            float height = curve.Evaluate(_currentTimeOfAnimation / timeOfAnimation);
            Vector3 rot = transform.rotation.eulerAngles;
            rot.z = maxEulerRotation * height;
            //if (height > 0)
            //{
            //    rot.z = Mathf.Lerp(minEulerRotation, maxEulerRotation, height);
            //}
            //else
            //{
            //    rot.z = Mathf.Lerp(minEulerRotation, maxEulerRotation, height);
            //}
            transform.localEulerAngles = rot;
        }
        _currentTimeOfAnimation = 0;
    }

    IEnumerator FeedBackFailOrSucces(AnimationCurve curve, float timeOfAnimation, Transform transform)
    {
        float _currentTimeOfAnimation = 0;
        while (_currentTimeOfAnimation / timeOfAnimation <= 1)
        {
            yield return new WaitForSeconds(0.01f);
            _currentTimeOfAnimation += Time.deltaTime;

            float height = curve.Evaluate(_currentTimeOfAnimation / timeOfAnimation);
            transform.localScale = new Vector3(Mathf.Lerp(childMinScale, childMaxScale, height), Mathf.Lerp(childMinScale, childMaxScale, height), Mathf.Lerp(childMinScale, childMaxScale, height));
        }
        _currentTimeOfAnimation = 0;
    }


    bool CheckPressedInput(string pressedInput)
    {
        if(input == pressedInput)
        {
            return true;
        }
        return false;
    }

    void ResetBoolArray()
    {
        for (int i = 0, l = isPressed.Length; i < l; ++i)
        {
            isPressed[i] = true;
        }
    }


    void InputSucces()
    {
        controller.ActivateCheckableOnNextInput();
        IsCheckable = false;
    }

    void InputFailure()
    {
        controller.OnSpriteGeneration();
        controller.OnResetTimer();
        controller.OnLoseCombo();
        m_convoyer.On_PotIsBreak();
    }
    #endregion
}
