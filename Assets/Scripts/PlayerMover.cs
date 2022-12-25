using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _headChekingRadius;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private Transform _headChecker;
    [SerializeField] private int _maxHp;
    
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _crouchAnimationKey;
    [SerializeField] private string _runAnimationKey;
    [SerializeField] private string _jumpAnimationKeySpace;
    [SerializeField] private string _jumpAnimationKeyAir;

    [Header("UI")] 
    [SerializeField] private TMP_Text _coinAnountText;
    [SerializeField] private Slider _hpBar;
    
    private float _direction;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;

    public bool CanClimb { private get; set; }
    private int _coinAmount;
    private int _currentHp;

    public int Coins
    {
        get => _coinAmount;
        set
        {
            _coinAmount = value;
            _coinAnountText.text = value.ToString();
        }
        
    }

    private int CurrentHp
    {
        get => _currentHp;
        set
        {
            _currentHp = value;
            _hpBar.value = value;
            Debug.Log(value);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Coins = 0;
        _hpBar.maxValue = _maxHp;
        CurrentHp = _maxHp;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        _verticalDirection = Input.GetAxisRaw("Vertical");
        
        if (_direction < 0 && _spriteRenderer.flipX == false)
        {
            _spriteRenderer.flipX = true;
        }
        else if(_direction > 0 && _spriteRenderer.flipX == true)
        {
            _spriteRenderer.flipX = false;  
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }

        _crawl = Input.GetKey(KeyCode.C);
       
        // if (Input.GetKeyDown(KeyCode.Space) && doubleJump)
        // {
        //     _rigidbody.AddForce(Vector2.up * _jumpForce);
        //     doubleJump = false;
        // }

        //Debug.Log(_crawl);
        //Debug.Log(canJump || doubleJump);
        
        //if(Input.GetKeyDown(KeyCode.A)&&Input.GetKeyDown(KeyCode.LeftArrow))
        //if(Input.GetKeyDown(KeyCode.A)&&Input.GetKeyDown(KeyCode.LeftArrow))
    }
    
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);
        _animator.SetInteger(_runAnimationKey, (int)_direction);
        bool _canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        bool _canStand = !Physics2D.OverlapCircle(_headChecker.position, _headChekingRadius, _whatIsGround);
        _headCollider.enabled = !_crawl && _canStand;
        //Debug.Log("Can stand "+ _canStand);
        //Debug.Log("Crawl "+ _crawl);
        _animator.SetBool(_crouchAnimationKey, !_headCollider.enabled);
        _animator.SetBool(_jumpAnimationKeySpace, _jump);
        _animator.SetBool(_jumpAnimationKeyAir, !_canJump);
        
        if (_jump && _canJump)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

        if (CanClimb)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _verticalDirection * _speed);
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = 1;
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            Debug.Log("Died");
            gameObject.SetActive(false);
            Invoke(nameof(ReloadScene), 1f);
        }
    }
    
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _hpBar.maxValue = _maxHp;
        CurrentHp = _maxHp;
    }
    private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_headChecker.position, _headChekingRadius);
        }
}
