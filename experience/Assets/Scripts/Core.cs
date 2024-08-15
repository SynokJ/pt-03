using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] private float _speed, _jumpSpeed, _centerSpeed, _maxVelocity;
    [SerializeField] private bool _core;

    private Rigidbody2D _rb;
    private bool _up, _right, _jump, _left;

    private List<GameObject> _tiles = new List<GameObject>();

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tile" && _core)
            _tiles.Add(collision.gameObject);
            //_tiles = Append(_tiles, collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "tile" && _core)
            _tiles.Remove(collision.gameObject);
            //_tiles = Remove(_tiles, collision.gameObject);
    }

    private void Move(Vector3 direction)
    {
        _rb.AddForce(direction * _speed * Time.deltaTime);
        if (_rb.velocity.magnitude >= _maxVelocity)
            _rb.velocity = _rb.velocity.normalized * _maxVelocity;
    }

    private void Inputs()
    {
        _up = Input.GetKey(KeyCode.UpArrow);
        _right = Input.GetKey(KeyCode.RightArrow);
        _jump = Input.GetKeyDown(KeyCode.Space);
        _left = Input.GetKey(KeyCode.LeftArrow);
    }

    private void Movements()
    {
        if (_left && !_up)
            Move(new Vector3(-1.0f, 0.0f, 0.0f));
        else if (_right && !_up)
            Move(new Vector3(1.0f, 0.0f, 0.0f));

        if (_jump && !_up)
            Move(new Vector3(0.0f, _jumpSpeed, 0.0f));

        if (_up && _left)
            Move(new Vector3(-1.0f, 1.0f / 1.5f, 0.0f));
        else if (_up && _right)
            Move(new Vector3(1.0f, 1.0f / 1.5f, 0.0f));
    }

    private void ToCenter()
    {
        int size = _tiles.Count;
        for (int i = 0; i < size; ++i)
        {
            if (_tiles[i] == null) continue;

            _tiles[i].gameObject.GetComponent<Rigidbody2D>().AddForce(
                (transform.position - _tiles[i].transform.position)
                * _centerSpeed * Time.deltaTime);
        }
    }

    //private GameObject[] Append(GameObject[] array, GameObject item)
    //{
    //    GameObject[] result = new GameObject[array.Length + 1];
    //    int size = array.Length;
    //    for (int i = 0; i < size; ++i)
    //        result[i] = array[i];
    //    result[result.Length - 1] = item;
    //    return result;
    //}

    //private GameObject[] Remove(GameObject[] array, GameObject item)
    //{
    //    GameObject[] result = new GameObject[array.Length - 1];
    //    int counter = 0;

    //    int size = array.Length;
    //    for (int i = 0; i < size; ++i)
    //    {
    //        if (array[i] != item)
    //        {
    //            result[counter] = array[i];
    //            counter++;
    //        }
    //    }
    //    return result;
    //}

    private void Update()
    {
        if (_core)
        {
            Inputs();
            Movements();
        }
    }

    private void FixedUpdate()
    {
        ToCenter();
    }
}
