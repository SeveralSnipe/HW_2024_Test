using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private CharacterController characterController;
    private bool onPulpit = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing from the GameObject.");
        }

        StartCoroutine(WaitForData());
    }

    IEnumerator WaitForData()
    {
        while (DataManager.Instance.GetData() == null)
        {
            yield return null;
        }

        GameData data = DataManager.Instance.GetData();
        speed = data.player_data.speed;
    }

    void Update()
    {
        if (characterController != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed * 2;
            movementDirection.Normalize();

            characterController.SimpleMove(movementDirection * magnitude);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            CheckIfFalling();
        }
    }

    void CheckIfFalling()
    {
        if (transform.position.y < -1f)
        {
            GameManager.Instance.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered onTrigger");
        if (other.CompareTag("Pulpit") && !onPulpit)
        {
            onPulpit = true;
            Debug.Log("Player is on a pulpit.");
            GameManager.Instance.IncreaseScore();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pulpit"))
        {
            onPulpit = false;
            Debug.Log("Player left the pulpit.");
        }
    }
}
