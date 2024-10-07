using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float rotationSpeed = 720.0f;
    
    private CharacterController controller;
    private Vector3 velocity;
    
    private float gravity = -9.81f;
    
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        if (controller.isGrounded)
        {
            velocity.y = 0f;
        }
        velocity.y += gravity * Time.deltaTime;
        
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        controller.Move((moveDirection * speed + new Vector3(0, velocity.y, 0)) * Time.deltaTime);
    }
}
