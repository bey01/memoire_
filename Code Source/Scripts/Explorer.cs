using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class Explorer : MonoBehaviour
{
    public Text eText;



    private bool readText = false;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReadText(string path)
    {
        eText.text = File.ReadAllText(path);
    }
}