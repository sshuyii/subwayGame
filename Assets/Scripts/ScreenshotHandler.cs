using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    //a list of all the posts


    public InstagramController instagramController;

    private GameObject toothpastePost;
    private int postNum;
    private static ScreenshotHandler instance;

    public Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private Image postImage;
   
    private string ScreenCapDirectory;

    private Texture2D myScreenshot;

    private Texture2D renderResult;

    private IEnumerator coroutine;

    
    private int width = Screen.width;
    private int height = Screen.height;
    
    void Awake()
    {
    }
    void Start()
    {
        //height = width;
        instance = this;
        //myCamera = gameObject.GetComponent<Camera>();
        
        ScreenCapDirectory = Application.persistentDataPath;

//        postImage = GetComponent<Image>();
//        print(postImage.name);
    }

    private void addToKararaPage()
    {
        //instantiate new post object     
        var newPost = Instantiate(instagramController.photoPostPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //set parent(probably a better way to do
        newPost.transform.parent = instagramController.filmParent.transform;
        
        Texture2D sprites = CropImage();

        Rect rec = new Rect(0, 0, sprites.width, sprites.height);

        //change sprite to the newly taken photo
        newPost.GetComponent<Image>().sprite = Sprite.Create(sprites,rec,new Vector2(0,0),100f);
        
        toothpastePost.transform.Find("Post").gameObject.GetComponent<Image>().sprite = Sprite.Create(sprites,rec,new Vector2(0,0),100f);
    
        //create comments
        var newComment = Instantiate(instagramController.commentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //set parent(probably a better way to do
        newComment.transform.parent = toothpastePost.transform.Find("Comments");

        //get the text child
        newComment.transform.GetComponentInChildren<Text>().text = "I don't like the environment.";
    }
    

    public void TakeScreenShotSprite()
    {
        
        //instantiate new post object     
        toothpastePost = Instantiate(instagramController.PosturePostPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //set parent(probably a better way to do
        toothpastePost.transform.parent = instagramController.postParent.transform;
            
        //move to the first of the list
        instagramController.postList.Insert(0,toothpastePost);
            
        //get the post child
        Image[] postImageList = toothpastePost.transform.Find("PostFolder").gameObject.GetComponentsInChildren<Image>();
        
        //get the background
        for (int i = 0; i < postImageList.Length; i++)
        {
            postImageList[i].sprite = instagramController.PosturePostImageList[i].sprite;
        }
        
        //re-arrange children object, so the latest is displayed as the first
        for (int i = 0; i < instagramController.postList.Count; i++)
        {
            instagramController.postList[i].transform.SetSiblingIndex(i);
        }
        
        StartCoroutine(ExampleCoroutine());

    }
    
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        addToKararaPage();
        
       
        
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        
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
//        myCamera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
//        takeScreenshotOnNextFrame = true;
        //ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/Screenshots/CameraScreenshot.png");
        
        TakeScreenshot(width, height);
        
        //instantiate new post object     
        toothpastePost = Instantiate(instagramController.PosturePostPrefabNew, new Vector3(0, 0, 0), Quaternion.identity);
        //set parent(probably a better way to do
        toothpastePost.transform.parent = instagramController.postParent.transform;
            
        //move to the first of the list
        instagramController.postList.Insert(0,toothpastePost);
            
        StartCoroutine(ExampleCoroutine());

      
        //re-arrange children object, so the latest is displayed as the first
        for (int i = 0; i < instagramController.postList.Count; i++)
        {
            instagramController.postList[i].transform.SetSiblingIndex(i);
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


    Texture2D CropImage()
    {
        Texture2D tex = new Texture2D(width, width, TextureFormat.RGB24, false);
        
        //Height of image in pixels
        for (int y = 0; y < tex.height; y++)
        {
            //Width of image in pixels
            for (int x = 0; x < tex.width; x++)
            {
                Color cPixelColour = renderResult.GetPixel(x , y + 200);
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
            toothpastePost = Instantiate(instagramController.postPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            toothpastePost.transform.parent = instagramController.postParent.transform;
            
            //move to the first of the list
            instagramController.postList.Insert(0,toothpastePost);
            
            //get the post child
            postImage = toothpastePost.transform.Find("Post").gameObject.GetComponent<Image>();
            print(postImage.name);

            postImage.sprite = Sprite.Create(sprites,rec,new Vector2(0,0),100f);

            print("toothpaste generated");
//        }
    }
   

  
}
