using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float speed = 5f;
    private Transform rotator;

    // Start is called before the first frame update
    void Start()
    {
        rotator = GetComponent<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(0, speed * Time.deltaTime, 0); 
    }
}
