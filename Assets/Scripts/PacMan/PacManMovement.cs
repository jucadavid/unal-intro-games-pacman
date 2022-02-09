using UnityEngine;
using System.Collections;

public class PacManMovement : MonoBehaviour {
    public Animator _animator;

    public float speed = 0.2f;

    public Vector3 direction;
    public Vector3 destination;

    void Start() {
        destination = Vector3.zero;
        destination.x = speed;

        _animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) {
            direction.y = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            direction.y = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            direction.x = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            direction.x = -1;
        }

        if (direction != Vector3.zero) {
            _animator.SetFloat("DirX", direction.x);
            _animator.SetFloat("DirY", direction.y);
            destination = direction*speed;
        }
        transform.position += destination;
    }
}
