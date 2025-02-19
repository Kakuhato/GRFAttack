using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * Vector3.right);
        this.transform.Translate(Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * Vector3.up);
        // Vector3 mousePos = Input.mousePosition;
        // mousePos.z = 10;
        // Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // worldMousePos.z = 0;
        // this.transform.position = worldMousePos;
    }
}
