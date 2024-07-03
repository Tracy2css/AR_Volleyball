using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TouchTap_02 : MonoBehaviour
{
    public GameObject image1; // img02
    public GameObject image2; // img02
    public GameObject anim1;  // gif01
    public GameObject anim2;  // gif02

    private float timePressStarted;
    private bool newTouch = false;
    private bool isAnimating = false;

    private GameObject currentActiveImage;

    void Start()
    {
        // Initialize the state by showing image1 and image2
        currentActiveImage = null;
        ShowImages(true, true);
        ShowAnimations(false, false);
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Vector2.zero);

            if (hitInfo.collider != null)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    timePressStarted = Time.time;
                    newTouch = true;
                }
                else if (touch.phase == TouchPhase.Stationary && newTouch)
                {
                    if (Time.time - timePressStarted > 1f)
                    {
                        newTouch = false;

                        //  Check which object is long-pressed
                        if (hitInfo.collider.gameObject == image1)
                        {
                            // Long press on image1, play animation1
                            HandleLongPress(image1, anim1);
                        }
                        else if (hitInfo.collider.gameObject == image2)
                        {
                            // Long press on image2, play animation2
                            HandleLongPress(image2, anim2);
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    // Handle end of long press
                    newTouch = false;
                    if (isAnimating)
                    {
                        HandleAnimationEnd();
                    }
                }
            }
        }
    }

    void HandleLongPress(GameObject image, GameObject animation)
    {
        Debug.Log("Handling long press for: " + image.name); // Debug log

        // Hide the image instead of destroying it
        image.SetActive(false);

        // Show the animation
        animation.SetActive(true);
        currentActiveImage = animation;
        isAnimating = true;
    }

    void HandleAnimationEnd()
    {
        // Hide the current animation
        if (currentActiveImage != null)
        {
            currentActiveImage.SetActive(false);
            currentActiveImage = null;
        }

        // Show image1 and image2 again
        ShowImages(true, true);
        isAnimating = false;
    }

    void ShowImages(bool showImage1, bool showImage2)
    {
        image1.SetActive(showImage1);
        image2.SetActive(showImage2);
    }

    void ShowAnimations(bool showAnim1, bool showAnim2)
    {
        anim1.SetActive(showAnim1);
        anim2.SetActive(showAnim2);
    }
}
