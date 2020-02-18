using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 200f;
  [SerializeField] AudioClip mainEngine;
  [SerializeField] AudioClip success;
  [SerializeField] AudioClip death;
  [SerializeField] ParticleSystem mainEngineParticles;
  [SerializeField] ParticleSystem successParticles;
  [SerializeField] ParticleSystem deathParticles;
  private Rigidbody _rigidBody;
  private AudioSource _audioSource;

  enum State {
    Alive,
    Dying,
    Transcending
  }
  private State _state = State.Alive;

  void Start () {
    _rigidBody = this.GetComponent<Rigidbody> ();
    _audioSource = this.GetComponent<AudioSource> ();
  }

  void Update () {
    if (_state == State.Alive) {
      Thrust ();
      Rotate ();
    }
  }

  private void OnCollisionEnter (Collision collision) {
    if (_state != State.Alive) { return; } // Ignore collisions when dead

    switch (collision.gameObject.tag) {
      case "Friendly":
        // Do nothing
        break;
      case "Finish":
        StartSuccessSequence ();
        break;
      default:
        StartDeathSequence ();
        break;
    }
  }

  private void StartSuccessSequence () {
    _state = State.Transcending;
    _audioSource.Stop ();
    _audioSource.PlayOneShot (success);
    successParticles.Play ();
    Invoke ("LoadNextLevel", 1f); // Make time parameter
  }

  private void StartDeathSequence () {
    _state = State.Dying;
    _audioSource.Stop ();
    _audioSource.PlayOneShot (death);
    deathParticles.Play ();
    Invoke ("LoadFirstLevel", 1f); // Make time parameter
  }

  private void LoadFirstLevel () {
    SceneManager.LoadScene (0);
  }

  private void LoadNextLevel () {
    // TODO: Allow for more than 2 levels
    SceneManager.LoadScene (1);
  }

  private void Thrust () {
    if (Input.GetKey (KeyCode.Space)) {
      ApplyThrust ();
    } else {
      _audioSource.Stop ();
      mainEngineParticles.Stop ();
    }
  }

  private void ApplyThrust () {
    float thrustThisFrame = mainThrust * Time.deltaTime;
    _rigidBody.AddRelativeForce (Vector3.up * thrustThisFrame);

    if (!_audioSource.isPlaying) {
      _audioSource.PlayOneShot (mainEngine);
    }
    mainEngineParticles.Play ();
    Debug.Log ("is this calling");
  }

  private void Rotate () {
    _rigidBody.freezeRotation = true; // Take manual control of rotation

    float rotationThisFrame = rcsThrust * Time.deltaTime;
    if (Input.GetKey (KeyCode.A)) {
      transform.Rotate (Vector3.forward * rotationThisFrame);
    } else if (Input.GetKey (KeyCode.D)) {
      transform.Rotate (-Vector3.forward * rotationThisFrame);
    }

    _rigidBody.freezeRotation = false; // Resume physics control of rotation

  }
}