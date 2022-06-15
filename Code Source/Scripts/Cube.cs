using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public bool Selected = false;
    public Material Pink ;
    public Material OldMaterial;
    public InstantiateButton BtnManager;
    
    // Start is called before the first frame update
    void Start()
    {
        Pink = (Material)Resources.Load("Pink");
        BtnManager = GameObject.Find("BtnManager").GetComponent<InstantiateButton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Select()
    {
        if(!Selected && BtnManager.LeftToSelect>0)
        {
            BtnManager.LeftToSelect--;
            BtnManager.updateConfirmBtn();
            OldMaterial = this.gameObject.GetComponent<Renderer>().material;
            this.gameObject.GetComponent<Renderer>().material = Pink;
            Selected = true;
        }
        else if (Selected)
        {
            BtnManager.LeftToSelect++;
            BtnManager.updateConfirmBtn();
            this.gameObject.GetComponent<Renderer>().material = OldMaterial;
            Selected = false;
        }
    }
}
