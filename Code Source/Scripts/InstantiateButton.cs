using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.IO;

public class InstantiateButton : MonoBehaviour
{
    public List<GameObject> Cubes;
    
    private int ListSize = 10;
    public string CSVFile;
    public int listSize 
    {
        get
        {
            return ListSize;
        }
        set
        {
            ListSize = value;
            UiUpdate();
        }
    }
    public bool Confirmed = false;
    public bool WantToFinish = false;
    public bool CoroutineRunning = false;
    public int MaxHeight = 20;
    public float speed = 0.5f;
    private GameObject temp;
    public IEnumerator e;
    public Material Red;
    public Material Green;
    public bool CanSelectCube = false;
    public int LeftToSelect = 0;
    public int maxScore;
    public int myScore;
    
    //UI Elements
    public Text ConfirmTxt;
    public Text CSV_data;
    public Text ListSizeText;
    public Text ArrayUi;
    public Text ScoreText;
    public GameObject BubbleAlgo;
    public GameObject SelectAlgo;
    public GameObject InsertAlgo;

    public TextMeshProUGUI InstructionText;
    
    public Canvas HeightHolder;
    public GameObject PauseMenu;
    public GameObject Option1;
    public GameObject Option2;
    public GameObject Option3;
    public GameObject ConfirmBtn;
    public GameObject AutoSortBtn;
    public int OptionSelected = 1;
    public GameObject ShowExplorerButton;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void resume()
    {
        PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void quit()
    {
        Application.Quit();
    }
    public void option1()
    {
        Option1.GetComponent<Renderer>().material = Green;
        Option2.GetComponent<Renderer>().material = Red;
        Option3.GetComponent<Renderer>().material = Red;
        OptionSelected = 1;
    }
    public void option2()
    {   
        Option1.GetComponent<Renderer>().material = Red;
        Option2.GetComponent<Renderer>().material = Green;
        Option3.GetComponent<Renderer>().material = Red;
        OptionSelected = 2;
    }
    public void option3()
    {   
        Option1.GetComponent<Renderer>().material = Red;
        Option2.GetComponent<Renderer>().material = Red;
        Option3.GetComponent<Renderer>().material = Green;
        OptionSelected = 3;
    }
    void UiUpdate(){
        ListSizeText.text = ListSize.ToString();
    }
    public void BubbleSort()
    {
        StartCoroutine(e);
    }
    void DeselectCubes(List<GameObject> c)
    {
        for (int i = 0; i < c.Count; i++)
        {
            if(c[i].GetComponent<Cube>().Selected)
            {
                c[i].GetComponent<Cube>().Select();
            }
        }
    }
    public void Confirm()
    {
        CanSelectCube = false;
        StartCoroutine(e);
        ConfirmBtn.SetActive(false);
        ConfirmTxt.text = "Confirm";
    }
    void StartSortBtn()
    {
        LeftToSelect = 2;
        ConfirmTxt.text = "Start";
        ConfirmBtn.SetActive(true);
    }
    void CheckAnswers(int j, List<GameObject> c)
    {   
        maxScore++;
        List<GameObject> answers = new List<GameObject>();
        for (int i = 0; i < c.Count; i++)
        {
            if(c[i].GetComponent<Cube>().Selected)
                answers.Add(c[i]);
        }
        if (c[j] == answers[0] || c[j] == answers[1])
        {
            if (c[j+1] == answers[0] || c[j+1] == answers[1])
            {
                InstructionText.text = "Nice Job !";
                myScore++;
            }
            else
            {
                InstructionText.text = "Better Luck Next Time";
            }
        }   
        else
        {
            InstructionText.text = "Better Luck Next Time";
        }


        ScoreText.text = "Score:"+myScore+"/"+maxScore;
        //Deselect after checking answers
        DeselectCubes(Cubes);
        LeftToSelect = 2 ;
    }
    void RemoveAlreadyInstatiatedCubes()
    {
        foreach (Transform child in GameObject.Find("CubeHolder").transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void Instatiate()
    {
        StartSortBtn();
        RemoveAlreadyInstatiatedCubes();
        ArrayUi.text = "Initial Array: ";
        Cubes = new List<GameObject>();
        for (int i = 0; i < ListSize; i++)
        {
            int randomHeight = Random.Range(1, MaxHeight + 1);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder); 
            cube.transform.parent = GameObject.Find("CubeHolder").transform;;
            cube.AddComponent<Cube>();
            cube.transform.localScale = new Vector3(1.5f, randomHeight, 1.5f);  
            cube.transform.position = new Vector3(2*i - ListSize, randomHeight / 2.0f + 0.5f, 11);
            //Ui that Holds the height value
            Canvas a = Instantiate(HeightHolder, new Vector3(0, 0, -0.6f), Quaternion.identity);
            a.transform.parent = cube.transform;
            a.transform.position = cube.transform.position + new Vector3(0,0,-0.9f);
            a.transform.GetChild(0).gameObject.GetComponent<Text>().text = randomHeight.ToString();
            ArrayUi.text += randomHeight.ToString() + ", ";
            Cubes.Add(cube);
        }
        if(OptionSelected == 1)
            e = BubbleSort(Cubes);
            
        if(OptionSelected == 2)
            e = SelectionSort(Cubes);
            
        if(OptionSelected == 3)
            e = InsertionSort(Cubes);
    }
    void WaitUserInput()
    {

        InstructionText.text = "What are the Next elements to Check ?";
        CanSelectCube = true;
        CoroutineRunning = false;
        StopCoroutine(e);
    }
    public void updateConfirmBtn()
    {
        if(CanSelectCube && LeftToSelect == 0)
        {
            ConfirmBtn.SetActive(true);
        }
        else
        {
            ConfirmBtn.SetActive(false);
        }
    }
    IEnumerator BubbleSort(List<GameObject> c)
    {
        CoroutineRunning = true;
        for(int i=0; i<c.Count - 1; i++)
        {
            for(int j=0; j<c.Count - i -1; j++)
            {
                if(j%2 == 0 && j != 0 && !WantToFinish)
                {
                    CoroutineRunning = true;
                    WaitUserInput();
                    yield return new WaitForSeconds(3);
                    if(!WantToFinish)
                    {
                        CheckAnswers(j, c);
                    }
                    CoroutineRunning = true;
                }
                yield return new WaitForSeconds(speed);
                Confirmed = false;
                LeanTween.color(c[j], Color.yellow, 0f);
                LeanTween.color(c[j+1], Color.blue, 0f);
                yield return new WaitForSeconds(0.5f);

                if(c[j].transform.localScale.y > c[j+1].transform.localScale.y)
                {
                    Vector3 tempPosition;
                    // Swap
                    temp = c[j];
                    c[j] = c[j + 1];
                    c[j + 1] = temp;
                    
                    tempPosition = c[j].transform.localPosition;

                    yield return new WaitForSeconds(speed);
                    LeanTween.moveX(c[j], c[j+1].transform.localPosition.x, speed);
                    LeanTween.moveLocalZ(c[j], 2f, speed).setLoopPingPong(1);
                    LeanTween.moveX(c[j+1], tempPosition.x, speed);
                    LeanTween.moveLocalZ(c[j + 1], 4f, speed).setLoopPingPong(1);
                    yield return new WaitForSeconds(speed);
                }
                else
                {
                    LeanTween.color(c[j+1], Color.white, 0.01f);
                }
                yield return new WaitForSeconds(0);
                LeanTween.color(c[j], Color.white, 0.01f);
            }
        }
        for (int i = 0; i < c.Count; i++)
        {
            LeanTween.color(c[i], Color.green, 1f);
        }
        InstructionText.text = "";
    }


    void SelectionCheckAnswers(int i, int j, List<GameObject> c){
        
        List<GameObject> answers = new List<GameObject>();
        for (int p = 0; p < c.Count; p++)
        {
            if (c[i].GetComponent<Cube>().Selected)
            {
                answers.Add(c[i]);
            }
        }
        if (c[i] == answers[0] || c[i] == answers[1])
        {
            if (c[j] == answers[0] || c[j] == answers[1])
            {
                InstructionText.text = "Good Job !!";
            }
            else
            {
                InstructionText.text = "Wrong Answer !!";
            }
        }
        else
        {
            InstructionText.text = "Wrong Answer !!";
        }
        DeselectCubes(Cubes);
        LeftToSelect = 2;
    }
    IEnumerator SelectionSort(List<GameObject> c)
    {
        CoroutineRunning = true;
        int min;
        GameObject temp;
        Vector3 tempPosition;

        for (int i = 0; i < c.Count; i++)
        {
            
            // Current position being evaluated set to blue colour.
            LeanTween.color(c[i], Color.blue, 0);
            min = i;
            yield return new WaitForSeconds(0.5f * speed);

            for (int j = i + 1; j < c.Count; j++)
            {

                if (i != 0 && i%2==0 && !WantToFinish && j%2==0)
                {
                    WaitUserInput();
                    if(!WantToFinish)
                    {
                        //SelectionCheckAnswers(i, j, c);
                    }
                }
                
                // Highlight the value thats currently being compared as cyan
                LeanTween.color(c[j], Color.cyan, 0);
                yield return new WaitForSeconds(0.1f * speed);

                // New lowest value found
                if (c[j].transform.localScale.y < c[min].transform.localScale.y)
                {
                    // If value isn't the initial value that was made blue, reset it to white.
                    if (min != i) LeanTween.color(c[min], Color.white, 0);
                    min = j;
                    // Set lowest value seen sow far to red
                    LeanTween.color(c[j], Color.red, 0);
                    continue;
                }
                // Set the colour of the compared block back to white after comparison completed.
                LeanTween.color(c[j], Color.white, 0);
            }

            // Swap to be made, including animation.
            if (min != i)
            {
                // If one of the items in the swap is the block set to blue, set it back to white.
                LeanTween.color(c[i], Color.white, 1f);

                // Swap places in the array.
                temp = c[i];
                c[i] = c[min];
                c[min] = temp;

                tempPosition = c[i].transform.localPosition;

                // Using LeanTween for animations, swaps the cubes
                LeanTween.moveLocalX(c[i],
                                     c[min].transform.localPosition.x,
                                     1);

                LeanTween.moveLocalZ(c[i],
                                     2,
                                     0.5f).setLoopPingPong(1);

                LeanTween.moveLocalX(c[min],
                                     tempPosition.x,
                                     1);

                LeanTween.moveLocalZ(c[min],
                                     4,
                                     0.5f).setLoopPingPong(1);

                LeanTween.color(c[i], Color.green, 1f);
                yield return new WaitForSeconds(1f);
                continue;
            }
            LeanTween.color(c[i], Color.green, 1f);
        }
        InstructionText.text = "";
    }
    bool CheckAnswersInsertion()
    {

        return true;
    }
    IEnumerator InsertionSort(List<GameObject> c)
    {
        for(int i=1; i<c.Count; i++)
        {
            yield return new WaitForSeconds(speed);

            this.temp = c[i];
            LeanTween.color(this.temp, Color.red, 0.01f);
            LeanTween.moveLocalZ(this.temp, 6, speed);

            int j = i - 1;

            float temp2 = -100; // for animation

            while (j >= 0 && c[j].transform.localScale.y > this.temp.transform.localScale.y)
            {

                WaitUserInput();
                yield return new WaitForSeconds(3);
                    if(!WantToFinish)
                    {
                      //check answers
                        
                    }
                    CoroutineRunning = true;
                yield return new WaitForSeconds(speed);

                LeanTween.moveLocalX(c[j], c[j].transform.localPosition.x + 2f, speed);
                c[j + 1] = c[j];

                temp2 = c[j].transform.localPosition.x;
                j--;
            }

            // for animation
            if(temp2 >= -15)
            {
                yield return new WaitForSeconds(speed);
                LeanTween.moveLocalX(this.temp, temp2, speed);
            }
            yield return new WaitForSeconds(speed);
            LeanTween.moveLocalZ(this.temp, 2.89f , speed);
            
            LeanTween.color(this.temp, Color.white, 0.01f);

            c[j + 1] = this.temp;
        }

        
        for (int i = 0; i < c.Count; i++)
        {
            LeanTween.color(c[i], Color.green, 1f);
        }
        InstructionText.text = "";
    }
    
}