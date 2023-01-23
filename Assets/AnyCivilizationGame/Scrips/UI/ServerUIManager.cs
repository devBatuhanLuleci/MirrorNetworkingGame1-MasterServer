using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerUIManager : MonoBehaviour
{
    [Header("Informations")]
    public string version = "0.01";


    [Space]
    [Header("Assigments")]
    public TextMeshProUGUI version_Text;


    private void Awake()
    {
        version_Text.text = version;    
    }
}
