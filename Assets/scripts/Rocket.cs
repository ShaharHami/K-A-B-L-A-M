using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    bool isTransitioning = false;
    [SerializeField] float rcsThrust = 300.0f;
    [SerializeField] float mainThrust = 2000.0f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip engineAudio;
    [SerializeField] AudioClip successAudio;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] ParticleSystem thrustParticle1;
    [SerializeField] ParticleSystem thrustParticle2;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;
    private bool collisionDetection = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!isTransitioning)
        {
            HandleThrustInput();
            HandleRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            HandleCheat();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning)
        {
            return;
        }
        if (!collisionDetection)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "friendly":
                //do nothing
                break;
            case "finish":
                //next level
                StartSuccessSequence();
                break;
            default:
                //die
                StartDeathSequence();
                break;
        }
    }
    private void StartDeathSequence()
    {
        thrustParticle1.Stop();
        thrustParticle2.Stop();
        isTransitioning = true;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        HandleAudioOneShot(deathAudio);
        deathParticle.Play();
        Invoke("ReloadLevel", levelLoadDelay);
    }
    private void StartSuccessSequence()
    {
        isTransitioning = true;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        HandleAudioOneShot(successAudio);
        successParticle.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void HandleAudioOneShot(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    private void HandleThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopThrust();
        }
    }
    private void StopThrust()
    {
        audioSource.Stop();
        thrustParticle1.Stop();
        thrustParticle2.Stop();
    }
    private void HandleRotateInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateManually(rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateManually(-rcsThrust * Time.deltaTime);
        }
    }

    private void RotateManually(float rotationSpeed)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationSpeed);
        rb.freezeRotation = false;
    }

    private void HandleCheat()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDetection = !collisionDetection;
        }
    }
    private void ApplyThrust()
    {
        float mainThrustForce = mainThrust * Time.deltaTime;
        rb.AddRelativeForce(Vector3.up * mainThrustForce);
        if (audioSource != null && !audioSource.isPlaying)
        {
            HandleAudioOneShot(engineAudio);
        }
        if (!thrustParticle1.isPlaying && !thrustParticle2.isPlaying)
        {
            thrustParticle1.Play();
            thrustParticle2.Play();
        }
    }
}
