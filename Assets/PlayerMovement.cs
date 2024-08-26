using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing from the GameObject.");
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(0.5f, 1, 1);
        }

        // Subscribe to the OnDataLoaded event
        DataManager.Instance.OnDataLoaded += OnGameDataLoaded;
    }

    private void OnGameDataLoaded()
    {
        GameData data = DataManager.Instance.GetData();
        if (data != null)
        {
            speed = data.player_data.speed;
            Debug.Log("Game data loaded and applied to player movement.");
        }
    }

    void Update()
    {
        if (characterController != null)
        {
            Debug.Log($"{speed}");
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
            movementDirection.Normalize();

            characterController.SimpleMove(movementDirection * magnitude);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        DataManager.Instance.OnDataLoaded -= OnGameDataLoaded;
    }
}

