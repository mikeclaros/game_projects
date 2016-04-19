using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {
	//player properties
	public float Speed = 10f;
	public float JumpSpeed = 1.0f;
	public LayerMask GroundLayers;
	private float m_cameraZ = -10f;
	//camera properties
	public float frac = 0.5f; // range from 0 to 1

    private GameObject g_HoldWall;
    private GameObject g_backWall;
	private GameObject g_cameraHold;
	private GameObject g_MainCamera;

	private Animator a_Animator;


	private Transform t_GroundCheck;
    private Transform t_LeftEdgeCheck;
    private Transform t_RightEdgeCheck;
    private Transform t_backWall;

	private bool isDropped;
	//private bool isShooting = false;
	
	
	void Start () {
		a_Animator = GetComponent<Animator>();

        g_HoldWall = GameObject.Find("HoldWall");
        g_backWall = GameObject.Find("backWall");
		g_cameraHold = GameObject.Find ("HoldCamera");
		g_MainCamera = GameObject.Find("Main Camera");

		t_GroundCheck = transform.FindChild("groundCheck");
        t_LeftEdgeCheck = transform.FindChild("L_edgeCheck");
        t_RightEdgeCheck = transform.FindChild("R_edgeCheck");
        t_backWall = g_HoldWall.transform.FindChild("backWall");

       
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Handle ground check
        // jumping handled in groundcheck
        groundCheck();

       
        ////////////////////////////////////
        //
        //  Player movements


		float hSpeed = Input.GetAxis ("Horizontal");
		a_Animator.SetFloat ("Speed", Mathf.Abs (hSpeed));

		if( hSpeed < 0){ //Moving Left
            dropWall();
			dropCamera();
			this.transform.localScale = new Vector3(-1,1,1);
        }
		if( hSpeed > 0){//Moving Right
			if(isDropped){
				grabWall();
				grabCamera();
				isDropped = false;
			}
			this.transform.localScale = new Vector3(1,1,1);
			t_backWall.transform.position = new Vector2(this.transform.position.x - 2.64f, 0 );
        }

		this.GetComponent<Rigidbody2D>().velocity = new Vector2(hSpeed * Speed, this.GetComponent<Rigidbody2D>().velocity.y);
        

		// following code trys for a double tap for running
        //float ButtonCooler = 0.5f;
        //int ButtonCount = 0;

        //if(Input.GetAxis("Horizontal")){
        //    if(ButtonCooler > 0 && ButtonCount ==1){
        //        //this button has been double tapped
        //        float hSpeed = Input.GetAxis ("Horizontal");
        //        a_Animator.SetFloat ("Speed", Mathf.Abs (hSpeed));
        //        this.transform.localScale = new Vector3(-1,1,1);
        //    }
        //    else{
        //        ButtonCooler = 0.5f;
        //        ButtonCount += 1;
        //    }
        //}

        //if(ButtonCooler > 0){
        //    ButtonCooler -= 1 * Time.deltaTime;
        //}
        //else{
        //    ButtonCount = 0;
        //}
		

        
        //  
        ////////////////////////////////////





         
	}


    private void dropWall() {
        g_HoldWall.transform.DetachChildren();
		isDropped = true;
    }

	private void grabWall() {
		g_backWall.transform.SetParent(g_HoldWall.transform);
	}

	private void dropCamera(){
		g_cameraHold.transform.DetachChildren();
		float backWallPos = g_backWall.transform.position.x; //left wall location
		//g_MainCamera.transform.position = new Vector3(backWallPos + 3.8f,0,m_cameraZ);
        
        // camera offset relative to player

        g_MainCamera.transform.position = new Vector3(this.transform.position.x, 0, m_cameraZ);

	}

	private void grabCamera(){
		// gradually move camera
		// should grab camera once the player is positioned near center of view
		float startPos = g_MainCamera.transform.position.x;
		float endPos = this.transform.position.x;
		Vector3 startMarker = new Vector3(startPos,0,m_cameraZ);
		Vector3 endMarker = new Vector3(endPos,0,m_cameraZ);
//		g_MainCamera.transform.position = Vector3.Lerp(startMarker,endMarker,frac);
		g_MainCamera.transform.position = new Vector3(endPos,0,m_cameraZ);
		g_MainCamera.transform.SetParent(g_cameraHold.transform);
	}



	
	private void groundCheck(){
        bool isGrounded = Physics2D.OverlapPoint(t_GroundCheck.position, GroundLayers);
        bool isAtEdge_Left = Physics2D.OverlapPoint(t_LeftEdgeCheck.position, GroundLayers);
        bool isAtEdge_Right = Physics2D.OverlapPoint(t_RightEdgeCheck.position, GroundLayers);

        if (Input.GetButton("Jump")){
            if (isGrounded){
                this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpSpeed * 1/2, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }

       

        if (isAtEdge_Left){
            a_Animator.SetBool("isGrounded", isAtEdge_Left);
        }
        if (isAtEdge_Right){
            a_Animator.SetBool("isGrounded", isAtEdge_Right);
        }
         a_Animator.SetBool("isGrounded", isGrounded);
    }

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Respawn")
			this.transform.position = new Vector2(-3.35f, 0.19f);
		if(coll.gameObject.tag == "Enemy")
			this.transform.position = new Vector2(-3.35f, 0.19f);
	}
}
