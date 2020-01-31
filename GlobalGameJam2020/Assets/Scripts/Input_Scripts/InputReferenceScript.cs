using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputReferenceScript : MonoBehaviour
{
    public Image m_childImage;

    public string input;

    string[] allPossibleInputs;

    private void Start()
    {
        allPossibleInputs = new string[8] { "a", "b", "x", "y", "up", "right", "down", "left" };
    }
}
