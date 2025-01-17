using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensY; // Sensibilité de la souris (vertical)
    public float sensX; // Sensibilité de la souris (horizontal)

    public Transform orientation; // Orientation pour le mouvement du joueur
    public Transform player;      // Transform du joueur
    public Transform cameraTarget; // Point de focalisation de la caméra (par exemple, au-dessus de la tête du joueur)
    public float distanceFromTarget = 5f; // Distance entre la caméra et le joueur
    public Vector2 pitchLimits = new Vector2(-40f, 80f); // Limites verticales (haut/bas)

    float xRotation; // Rotation sur l'axe X (vertical)
    float yRotation; // Rotation sur l'axe Y (horizontal)

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Lecture des mouvements de la souris
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Mise à jour de la rotation
        yRotation += mouseX; // Rotation horizontale
        xRotation -= mouseY; // Rotation verticale
        xRotation = Mathf.Clamp(xRotation, pitchLimits.x, pitchLimits.y); // Limiter l'angle vertical

        // Rotation autour de la cible
        Quaternion camRotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.position = cameraTarget.position - camRotation * Vector3.forward * distanceFromTarget;

        // Orientation de la caméra vers la cible
        transform.LookAt(cameraTarget);

        // Synchronisation de la rotation du joueur avec la caméra (axe Y uniquement)
        player.rotation = Quaternion.Euler(0, yRotation, 0);

        // Synchronisation de l'orientation pour les mouvements
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
