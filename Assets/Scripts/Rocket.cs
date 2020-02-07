using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 200f;
    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    void Start()
    {
        _rigidBody = this.GetComponent<Rigidbody>();
        _audioSource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        Thrust();
        Rotate();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK"); // TODO: Remove Line
                break;
            case "Fuel":
                print("Fuel Pickup"); // TODO: Remove Line
                break;
            default:
                print("Dead"); 
                // TODO: Kill Player
                break;
        }
    }
    
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustThisFrame = mainThrust * Time.deltaTime;
            _rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void Rotate()
    {
        _rigidBody.freezeRotation = true; // Take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        
        _rigidBody.freezeRotation = false; // Resume physics control of rotation

    }
}
