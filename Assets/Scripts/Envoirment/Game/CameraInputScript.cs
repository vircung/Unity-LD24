using UnityEngine;
using System.Collections;

public class CameraInputScript : MonoBehaviour
{

    float moveSpeed = 5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;

        transform.Translate(new Vector3(x, 0, y), Space.World);
    }
}
