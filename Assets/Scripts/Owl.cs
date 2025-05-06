using KinematicCharacterController.Examples;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Owl : MonoBehaviour
{
    public PlayerStats playerStats;
    public float flightSpeed;
    public float speedMultiplyer;
    public float yawAmount;
    public Transform HumanPos;
    public float ImmunityAfterHit = 5;

    private float horizontalInput;
    private float verticalInput;
    private float yaw;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isFlying", true);
    }

    // Update is called once per frame
    void Update()
    {       
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            flightSpeed += speedMultiplyer;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            flightSpeed -= speedMultiplyer;
        }
        transform.localPosition += Time.deltaTime * flightSpeed * transform.forward;
        yaw += horizontalInput * yawAmount * Time.deltaTime;
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);
    }
    void OnEnable()
     {
        AudioManager.instance.Play("Hoot");
        AudioManager.instance.Play("Fly");
     }
    private void OnDisable()
    {
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))
        {
            playerStats.TakeDamage(1);
            UIManager.instance.UpdateHealth();
        }
    }
}
