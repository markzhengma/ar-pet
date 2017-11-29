using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharMovement : MonoBehaviour 
{

	public float jumpSpeed = 600.0f;
	public bool grounded = false;
	public bool doubleJump = false;
	public Transform groundCheck;
	public float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	private Animator anim;
	public Rigidbody rb;
	public float vSpeed;

	public Button jumpBtn;
	public Button walkBtn;
	public Button idleBtn;
	public Button runBtn;
	public Button danceBtn;

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
		anim.SetBool("isIdle", true);
	}
	void Start ()
	{
		Button buttonOne = jumpBtn.GetComponent<Button> ();
		buttonOne.onClick.AddListener(Jump);
		Button buttonTwo = walkBtn.GetComponent<Button> ();
		buttonTwo.onClick.AddListener(CrippledWalk);
		Button buttonThree = idleBtn.GetComponent<Button> ();
		buttonThree.onClick.AddListener(Idle);
		Button buttonFour = runBtn.GetComponent<Button> ();
		buttonFour.onClick.AddListener(Run);
		Button buttonFive = danceBtn.GetComponent<Button> ();
		buttonFive.onClick.AddListener(Dance);
	}
	void FixedUpdate () 
	{
		grounded = Physics.CheckSphere(groundCheck.position, groundRadius, whatIsGround);
		vSpeed = rb.velocity.y;
        anim.SetFloat ("vSpeed", vSpeed);
	}
	void Update () 
	{
		if (Input.GetKeyDown("space") && anim.GetBool("isIdle"))
		{
			Jump();
		}
		// if (Input.GetKeyDown("return") && anim.GetBool("isIdle"))
		// {
		// 	Dance();
		// }
	}

	public void Jump ()
	{
		Debug.Log("jump!");
		// if (grounded && rb.velocity.y == 0)
		if (rb.velocity.y == 0)
		{
			anim.SetTrigger("isJump");
            rb.AddForce(0,jumpSpeed,0, ForceMode.Impulse);
		}
	}

	public void CrippledWalk ()
	{
		anim.SetBool("crippled", !(anim.GetBool("crippled")));
		anim.SetBool("isIdle", false);
		Debug.Log("crippled");
		// GetComponent<Animator>().Play("crippledWalk");
	}

	public void Idle ()
	{
		anim.SetBool("isIdle", true);
		anim.SetBool("isRun", false);
		anim.SetBool("crippled", false);
		anim.SetBool("dancing", false);
	}

	public void Run ()
	{
		anim.SetBool("isRun",!(anim.GetBool("isRun")));
		anim.SetBool("isIdle", false);

	}

	public void Dance()
	{
		anim.SetBool ("dancing", !(anim.GetBool("dancing")));
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
	}

}
