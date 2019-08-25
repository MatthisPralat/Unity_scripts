using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Android_Free_Camera_Control : MonoBehaviour
{

    public float sensibility = 100;
    //Mon Canvas
    public Canvas InterfaceCanvas;

    // Rotate Buton
    public GameObject Joypad_Right_Contnair;
    public GameObject Joypad_Right_Button;
    public bool RightJoypadActive = false;
    public Vector2 RightButtonInitPosition;
    public int RightButtonID;
    public Vector2 RightButtonCurrentPosition;

    // Rotate Buton
    public GameObject Joypad_Left_Contnair;
    public GameObject Joypad_Left_Button;
    public bool LeftJoypadActive = false;
    public Vector2 LeftButtonInitPosition;
    public int LeftButtonID;
    public Vector2 LeftButtonCurrentPosition;

    // Game Object Controled
    public GameObject ControledGameObj;

    public enum JoysticControls
    {
        Rotate, Translate
    }


    // Use this for initialization
    void Start()
    {
        //La position Initiale de mes joystic
        RightButtonInitPosition = Joypad_Right_Button.GetComponent<RectTransform>().position;
        LeftButtonInitPosition = Joypad_Left_Button.GetComponent<RectTransform>().position;
    
    }

    //----------------------------------------------------------------
    // MOBILE POSITION
    //----------------------------------------------------------------
    void MobileButtonsCheck(Vector2 touchPos, int touchID)
    {
        //-- Left Controller
        RectTransform Left_Contnair_Rect = Joypad_Left_Contnair.GetComponent<RectTransform>();
        RectTransform Left_Button_Rect = Joypad_Left_Button.GetComponent<RectTransform>();
        if (RectTransformUtility.RectangleContainsScreenPoint(Left_Contnair_Rect, touchPos) && !LeftJoypadActive)
        {
            LeftJoypadActive = true;
            Left_Button_Rect.GetComponent<RectTransform>().position = touchPos;
            LeftButtonID = touchID;
        }
        //-- Right Controller
        RectTransform Right_Contnair_Rect = Joypad_Right_Contnair.GetComponent<RectTransform>();
        RectTransform Right_Button_Rect = Joypad_Right_Button.GetComponent<RectTransform>();   
        if (RectTransformUtility.RectangleContainsScreenPoint(Right_Contnair_Rect, touchPos) && !RightJoypadActive)
        {
            RightJoypadActive = true;
            Right_Button_Rect.GetComponent<RectTransform>().position = touchPos;
            RightButtonID = touchID;
        }
    }

    //----------------------------------------------------------------
    // MOBILE STOP
    //----------------------------------------------------------------
    void MobileButtonStop(int touchID)
    {
        if (LeftJoypadActive && LeftButtonID == touchID)
        {
            LeftJoypadActive = false;
            Joypad_Right_Button.GetComponent<RectTransform>().position = RightButtonInitPosition;
            LeftButtonID = -1;
        }
        if (RightJoypadActive && RightButtonID == touchID)
        {
            RightJoypadActive = false;
            Joypad_Right_Button.GetComponent<RectTransform>().position = LeftButtonInitPosition;
            RightButtonID = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //----------------------------------------------------------------
        // INPUT EDITOR  / MOBILE
        //----------------------------------------------------------------
        for (var i = 0; i < Input.touchCount; ++i) // ici je regarde le nombre de doigts sur l'ecrans
        {
            Touch touch = Input.GetTouch(i);
            // Quand je commence avec le touch
            if (touch.phase == TouchPhase.Began)
            {
                // là jenvoie dans mes boutons ma Position X et Y ainsi que l'ID de mon tuch 
                MobileButtonsCheck(new Vector2(touch.position.x, touch.position.y), touch.fingerId);
            }

            // Quand je bouge le touch
            if (touch.phase == TouchPhase.Moved) 
            {
                if (RightJoypadActive && RightButtonID == touch.fingerId)
                {
                    RightButtonCurrentPosition = touch.position;
                }
                if (LeftJoypadActive  && LeftButtonID == touch.fingerId)
                {
                    LeftButtonCurrentPosition = touch.position;
                }
            }


            // ------------------------------------------------------------------------------------------
            // Je desactive le tout
            // ------------------------------------------------------------------------------------------

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) // si j'enleve le touch
            {
                MobileButtonStop(touch.fingerId);
            }

        }
          

        //----------------------------------------------------------------
        //END
        //----------------------------------------------------------------



        Vector2 mousePos = Input.mousePosition;

        //----------------------------------------------------------------
        // RIGHT CONTROLLER 
        //----------------------------------------------------------------

        RectTransform Right_Contnair_Rect = Joypad_Right_Contnair.GetComponent<RectTransform>();
        RectTransform Right_Button_Rect = Joypad_Right_Button.GetComponent<RectTransform>();


        if (RectTransformUtility.RectangleContainsScreenPoint(Right_Contnair_Rect, mousePos) && Input.GetMouseButtonDown(0) )
        {
            RightJoypadActive = true;
        }

        if (RightJoypadActive == true)
        {
            Right_Button_Rect.GetComponent<RectTransform>().position = mousePos;

            float xpos = (Right_Button_Rect.position.x - RightButtonInitPosition.x);
            float ypos = Right_Button_Rect.position.y - RightButtonInitPosition.y;

            //       transform.Translate( new Vector3(xpos, ypos, 0) * Time.deltaTime * 4.5f, ControledGameObj.transform);

            ControledGameObj.transform.Rotate(new Vector3(-ypos / sensibility, xpos / sensibility, 0));

            // Tricks  to lock Z axis Thanks to internet
            float z = ControledGameObj.transform.eulerAngles.z;
            ControledGameObj.transform.Rotate(0, 0, -z);

            //ControledGameObj.transform.translate.x = ControledGameObj.transform.x + (Right_Button_Rect.position.x - RightButtonInitPosition.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Je relache tout");
            RightJoypadActive = false;
            Right_Button_Rect.GetComponent<RectTransform>().position = RightButtonInitPosition;
        }

        //----------------------------------------------------------------
        // LEFT CONTROLLER
        //----------------------------------------------------------------

        RectTransform Left_Contnair_Rect = Joypad_Left_Contnair.GetComponent<RectTransform>();
        RectTransform Left_Button_Rect = Joypad_Left_Button.GetComponent<RectTransform>();


        if (RectTransformUtility.RectangleContainsScreenPoint(Left_Contnair_Rect, mousePos) && Input.GetMouseButtonDown(0))
        {
            LeftJoypadActive = true;
        }

        if (LeftJoypadActive == true)
        {
            Left_Button_Rect.GetComponent<RectTransform>().position = mousePos;

            float xpos = Left_Button_Rect.position.x - LeftButtonInitPosition.x;
            float ypos = Left_Button_Rect.position.y - LeftButtonInitPosition.y;

            //       transform.Translate( new Vector3(xpos, ypos, 0) * Time.deltaTime * 4.5f, ControledGameObj.transform);

            ControledGameObj.transform.Translate(new Vector3(xpos / 1000, 0, ypos / 1000), Space.Self);
            //ControledGameObj.transform.translate.x = ControledGameObj.transform.x + (Left_Button_Rect.position.x - LeftButtonInitPosition.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Je relache tout");
            LeftJoypadActive = false;
            Left_Button_Rect.GetComponent<RectTransform>().position = LeftButtonInitPosition;
        }


    }


}

