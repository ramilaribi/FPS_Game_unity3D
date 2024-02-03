using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float mouseSensitivity = 200f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        // locking the cursor just put the screen in the middle 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotation arround the axe x (look up and down ) 
        xRotation -= mouseY;
        //Clamp rotation block la rotation : UP  and down 
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        // rotation arround the axe y (look left and right ) 
        yRotation += mouseX;
        //apply rotation to our transform 
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);    
    }
}
