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

    ProceduralInputGenerationController controller;
    bool _isCheckable;

    public ProceduralInputGenerationController Controller { get => controller; set => controller = value; }
    public bool IsCheckable { get => _isCheckable; set => _isCheckable = value; }

    private void Start()
    {

        m_childImage.gameObject.SetActive(false);

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

    private void Update()
    {
        if (IsCheckable)
        {
            if (Input.GetButtonDown("a"))
            {
                _currentInput = "a";
                CheckValidation(_currentInput);
            }
            else if (Input.GetButtonDown("b"))
            {
                _currentInput = "b";
                CheckValidation(_currentInput);
            }
            else if (Input.GetButtonDown("x"))
            {
                _currentInput = "x";
                CheckValidation(_currentInput);
            }
            else if (Input.GetButtonDown("y"))
            {
                _currentInput = "y";
                CheckValidation(_currentInput);
            }
            else if (Input.GetButtonDown("up"))
            {
                _currentInput = "up";
                CheckValidation(_currentInput);
            }
            else if (Input.GetButtonDown("right"))
            {
                _currentInput = "right";
                CheckValidation(_currentInput);
            }
            else if (Input.GetButtonDown("down"))
            {
                _currentInput = "down";
                CheckValidation(_currentInput);
            }
            else if (Input.GetButtonDown("left"))
            {
                _currentInput = "left";
                CheckValidation(_currentInput);
            }
        }
    }

    void CheckValidation(string pressedInput)
    {
        m_childImage.gameObject.SetActive(true);
        IsCheckable = false;

        if (input == pressedInput)
        {
            m_childImage.color = Color.green;
            controller.ActivateCheckableOnNextInput();
        }
        else
        {
            m_childImage.color = Color.red;
            StartCoroutine(EchecAnimation());
        }
    }

    IEnumerator EchecAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        controller.OnSpriteGeneration(4, false);
        controller.OnResetTimer();
    }
}
