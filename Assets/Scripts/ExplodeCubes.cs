using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    private bool _collisionSet;
    public GameObject restartButton, explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Cube" && !_collisionSet)
        {
            for(int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
                child.SetParent(null);
            }
            restartButton.SetActive(true);
            Camera.main.transform.position -= new Vector3(0, 0, 2f);
            Camera.main.gameObject.AddComponent<CameraShake>();

            GameObject VFXObj = Instantiate(explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity) as GameObject;

            if(PlayerPrefs.GetString("Music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }

            Destroy(VFXObj, 2.5f);
            Destroy(collision.gameObject);
            _collisionSet = true;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
