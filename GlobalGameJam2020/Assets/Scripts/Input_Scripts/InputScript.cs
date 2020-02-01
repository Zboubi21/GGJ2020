using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    public Image m_childImage;

    public string input;
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
        }
    }


    #region Private Methods

    void CheckValidation(string pressedInput)
    {
        m_childImage.gameObject.SetActive(true);

        if (CheckPressedInput(pressedInput))
        {
            m_childImage.color = Color.green;
        }
        else
        {
            m_childImage.color = Color.red;
            StartCoroutine(EchecAnimation());
        }
    }

    IEnumerator EchecAnimation()
    {
        ResetBoolArray();
        IsCheckable = false;
        yield return new WaitForSeconds(0.5f);
        InputFailure();
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
        m_convoyer.On_PotIsBreak();
    }
    #endregion
}
