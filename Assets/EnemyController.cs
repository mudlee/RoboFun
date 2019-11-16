using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 from;
    public Vector2 to;
    public float speed;
    private Vector2 _target;
    private bool movingToTo = true;
    void Start()
    {
        transform.position = from;
        _target = to;
    }

    void FixedUpdate()
    {
        if(Mathf.Abs(Vector2.Distance(_target, transform.position)) <= 0.1) {
            if(movingToTo) {
                _target = from;
                movingToTo = false;
            }
            else {
                _target = to;
                movingToTo = true;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, _target, speed);
    }
}
