using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    //a list of all the posts


    public InstagramController instagramController;

    private GameObject toothpastePost;
    private int postNum;
    private static ScreenshotHandler instance;

    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private Image postImage;
   
    private string ScreenCapDirectory;

    private Texture2D myScreenshot;

    void Start()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
        
        ScreenCapDirectory = Application.persistentDataPath;

//        postImage = GetComponent<Image>();
//        print(postImage.name);
    }
    
    public void TakeScreenshot()
    {
//        myCamera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
//        takeScreenshotOnNextFrame = true;
        //ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/Screenshots/CameraScreenshot.png");
        
        myScreenshot = Resources.Load<Texture2D>("Screenshots/CameraScreenshot");

        print(myScreenshot.name + "aaaaaaaa");

        TexToPng(CropImage());
        
        for (int i = 0; i < instagramController.postList.Count; i++)
        {
            instagramController.postList[i].transform.SetSiblingIndex(i);
            print(i + "dddd");
            print(instagramController.postList[i].transform.name + instagramController.postList[i].transform.GetSiblingIndex());
        }
    }

 
    Texture2D CropImage()
    {
        int width = Screen.width;
        int height = Screen.width;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        
        //Height of image in pixels
        for (int y = 0; y < tex.height; y++)
        {
            //Width of image in pixels
            for (int x = 0; x < tex.width; x++)
            {
                Color cPixelColour = myScreenshot.GetPixel(x, y + 200);
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

  
//    public static void TakeScreenshot_Static(int width, int height)
//    {
//        instance.TakeScreenshot(width, height);
//    }
}
