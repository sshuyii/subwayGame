using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeCloth : MonoBehaviour
{
    //all of alex's clothes
    public List<Sprite> AlexClothes;
    private List<Sprite> alexClothesTemp = new List<Sprite>();

    private bool isFirstOpen = true;


    
    //all the buttons, aka clothes in the machine
    //length should be 5, which is the number of buttons on screen

    public Button[] clothesInMachine;

    //length should also be 5, which is the number of buttons on screen
    private Sprite[] UIsprites;

    //all possible positions of buttons on the interface
    //public Vector3[] buttonPositions;
    


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AlexClothes.Count; i++)
        {
            alexClothesTemp.Add(AlexClothes[i]);
        }
        print(alexClothesTemp.Count);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCloth()
    {
        //randomly generate clothes when player opens the machine
        if(isFirstOpen)
        {
            for (int i = 0; i < clothesInMachine.Length; i++)
            {
                int randomIndex = Random.Range(0, alexClothesTemp.Count);
                print("random = " + randomIndex);
                clothesInMachine[i].image.sprite = AlexClothes[randomIndex];

                alexClothesTemp.Remove(alexClothesTemp[randomIndex]);
//            GameObject instantiatedObject = Instantiate(machine1[randomIndex], position1, Quaternion.identity) as GameObject;

            }

            isFirstOpen = false;
        }
        

        
    }
    
}
