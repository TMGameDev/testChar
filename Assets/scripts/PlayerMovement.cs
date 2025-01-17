using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public int maxJumps = 2; // Nombre maximum de sauts autorisés
    private int jumpCounter; // Compteur de sauts restants

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Detection")]
    public LayerMask whatIsGround;
    private bool isGrounded;
    private bool wasGrounded; // Nouvelle variable pour suivre l'état précédent

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJumps();
    }

    private void Update()
    {
        // Vérifie si le joueur est au sol
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // Réinitialise le compteur de sauts si le joueur vient juste d'atterrir
        if (isGrounded && !wasGrounded) // Si le joueur vient d'atterrir
        {
            ResetJumps();
            Debug.Log("Reset des sauts");
        }

        // Met à jour l'état précédent
        wasGrounded = isGrounded;

        MyInput();

        // Ajuste le drag en fonction de l'état au sol
        rb.drag = isGrounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Gère les sauts
        if (Input.GetKeyDown(jumpKey) && jumpCounter > 0)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limite la vitesse du joueur
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Remet la vitesse verticale à zéro avant d'appliquer la force de saut
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Applique la force de saut
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Diminue le compteur de sauts restants
        jumpCounter--;
        Debug.Log("Sauts restants : " + jumpCounter);

    }

    private void ResetJumps()
    {
        jumpCounter = maxJumps; // Réinitialise les sauts restants au maximum
    }
}
