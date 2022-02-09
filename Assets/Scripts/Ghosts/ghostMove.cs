using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostMove : MonoBehaviour
{
    public Rigidbody2D _rigidbody;
    public SpriteRenderer _spriteRenderer;

    [SerializeField]
    public float timeToStart;
    [SerializeField]
    public Sprite _spritePowerUpActive;
    [SerializeField]
    public Sprite _spriteNormal;

    public float speed = 0.05f;
    private float dist = 1.5f;
    private Vector2[] directions = {Vector2.up, Vector2.right, -Vector2.up, -Vector2.right};
    private Vector2 direction;
    private bool onStart;
    private bool started;
    private Vector2 startPosition = new Vector2(13.75f, 20f);
    private Vector2 pos;

    private bool _superPacMan;

    List<Vector2> posibleDirections;
    List<Vector2> pastOptions = new List<Vector2>();

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        onStart = false;
        started = true;
        _superPacMan = false;

        GameEventsController.current.onGetPowerUp += setSuperPacman;
        GameEventsController.current.onLostPowerUp += unsetSuperPacman;
    }

    void FixedUpdate() {
        if (timeToStart <= 0) {
            if (started) {
                pos = gameObject.transform.position;
                onStart = true;
                if (Mathf.Abs(gameObject.transform.position.x-startPosition.x) > 0.05) {
                    pos.x += (startPosition.x-gameObject.transform.position.x)*speed;
                }
                else if (gameObject.transform.position.y < startPosition.y) {
                    pos += Vector2.up*speed;
                }
                else {
                    started = false;
                    onStart = false;
                }
                Vector3 finalPos = pos;
                gameObject.transform.position = finalPos;
            }
        }
        else {
            timeToStart -= Time.deltaTime;
        }

        if (!onStart) {
            posibleDirections = new List<Vector2>();

            bool newOptions = false;
            foreach (Vector2 dir in directions) {
                if (freeToMove(dir)) {
                    posibleDirections.Add(dir);
                    if (!pastOptions.Contains(dir)) {
                        newOptions = true;
                    }
                }
            }

            if (newOptions || posibleDirections.Count != pastOptions.Count) {
                if (Random.Range(0, 10) > 8 || !(posibleDirections.Contains(direction))) {
                    direction = posibleDirections[Random.Range(0, posibleDirections.Count)];
                }
                pastOptions = posibleDirections;
            }

            Vector3 finalDirection = direction*speed;
            gameObject.transform.position = gameObject.transform.position + finalDirection;

        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "PacMan") {
            if (_superPacMan) {
                Destroy(gameObject);
            }
            else {
                GameEventsController.current.GameOver();
                Destroy(collider.gameObject);
            }
        }
    }

    bool freeToMove(Vector2 direction) {
        RaycastHit2D[] hits;

        Vector2 pos1 = gameObject.transform.position;
        Vector2 pos2 = gameObject.transform.position;
        if (direction.x == 0) {
            pos1.x += 0.5f;
            pos2.x -= 0.5f;
        }
        else if (direction.y == 0) {
            pos1.y += 0.5f;
            pos2.y -= 0.5f;
        }

        hits = Physics2D.RaycastAll(pos1, direction, dist);
        Debug.DrawRay(pos1, direction*dist, Color.green);
        bool hitted1 = false;
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.name == "maze") {
                hitted1 = true;
            }
        }

        hits = Physics2D.RaycastAll(pos2, direction, dist);
        Debug.DrawRay(pos2, direction*dist, Color.green);
        bool hitted2 = false;
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.name == "maze") {
                hitted2 = true;
            }
        }
        return !(hitted1 || hitted2);
    }

    private void setSuperPacman() {
        _superPacMan = true;
        _spriteRenderer.sprite = _spritePowerUpActive;
    }

    private void unsetSuperPacman() {
        _superPacMan = false;
        _spriteRenderer.sprite = _spriteNormal;
    }
}
