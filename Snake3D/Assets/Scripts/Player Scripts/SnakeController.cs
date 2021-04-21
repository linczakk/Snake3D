using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    // Movement handlers
    [HideInInspector] public PlayerDirection Direction;

    [HideInInspector] public float StepLength = 0.2f;

    [HideInInspector] public float MovementFrequency = 0.1f;

    private float _counter;
    private bool _move;

    private List<Vector3> _deltaPosition;

    private List<Rigidbody> _nodesRigidbodies;

    private Rigidbody _mainRigidbody;
    private Rigidbody _headRigidbody;
    private GameObject _head;

    //handling generation of new body part
    [SerializeField]
    private GameObject _tailPrefab;
    private readonly int _baseNumberOfNodes = 3;
    private bool _createNodeAtTail;
    private List<GameObject> _nodes;

    private void Awake()
    {
        Time.timeScale = 1f;
        _mainRigidbody = GetComponent<Rigidbody>();

        _deltaPosition = GetPositions();

        InitSnakeNodes();
        InitSnake();
    }
    private List<Vector3> GetPositions() => new List<Vector3>()
                                            {
                                                 new Vector3(-StepLength, 0f), // Left
                                                 new Vector3(0f, StepLength), // Up
                                                 new Vector3(StepLength, 0f), // Right
                                                 new Vector3(0f, -StepLength) // Down
                                            };
    

    private void Update()
    {
        CheckMovementFrequency();
    }

    private void FixedUpdate()
    {
        if (_move)
        {
            _move = false;
            Move();
        }
    }

    private void InitSnakeNodes()
    {
        _nodesRigidbodies = new List<Rigidbody>();
        _nodes = new List<GameObject>();

        

        for (int i = 0; i < _baseNumberOfNodes; i++)
        {
            _nodesRigidbodies.Add(transform.GetChild(i).GetComponent<Rigidbody>());
        }
        _headRigidbody = _nodesRigidbodies[0];
        _head = transform.GetChild(0).gameObject;
        
    }

    private void InitSnake()
    {
        SetDirectionRandom();
        //arrangement of body parts
        switch (Direction)
        {
            case PlayerDirection.RIGHT:

                _nodesRigidbodies[1].position = _nodesRigidbodies[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                _nodesRigidbodies[2].position = _nodesRigidbodies[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);

                break;

            case PlayerDirection.LEFT:

                _nodesRigidbodies[1].position = _nodesRigidbodies[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                _nodesRigidbodies[2].position = _nodesRigidbodies[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);

                break;

            case PlayerDirection.UP:

                _nodesRigidbodies[1].position = _nodesRigidbodies[0].position - new Vector3(0f, Metrics.NODE, 0f);
                _nodesRigidbodies[2].position = _nodesRigidbodies[0].position - new Vector3(0f, Metrics.NODE * 2f, 0f);


                break;

            case PlayerDirection.DOWN:

                _nodesRigidbodies[1].position = _nodesRigidbodies[0].position + new Vector3(0f, Metrics.NODE, 0f);
                _nodesRigidbodies[2].position = _nodesRigidbodies[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);

                break;
        }

    }
    private void SetDirectionRandom()
    {
        var dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        Direction = (PlayerDirection)dirRandom;
    }

    private void Move()
    {
        var dPosition = _deltaPosition[(int)Direction];

        _head.transform.rotation = Quaternion.LookRotation(dPosition, Vector3.forward) * Quaternion.Euler(90f, 0f, 90f);

        var parentPos = _headRigidbody.position;
        Vector3 prevPosition;

        _mainRigidbody.position += dPosition;
        _headRigidbody.position += dPosition;

        for (int i = 1; i < _nodesRigidbodies.Count; i++)
        {
            prevPosition = _nodesRigidbodies[i].position;

            _nodesRigidbodies[i].position = parentPos;
            parentPos = prevPosition;
        }

        // check if new node need to be created
        if (_createNodeAtTail)
        {
            _createNodeAtTail = false;

            var newNode = Instantiate(_tailPrefab, _nodesRigidbodies[_nodesRigidbodies.Count - 1].position, Quaternion.identity);

            newNode.transform.SetParent(transform, true);
            _nodes.Add(newNode);
            _nodesRigidbodies.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    private void CheckMovementFrequency()
    {
        _counter += Time.deltaTime;

        if (_counter >= MovementFrequency)
        {
            _counter = 0f;
            _move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        // It prevent from turning back snake (in same line) 
        if (dir == PlayerDirection.UP && Direction == PlayerDirection.DOWN ||
           dir == PlayerDirection.DOWN && Direction == PlayerDirection.UP ||
           dir == PlayerDirection.RIGHT && Direction == PlayerDirection.LEFT ||
           dir == PlayerDirection.LEFT && Direction == PlayerDirection.RIGHT)
        {
            return;
        }

        Direction = dir;

        ForceMove();
    }

    private void ForceMove()
    {
        _counter = 0;
        _move = false;

        Move();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag(Tags.FOOD))
        {
            OnPickingFood(target.gameObject);
        }

        if (target.CompareTag(Tags.WALL) || target.CompareTag(Tags.TAIL))
        {
            GameplayController.Instance.OnGameOver();
        }

        if(target.CompareTag(Tags.BOMB))
        {
            OnPickingBomb(target.gameObject);
        }
    }
    private void OnPickingFood(GameObject food)
    {
        food.SetActive(false);
        _createNodeAtTail = true;

        ScoreController.Instance.IncreaseScore();
        AudioManager.Instance.PlayPickUpSound();
    }

    private void OnPickingBomb(GameObject bomb)
    {
        if (_nodes.Count == 0)
        {
            GameplayController.Instance.OnGameOver();
        }
        else
        {
            AudioManager.Instance.DetonateBombSound();
            bomb.SetActive(false);
           
            ScoreController.Instance.DecreaseScore();

            _nodesRigidbodies.RemoveAt(_nodesRigidbodies.Count - 1);

            Destroy(_nodes[_nodes.Count - 1]);
            _nodes.RemoveAt(_nodes.Count - 1);
        }
    }
}
