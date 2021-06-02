using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
 {
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float force;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float minimalHeight;
    [SerializeField] private bool isCheatMode;
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Arrow arrow;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float shootForce;
    [SerializeField] private float cooldown;
    [SerializeField] private float demageForce;
    [SerializeField] private int arrowsCount = 3;
    [SerializeField] private Health health;
    [SerializeField] private Item item;
    [SerializeField] private BuffReciever buffReciever;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioSource shootSource;
  



    public Health Health { get { return health; } }
    private Arrow currentArrow;
    private float bonusForce;
    private float bonusDemage;
    private float bonusHealth;
    private List<Arrow> arrowPool;
    private bool isJumping;
    private bool isCooldown;
    private bool isBlockMovemant;
    private UICharacterControler controller;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        arrowPool = new List<Arrow>();
        for(int i =0; i < arrowsCount; i++)
        {
            var arrowTemp = Instantiate(arrow, arrowSpawnPoint);
            arrowPool.Add(arrowTemp);
            arrowTemp.gameObject.SetActive(false);
        }

        health.OnTakeHit += TakeHit;

        buffReciever.OnBuffsChanged += ApplyBuffs;
           
    }

    public void InitUIController(UICharacterControler uiControler)
    {
        controller = uiControler;
        controller.Jump.onClick.AddListener(Jump);
        controller.Fire.onClick.AddListener(CheckShoot);
    }

    #region Singleton
    public static Player Instance { get; set; }
    #endregion

    private void ApplyBuffs()
    {
        var forceBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Force);
        var demageBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Demage);
        var armorBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Armor);
        bonusForce = forceBuff == null ? 0 : forceBuff.additiveBonus;
        bonusHealth = armorBuff == null ? 0 : armorBuff.additiveBonus;
        health.SetHealth((int)bonusHealth);
        bonusDemage = demageBuff == null ? 0 : demageBuff.additiveBonus;
    }

    private void TakeHit(int demage, GameObject attacker)
    {
        animator.SetBool("GetDemage", true);
        animator.SetTrigger("TakeHit");
        isBlockMovemant = true;
        rigidbody.AddForce(transform.position.x < attacker.transform.position.x ?
        new Vector2(-demageForce, 0) : new Vector2(demageForce, 0), ForceMode2D.Impulse);
        SoundManager.PlaySound("playerHit");
    }

    public void UnblockMovement()
    {
         isBlockMovemant = false;
         animator.SetBool("GetDemage", false);
    }

    void FixedUpdate()
    {
        Move();
        animator.SetFloat("Speed", Mathf.Abs(rigidbody.velocity.x));
        CheckFall();

    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.OnClickPause();
        
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
#endif
    }

    private void Move()
    {
       animator.SetBool("isGrounded", groundDetection.isGrounded);
       if (!isJumping && !groundDetection.isGrounded)
           animator.SetTrigger("StartFall");

       isJumping = isJumping && !groundDetection.isGrounded;

       direction = Vector3.zero;

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
            direction = Vector3.left;

        if (Input.GetKey(KeyCode.D))
            direction = Vector3.right;
#endif

        if (controller.Left.IsPressed)
           direction = Vector3.left;

        if (controller.Right.IsPressed)
           direction = Vector3.right;

       direction *= speed;
       direction.y = rigidbody.velocity.y;
       if (!isBlockMovemant)

       rigidbody.velocity = direction;

       if (direction.x > 0)
           spriteRenderer.flipX = false;
       if (direction.x < 0)
           spriteRenderer.flipX = true;
    }

    private void Jump()
    {
       if (groundDetection.isGrounded)
       {
            rigidbody.AddForce(Vector2.up * (force + bonusForce), ForceMode2D.Impulse);
            animator.SetTrigger("StartJump");
            isJumping = true;
            SoundManager.PlaySound("jump");
        }
    }


    private void CheckShoot()
    {
       if (!isCooldown)
       {
          animator.SetTrigger("StartShoot");               
       }
    }

    public void InitArrow()
    {
        currentArrow = GetArrowFromPool();
        currentArrow.SetImpulse(Vector2.right, spriteRenderer.flipX ?
               -force * shootForce : force * shootForce, (int) bonusDemage, this); ;

        shootSource.Play(0);

        StartCoroutine(Cooldown());
    }

        

    private IEnumerator Cooldown()
    {
       isCooldown = true;
       yield return new WaitForSeconds(cooldown);
       isCooldown = false;
    }

    private Arrow GetArrowFromPool()
    {
       if(arrowPool.Count > 0)
       {
           var arrowTemp = arrowPool[0];
           arrowPool.Remove(arrowTemp);
           arrowTemp.gameObject.SetActive(true);
           arrowTemp.transform.parent = null;
           arrowTemp.transform.position = arrowSpawnPoint.transform.position;
           return arrowTemp;
       }
       return Instantiate
       (arrow, arrowSpawnPoint.position, Quaternion.identity);
    }

    public void ReturnArrowToPool(Arrow arrowTemp)
    {
        if (!arrowPool.Contains(arrowTemp))
            arrowPool.Add(arrowTemp);

        arrowTemp.transform.parent = arrowSpawnPoint;
        arrowTemp.transform.position = arrow.transform.position;
        arrowTemp.gameObject.SetActive(false);

    }

    void CheckFall()
    {
        if (transform.position.y < minimalHeight && isCheatMode)
        {
            rigidbody.velocity = new Vector2(0, 0);
            transform.position = new Vector2(0, 0);
            SoundManager.PlaySound("playerFall");
        }

        else if (transform.position.y < minimalHeight)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        playerCamera.transform.parent = null;
        playerCamera.enabled = true;
    }

 }



       
    
