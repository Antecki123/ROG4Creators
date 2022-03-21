using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Elevator : MonoBehaviour
{
    [Header("Elevator Properties")]
    [SerializeField, Range(0f, 50f)] private float speed = 5f;
    [SerializeField, Range(0f, 30f)] private float range = 10f;

    [SerializeField] private bool horizontal;

    private Vector3 startPosition;

    private void Start() => startPosition = transform.position;

    private void Update()
    {
        float positionX;
        float positionY;
        float positionZ = transform.position.z;

        if (horizontal)
        {
            positionX = startPosition.x + Mathf.Cos(Time.time * speed / range) * range;
            positionY = transform.position.y;
        }
        else
        {
            positionX = transform.position.x;
            positionY = startPosition.y + Mathf.Cos(Time.time * speed / range) * range;
        }

        transform.position = new Vector3(positionX, positionY, positionZ);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<IDamageable<float>>().TakeDamage(100f);
    }

    private void OnDrawGizmos()
    {
        float sizeX;
        float sizeY;

        if (horizontal)
        {
            sizeX = range * 2;
            sizeY = GetComponent<Renderer>().bounds.size.y;
        }
        else
        {
            sizeX = GetComponent<Renderer>().bounds.size.x;
            sizeY = range * 2;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(sizeX, sizeY, 0));
    }
}