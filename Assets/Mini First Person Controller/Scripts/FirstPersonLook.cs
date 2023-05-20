using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;
    private RaycastHit[] shootHits;
    [SerializeField] float gunRange = 20f;


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 direction = transform.forward;
        Debug.DrawRay(transform.position, direction * 10, Color.red, 2);
        shootHits = Physics.RaycastAll(transform.position, direction, gunRange);
        for (int i = 0; i < shootHits.Length; i++)
        {
            RaycastHit hit = shootHits[i];
            EnemyController enemy = hit.collider.gameObject.GetComponent<EnemyController>();
            if (enemy != null) enemy.TakeDamage(1);
            else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Bubble"))
            {
                hit.collider.gameObject.GetComponent<SphereCollider>().radius = 12f;
                hit.collider.gameObject.GetComponent<Bubble>().RadiusKill();
            }
        }

    }
}
