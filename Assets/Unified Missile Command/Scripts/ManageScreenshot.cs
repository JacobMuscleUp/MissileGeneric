using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// a class used to add the functionality of capturing and saving screenshots
public class ManageScreenshot : MonoBehaviour
{
    [SerializeField] Text notificationText;

    [Tooltip("the destination of screenshots taken")]
    [SerializeField] string screenshotDestination;
    [Tooltip("the width of the screenshot")]
    [SerializeField] int width;
    [Tooltip("the height of the screenshot")]
    [SerializeField] int height;
    [Tooltip("the name of the folder in Application.dataPath that stores the current screenshot")]
    [SerializeField] string screenShotDirectory = "Screenshots";
    [Tooltip("the key required to press in order to capture a screenshot")]
    [SerializeField] KeyCode captureScreenshotKey;

    string fileExtension = ".png";

    void Awake()
    {
        notificationText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(captureScreenshotKey))
            StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        var renderTexture = new RenderTexture(width, height, 24);
        var screenshotCamera = new GameObject().AddComponent<Camera>();
        screenshotCamera.CopyFrom(Camera.main);

        screenshotCamera.targetTexture = renderTexture;
        screenshotCamera.Render();
        RenderTexture.active = renderTexture;
        
        var texture2d = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture2d.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(screenshotCamera.gameObject);
        Destroy(renderTexture);

        var bytes = texture2d.EncodeToPNG();
        var rootPath = string.Format("{0}/{1}", Application.dataPath, screenShotDirectory);
        var screenshotFilename = string.Format("{0}/{1}{2}", rootPath, GetNextScreenshotFilename(), fileExtension);
        if (!System.IO.Directory.Exists(rootPath))
            System.IO.Directory.CreateDirectory(rootPath);
        System.IO.File.WriteAllBytes(screenshotFilename, bytes);

        notificationText.gameObject.SetActive(true);
        notificationText.text = screenshotFilename;
        yield return new WaitForSecondsRealtime(4);
        notificationText.gameObject.SetActive(false);
    }

    // HELPER FUNC
    string GetNextScreenshotFilename()
    {
        return "screenshot" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.ffff");
    }
    //! HELPER FUNC
}
