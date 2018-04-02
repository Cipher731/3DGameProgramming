using UnityEngine;

public static class MovementExtension
{
    public static void MoveTowards(this GameObject gameObject, Vector3 destination)
    {
        gameObject.GetComponent<Movement>().MoveTowards(destination);
    }

    public static bool IsIdle(this GameObject gameObject)
    {
        return !gameObject.GetComponent<Movement>().IsMoving;
    }
}

public class Movement : MonoBehaviour
{
    private const float Speed = 10;
    private Vector3 _destination;

    public bool IsMoving { get; private set; }

    public void MoveTowards(Vector3 destination)
    {
        _destination = destination;
        IsMoving = true;
    }

    private void Update()
    {
        if (!IsMoving) return;
        var target = transform.position.y < _destination.y || Mathf.Approximately(transform.position.x, _destination.x)
            ? new Vector3(transform.position.x, _destination.y)
            : new Vector3(_destination.x, transform.position.y);

        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);

        if (transform.position == _destination) IsMoving = false;
    }
}