using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ScreenshotHandler : MonoBehaviour
{
    //a list of all the posts
    
    public InstagramController InstagramController;
    private FinalCameraController FinalCameraController;
    private CalculateInventory CalculateInventory;

    private AudioSource shutterSound;

    private int PosterTakenNum = 0;//record how many posters have been used?
    //record which posture has been used
    private Dictionary<string, bool> usedPostures = new Dictionary<string, bool>();

    //record what Karara is wearing
    private string KararaTop;
    private string KararaBottom;
    private string KararaShoe;
    private bool KararaWork;

    private bool startFlash;
    public Image KararaTopImage;
    public Image KararaBottomImage;
    public Image KararaShoeImage;
    public Image KararaWorkImage;

    

    private GameObject toothpastePost;
    public int followerNum;
    public TextMeshProUGUI subwayFollower;
    
    private static ScreenshotHandler instance;

    public Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private Image postImage;
   
    private string ScreenCapDirectory;

    private Texture2D myScreenshot;

    private Texture2D renderResult;

    private IEnumerator coroutine;

    public Image photoBackground;
    
    private int width = Screen.width;
    private int height = Screen.height;


    public CanvasGroup Notice;
    public CanvasGroup myFlash;
    private bool flash;
    
   
    void Awake()
    {
    }
    void Start()
    {
        //height = width;
        instance = this;
        //myCamera = gameObject.GetComponent<Camera>();
        shutterSound = GetComponent<AudioSource>();
        
        
        ScreenCapDirectory = Application.persistentDataPath;
        FinalCameraController = GetComponent<FinalCameraController>();
        CalculateInventory = GameObject.Find("---InventoryController").GetComponent<CalculateInventory>();

//        postImage = GetComponent<Image>();
//        print(postImage.name);

        if(!FinalCameraController.isTutorial)
        {
            KararaTop = KararaTopImage.sprite.name;
            KararaBottom = KararaBottomImage.sprite.name;
            KararaShoe = KararaShoeImage.sprite.name;
        }
    }

    private bool createPostOnce;
    private void Update()
    {

//            print(InstagramController.AdAlreadyTakenList[InstagramController.currentBackground]);

        //todo:if all posters have been used
        //if all posters have been used
//        if (!InstagramController.AdAlreadyTakenList.ContainsValue(true))
//        {
//            //if more than 20 followers, chapter 1 succeeds
//            if (followerNum >= 20 && !createPostOnce)
//            {
//                InstagramController.CreatePersonalPagePost("nico", InstagramController.nicoLaterPost[3],"this is the end of chapter 1");
//                createPostOnce = true;//should be a better way to run this function only once
//
//                FinalCameraController.Show(FinalCameraController.messageCG);
//            }
//        }
        
        //set follower number
        if(!FinalCameraController.isTutorial)
        {
            InstagramController.followerNum.text = followerNum.ToString();
            InstagramController.subFollowerNum.text = followerNum.ToString();
            if (KararaWorkImage.sprite.name == "workCloth")
            {
                KararaWork = true;
            }
            else
            {
                KararaWork = false;
            }
        }
        
        if (flash)
        {
            if(FinalCameraController.isTutorial == false)
            {
                myFlash.alpha = myFlash.alpha - Time.deltaTime;
     
                if (myFlash.alpha <= 0)
                {
                    myFlash.alpha = 0;
                    flash = false;
                }
            }
            else
            {
                FinalCameraController.TutorialManager.pressScreenshot = true;
                flash = false;
                FinalCameraController.DisableInput(false);
            }
        }
    }

   
    private void addToKararaPage()
    {
        //RECORD THE POSTURE
//        if (usedPostures.ContainsKey(CalculateInventory.posNum.ToString()))
//        {
//            FinalCameraController.Show(Notice);
//            return;
//        }
//        else
//        {
//            usedPostures.Add(CalculateInventory.posNum.ToString(), true);
//            
//        }
//        
//        if (usedBackground.ContainsKey(InstagramController.currentBackground))
//        {
//            FinalCameraController.Show(Notice);
//            return;
//        }
//        else
//        {
//            usedBackground.Add(InstagramController.currentBackground, true);
//            
//        }

        if(!FinalCameraController.isTutorial)
        {
            usedPostures.Add(CalculateInventory.posNum.ToString(), true);
        }        
        
        //usedBackground.Add(InstagramController.currentBackground, true);
        
        //instantiate new post object     
        var newPost = Instantiate(InstagramController.photoPostPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        //set parent(probably a better way to do
        newPost.transform.parent = InstagramController.filmParent.transform;
        newPost.transform.localScale = Vector3.one;

        Texture2D sprites = CropImage();

        Rect rec = new Rect(0, 0, sprites.width, sprites.height);

        //change sprite to the newly taken photo
        newPost.GetComponent<Image>().sprite = Sprite.Create(sprites,rec,new Vector2(0,0),100f);


        if (!FinalCameraController.isTutorial)
        {
            //get the post text
            toothpastePost.transform.Find("Post").gameObject.GetComponent<Image>().sprite =
                Sprite.Create(sprites, rec, new Vector2(0, 0), 100f);
            var textList = toothpastePost.transform.Find("Post").gameObject.GetComponentsInChildren<Text>();



            //create comments
            var newComment = Instantiate(InstagramController.commentPrefab, new Vector3(0, 0, 0), Quaternion.identity,
                toothpastePost.transform.Find("Comments"));
            //set parent(probably a better way to do
//        newComment.transform.parent = toothpastePost.transform.Find("Comments");

            newComment.transform.localPosition = new Vector3(0f, 0f, 0);

            //get the text child
            var CommentText = newComment.GetComponentInChildren<TextMeshProUGUI>();


            print(CommentText.name);
            //get the profile
            var ProfileImage = newComment.GetComponent<Image>();

            InstagramController.replyParent = toothpastePost.transform.Find("Reply");

            //now this background cannot be used again
            InstagramController.AdAlreadyTakenList[InstagramController.currentBackground] = false;

            //generate different comment for each post
            if (InstagramController.currentBackground == "RV")
            {
                //record background
                newPost.GetComponent<RecordBackgroundPosture>().backgroundName = 0;


                //todo: comments need to occur after sometime
                CommentText.text = "That RV looks amazing. I'm gonna get one for myself.";
                ProfileImage.sprite = InstagramController.allProfile["nico"];

                //change the post text
                textList[0].text = "<b>Karara</b> Vocation";
                textList[1].text = "Today";

                //Add followers to Karara
//            if (KararaTop == "TopA1")
//            {
//                followerNum += 8;
//                print("asdfasdf;lkj;lkjasdf");
//            }
//            else
//            {
//                followerNum += 3;}
//            
//
//            if (KararaBottom == "BottomA1")
//            {
//                followerNum += 6;
//            }else
//            {
//                followerNum += 3;}
//
                if (KararaWork)
                {
                    followerNum += 3;
                }
                else
                {
                    followerNum += 8;
                }

            }
//        else if (InstagramController.currentBackground == "Toothpaste")
//        {
//            CommentText.text = "I like the way you dress.";
//            ProfileImage.sprite = InstagramController.allProfile["ojisan"];
//            
//        }
            else if (InstagramController.currentBackground == "FruitStand")
            {
                //record background
                newPost.GetComponent<RecordBackgroundPosture>().backgroundName = 1;

                CommentText.text = "Flavor of life";
                ProfileImage.sprite = InstagramController.allProfile["ojisan"];

                //change the post text
                textList[0].text = "<b>Karara</b> Fresh";
                textList[1].text = "Today";

                if (KararaWork)
                {
                    followerNum += 3;
                }
                else
                {
                    followerNum += 8;
                }
            }
            else if (InstagramController.currentBackground == "Park")
            {
                //record background
                newPost.GetComponent<RecordBackgroundPosture>().backgroundName = 2;

                CommentText.text = "Amusement parks are boring.";
                ProfileImage.sprite = InstagramController.allProfile["nico"];

                //change the post text
                textList[0].text = "<b>Karara</b> Relaxing";
                textList[1].text = "Today";

                if (KararaWork)
                {
                    followerNum += 3;
                }
                else
                {
                    followerNum += 8;
                }
            }
            else if (InstagramController.currentBackground == "Alcohol")
            {
                //record background
                newPost.GetComponent<RecordBackgroundPosture>().backgroundName = 3;

//            CommentText.text = "Amusement parks are boring.";
//            ProfileImage.sprite = InstagramController.allProfile["nico"];

                //change the post text
                textList[0].text = "<b>Karara</b> Cheers";
                textList[1].text = "Today";

                if (KararaWork)
                {
                    followerNum += 3;
                }
                else
                {
                    followerNum += 8;
                }
            }

        }
        FinalCameraController.ChangeToApp();
    }
    

    public void TakeScreenShotSprite()
    {
        
        //instantiate new post object     
        toothpastePost = Instantiate(InstagramController.PosturePostPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //set parent(probably a better way to do
        toothpastePost.transform.parent = InstagramController.postParent.transform;
            
        //move to the first of the list
        InstagramController.postList.Insert(0,toothpastePost);
            
        //get the post child
        Image[] postImageList = toothpastePost.transform.Find("PostFolder").gameObject.GetComponentsInChildren<Image>();
        
        //get the background
        for (int i = 0; i < postImageList.Length; i++)
        {
            postImageList[i].sprite = InstagramController.PosturePostImageList[i].sprite;
        }
        
        //re-arrange children object, so the latest is displayed as the first
        for (int i = 0; i < InstagramController.postList.Count; i++)
        {
            InstagramController.postList[i].transform.SetSiblingIndex(i);
        }
        
        StartCoroutine(ExampleCoroutine());

    }
    
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        
        yield return new WaitForSeconds(0.5f);

        //in tutorial
        if (FinalCameraController.isTutorial)
        {
            flash = true;
            
            myFlash.alpha = 1;

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(0.1f);
            addToKararaPage();
        }
        else if (!usedPostures.ContainsKey(CalculateInventory.posNum.ToString()))//if the posture is never used
        {
            flash = true;
            
            myFlash.alpha = 1;

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(0.1f);
            addToKararaPage();



            //After we have waited 5 seconds print the time again.
            Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        }
        else //if the posture has already been used
        {
           // myFlash.alpha = 1;

            FinalCameraController.Show(Notice);
                   
            if (usedPostures.ContainsKey(CalculateInventory.posNum.ToString()))
            {
                Notice.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "no same posture!";
            }
            
        }
        
    }
    
    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;        
            
            RenderTexture renderTexture = myCamera.targetTexture;


            renderResult =
                new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/Screenshots/CameraScreenshot.png", byteArray);

            Debug.Log("Saved CameraScreenshot.png");
            renderResult.Apply();
            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

   
    IEnumerator screenShotCoroutine()
    {
        yield return new WaitForEndOfFrame();
        string path = Application.dataPath + "/Resources/Screenshots/CameraScreenshot.png";

        Debug.Log("Saved CameraScreenshot.png");

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
 
        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        screenImage.Apply();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        //Convert to png(Expensive)
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        //Create new thread then save image to file
        new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(100);
            System.IO.File.WriteAllBytes(path, imageBytes);
        }).Start();
    }
    
    
    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }
    
    
    public void TakeScreenshot()
    {
        FinalCameraController.DisableInput(true);
        shutterSound.Play();
        //hide notice bubble
        if(!FinalCameraController.isTutorial)
        {
            FinalCameraController.Hide(Notice);
        }        
        
        //if the background is already used
        if (!InstagramController.AdAlreadyTakenList[InstagramController.currentBackground] && !FinalCameraController.isTutorial)
        {
            FinalCameraController.Show(Notice);
            Notice.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "no same background";

            return;
        }
        InstagramController.usedAdsList.Add(photoBackground.GetComponent<Image>().sprite.name);
        
        
        TakeScreenshot(width, height);
        
        //don't take the picture in the first half of the tutorial
        if (FinalCameraController.isTutorial && FinalCameraController.TutorialManager.tutorialNumber < 9)
        {
            flash = true;
            myFlash.alpha = 1;
            return;
        }
        else if(!usedPostures.ContainsKey(CalculateInventory.posNum.ToString()))//没用过这个姿势
        {
            InstagramController.takenNum++;
            //instantiate new post object     
            toothpastePost = Instantiate(InstagramController.PosturePostPrefabNew, new Vector3(0, 0, 0), Quaternion.identity);
            //set parent(probably a better way to do
            toothpastePost.transform.SetParent(InstagramController.postParent.transform);
            toothpastePost.transform.localScale = Vector3.one;
            //move to the first of the list
            InstagramController.postList.Insert(0,toothpastePost);
        
            toothpastePost.GetComponent<EntryTime>().time = FinalCameraController.entryTime;

            FinalCameraController.entryTime += 1;
            
            //if in the tutorial and taking the photo for the second time
            if(FinalCameraController.isTutorial && FinalCameraController.TutorialManager.tutorialNumber == 13)
            {
                FinalCameraController.TutorialManager.tutorialNumber = 14;
            }
        }
       
            
      
            
        StartCoroutine(ExampleCoroutine());

      
        //re-arrange children object, so the latest is displayed as the first
        for (int i = 0; i < InstagramController.postList.Count; i++)
        {
            InstagramController.postList[i].transform.SetSiblingIndex(i);
        }
        
        

//        coroutine = screenShotCoroutine();
//        StartCoroutine(coroutine);
        
        //myScreenshot = Resources.Load<Texture2D>("Screenshots/CameraScreenshot");        

        //print(myScreenshot.name + "aaaaaaaa");

//        TexToPng(CropImage());
                
//        for (int i = 0; i < instagramController.postList.Count; i++)
//        {
//            instagramController.postList[i].transform.SetSiblingIndex(i);
//            print(i + "dddd");
//            print(instagramController.postList[i].transform.name + instagramController.postList[i].transform.GetSiblingIndex());
//        }
    }


    public void ClickNotice()
    {
        FinalCameraController.Hide(Notice);
//        myFlash.alpha = 0;
//        FinalCameraController.ChangeToSubway();
//        FinalCameraController.myHSS.GoToScreen(4);

    }
    
    //todo: crop image not by perfect pixels, but relative to the screen size
    Texture2D CropImage()
    {
        Texture2D tex = new Texture2D(width, width, TextureFormat.RGB24, false);
        
        
        //Height of image in pixels
        for (int y = 0; y < tex.height; y++)
        {
            //Width of image in pixels
            for (int x = 0; x < tex.width; x++)
            {
                Color cPixelColour = renderResult.GetPixel(x ,  y + height/2 - Mathf.FloorToInt(0.5f * width));
                tex.SetPixel(x, y, cPixelColour);
            }
        }
        tex.Apply();
        return tex;
    }

    private void TexToPng(Texture2D tex)
    {
        Texture2D sprites = tex;
        Rect rec = new Rect(0, 0, sprites.width, sprites.height);
//        if(this.name == "Toothpaste")
//        {
            toothpastePost = Instantiate(InstagramController.postPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            toothpastePost.transform.parent = InstagramController.postParent.transform;
            
            //move to the first of the list
            InstagramController.postList.Insert(0,toothpastePost);
            
            //get the post child
            postImage = toothpastePost.transform.Find("Post").gameObject.GetComponent<Image>();
            print(postImage.name);

            postImage.sprite = Sprite.Create(sprites,rec,new Vector2(0,0),100f);

            print("toothpaste generated");
//        }
    }
    
    
  
  
}
