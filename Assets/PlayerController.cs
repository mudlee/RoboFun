using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Text queueText;
    public Button[] buttons;
    private enum Direction {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    private readonly List<string> VALID_DIRS = new List<string>(){"L","R","U","D"};

    private const float SPEED = 0.03f;
    private Queue<Direction> queue = new Queue<Direction>();

    private Rigidbody2D _rigidBody2D;
    private SoundPlayer _soundPlayer;
    private Vector2? _target = null;
    private bool _executing = false;

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _soundPlayer = GetComponent<SoundPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_executing) {
            return;
        }

        if(!_target.HasValue && queue.Count>0) {
            UpdateQueueText();
            Direction dir = queue.Dequeue();
            _soundPlayer.Play(SoundType.MOVE);
            Debug.LogFormat("New Dir: {0}", dir);
            switch(dir) {
                case Direction.LEFT:
                    _target = new Vector2(transform.position.x-1, transform.position.y);
                    break;
                case Direction.RIGHT:
                    _target = new Vector2(transform.position.x+1, transform.position.y);
                    break;
                case Direction.UP:
                    _target = new Vector2(transform.position.x, transform.position.y+1);
                    break;
                case Direction.DOWN:
                    _target = new Vector2(transform.position.x, transform.position.y-1);
                    break;
            }
        }
    }

    void FixedUpdate() {
        if(_target.HasValue) {
            transform.position = Vector2.MoveTowards(transform.position, _target.Value, SPEED);

            if(Mathf.Abs(Vector2.Distance(_target.Value, transform.position)) <= 0.01) {
                Debug.Log("Stopped");
                _target = null;

                if(queue.Count == 0) {
                    _executing = false;
                    foreach(Button btn in buttons) {
                        btn.interactable = true;
                    }
                    queueText.text = "Nothing";
                    SceneManager.LoadScene("Lose");
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Enemy") {
            SceneManager.LoadScene("Lose");
            Destroy(gameObject);
            return;
        }
        else if(col.gameObject.tag == "Finish") {
            SceneManager.LoadScene("Win");
            Destroy(gameObject);
            return;
        }

        Debug.Log("Stopped");
        _target = null;
        SceneManager.LoadScene("Lose");
        Destroy(gameObject);
    }

    public void LeftButtonClick() {
        queue.Enqueue(Direction.LEFT);
        UpdateQueueText();
    }

    public void RightButtonClick() {
        queue.Enqueue(Direction.RIGHT);
        UpdateQueueText();
    }

    public void UpButtonClick() {
        queue.Enqueue(Direction.UP);
        UpdateQueueText();
    }

    public void DownButtonClick() {
        queue.Enqueue(Direction.DOWN);
        UpdateQueueText();
    }

    public void ResetButtonClick() {
        queue.Clear();
        queueText.text = "Nothing";
    }

    public void ExecuteButtonClick() {
        if(queue.Count == 0) {
            return;
        }

        _soundPlayer.Play(SoundType.EXECUTE);
        _executing = true;
        foreach(Button btn in buttons) {
            btn.interactable = false;
        }
    }

    void UpdateQueueText() {
        queueText.text = string.Join(",",queue.ToList());
    }
}
