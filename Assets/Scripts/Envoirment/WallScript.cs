using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Wall Click");
        Destroy(gameObject);
    }
}
