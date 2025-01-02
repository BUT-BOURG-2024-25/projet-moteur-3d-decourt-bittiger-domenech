using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private Camera mainCamera; // Caméra principale
    private Vector3 targetPosition; // Position cible où le joueur doit aller
    private bool isDragging = false; // Pour détecter si l'utilisateur glisse

    public float forwardSpeed = 5f; // Vitesse d'avancement automatique
    public float lateralSpeed = 0f; // Vitesse de déplacement latéral (réglable)

    void Start()
    {
        mainCamera = Camera.main; // Récupérer la caméra principale
        targetPosition = transform.position; // Initialiser la position cible
    }

    void Update()
    {
        // Déplacement automatique vers l'avant
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Debugging de la position et de la vitesse
        //Debug.Log($"Lateral Speed: {lateralSpeed}, Current X: {transform.position.x}, Target X: {targetPosition.x}");

        // Déplacement progressif sur l'axe X uniquement
        if (lateralSpeed > 0) // Se déplacer uniquement si la vitesse latérale est > 0
        {
            float step = lateralSpeed * Time.deltaTime; // Distance à parcourir ce frame
            float newX = Mathf.MoveTowards(transform.position.x, targetPosition.x, step);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        // Contrôle tactile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchStart(touch.position);
                    break;

                case TouchPhase.Moved:
                    OnTouchMove(touch.position);
                    break;

                case TouchPhase.Ended:
                    OnTouchEnd();
                    break;
            }
        }

        // Contrôle souris (pour tester dans l'éditeur)
        if (Input.GetMouseButtonDown(0))
        {
            OnTouchStart(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            OnTouchMove(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnTouchEnd();
        }
    }

    void OnTouchStart(Vector2 screenPosition)
    {
        isDragging = true;
        UpdateTargetPosition(screenPosition);
    }

    void OnTouchMove(Vector2 screenPosition)
    {
        if (isDragging)
        {
            UpdateTargetPosition(screenPosition);
        }
    }

    void OnTouchEnd()
    {
        isDragging = false;
    }

    void UpdateTargetPosition(Vector2 screenPosition)
    {
        // Convertir les coordonnées de l'écran en coordonnées du monde
        Vector3 screenPositionWithZ = new Vector3(screenPosition.x, screenPosition.y, mainCamera.WorldToScreenPoint(transform.position).z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPositionWithZ);

        // Définir la position cible uniquement sur l'axe X
        targetPosition = new Vector3(worldPosition.x, transform.position.y, transform.position.z);
    }
}
