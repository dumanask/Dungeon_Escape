using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;

    [SerializeField]
    private float _jumpForce = 5.0f;
    private bool _resetJump = false;      
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _speed = 2.5f;
    private bool _grounded = false;

    
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSprite;
    private SpriteRenderer _swordArcSprites;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _swordArcSprites = transform.GetChild(1).GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();
    }

    bool IsGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 1f, _groundLayer);
        if (hitInfo.collider != null)
        {
            if (_resetJump == false)
            {
                _playerAnim.Jump(false);
                return true;
            }
        }
        return false;
    }
    void Movement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        _grounded = IsGrounded();

        if(move > 0)
        {
            Flip(true);
        }
        else if(move < 0)
        {
            Flip(false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
           _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
           StartCoroutine(ResetJumpRoutine());
            _playerAnim.Jump(true);
        }

        _rigid.velocity = new Vector2(move * _speed, _rigid.velocity.y);

        _playerAnim.Move(move);
    }
    void Attack()
    {
        _grounded = IsGrounded();

        if(Input.GetMouseButtonDown(0) && IsGrounded() == true)
        {
            _playerAnim.Attack();
        }
    }

    void Flip(bool faceRight)
    {
        if (faceRight == true)
        {
            _playerSprite.flipX = false;
            _swordArcSprites.flipX = false;
            _swordArcSprites.flipY = false;

            Vector3 newPos = _swordArcSprites.transform.localPosition;
            newPos.x = 1.01f;
            _swordArcSprites.transform.localPosition = newPos;

        }
        else if (faceRight == false)
        {
            _playerSprite.flipX = true;
            _swordArcSprites.flipX = true;
            _swordArcSprites.flipY = true;

            Vector3 newPos = _swordArcSprites.transform.localPosition;
            newPos.x = -1.01f;
            _swordArcSprites.transform.localPosition = newPos;
        }
    }
    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }
   
}
