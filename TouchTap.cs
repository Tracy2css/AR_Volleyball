using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TouchTap : MonoBehaviour
{
    public GameObject image1; // Reference to Image 1
    public GameObject image2; // Reference to Image 2
    public GameObject anim1;  // Reference to Animation 1
    public GameObject anim2;  // Reference to Animation 2

    private float lastTapTime1;
    private float lastTapTime2;
    private float lastTapTimeOther;
    private const float doubleTapTime = 0.3f; // Maximum interval for double-tap detection

    // Padding to increase the detection area
    public float padding = 0.5f;

    void Start()
    {
        // Initialize the state by showing image1 and image2
        ShowImages(true, true);
        ShowAnimations(false, false);
        Debug.Log("Initialization complete, images shown.");
    }

    void Update()
    {
        // Handle touch input each frame
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                ProcessTouchTap(touch.position);
            }
        }
    }

    void ProcessTouchTap(Vector2 tapPosition)
    {
        // Convert touch position to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(tapPosition.x, tapPosition.y, Camera.main.nearClipPlane));
        Debug.Log("Touch position: " + worldPosition);

        // Expand the touch area by padding
        Collider2D hit = Physics2D.OverlapBox(worldPosition, new Vector2(padding, padding), 0);

        if (hit != null)
        {
            Debug.Log("Hit: " + hit.gameObject.name);
            if (hit.gameObject == image1)
            {
                if (Time.time - lastTapTime1 < doubleTapTime)
                {
                    DoubleTap(image1); // Handle double-tap on Image 1
                    lastTapTime1 = 0; // Reset tap time after processing double-tap
                }
                else
                {
                    lastTapTime1 = Time.time; // Update last tap time
                }
            }
            else if (hit.gameObject == image2)
            {
                if (Time.time - lastTapTime2 < doubleTapTime)
                {
                    DoubleTap(image2); // Handle double-tap on Image 2
                    lastTapTime2 = 0; // Reset tap time after processing double-tap
                }
                else
                {
                    lastTapTime2 = Time.time; // Update last tap time
                }
            }
        }
        else
        {
            Debug.Log("No collider hit.");
            if (Time.time - lastTapTimeOther < doubleTapTime)
            {
                HandleNonAnimationAreaDoubleTap(); // Handle double-tap on non-animation area
                lastTapTimeOther = 0; // Reset tap time after processing double-tap
            }
            else
            {
                lastTapTimeOther = Time.time; // Update last tap time
            }
        }
    }

    void DoubleTap(GameObject image)
    {
        Debug.Log("DoubleTap on: " + image.name);
        ResetAnimations(); // Reset all animations before activating the correct one

        if (image == image1)
        {
            ShowImages(false, false);
            anim1.SetActive(true);
            image1.GetComponent<Collider2D>().enabled = false; // Disable image1 collider
            Debug.Log("Image 1 double-tap, animation 1 shown.");
        }
        else if (image == image2)
        {
            ShowImages(false, false);
            anim2.SetActive(true);
            image2.GetComponent<Collider2D>().enabled = false; // Disable image2 collider
            Debug.Log("Image 2 double-tap, animation 2 shown.");
        }
    }

    void HandleNonAnimationAreaDoubleTap()
    {
        ShowAnimations(false, false);
        ShowImages(true, true);
        image1.GetComponent<Collider2D>().enabled = true; // Enable image1 collider
        image2.GetComponent<Collider2D>().enabled = true; // Enable image2 collider
        Debug.Log("Non-animation area double-tap, images shown.");
    }

    void ShowImages(bool showImage1, bool showImage2)
    {
        image1.SetActive(showImage1);
        image2.SetActive(showImage2);
        Debug.Log("ShowImages: Image1=" + showImage1 + ", Image2=" + showImage2);
    }

    void ShowAnimations(bool showAnim1, bool showAnim2)
    {
        anim1.SetActive(showAnim1);
        anim2.SetActive(showAnim2);
        Debug.Log("ShowAnimations: Anim1=" + showAnim1 + ", Anim2=" + showAnim2);
    }

    void ResetAnimations()
    {
        anim1.SetActive(false);
        anim2.SetActive(false);
        Debug.Log("Animations reset.");
    }
}
