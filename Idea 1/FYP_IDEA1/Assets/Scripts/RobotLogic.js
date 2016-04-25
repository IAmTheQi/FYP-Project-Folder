#pragma strict

var actions : String[];
var indexAction : int;
var statusGUI : GUIText;
var t : float = 0;
var speed : float = 1.0;
private var robotController : CharacterController;
private var jumpSpeed : float = 3;
private var flagIdle : boolean = false;
private var moveDirection : Vector3 = Vector3.zero;
private var boostIncr = 1;
private var canAnimate : boolean;
private var gravity = 15;
private var nextLoad : float = 0;
private var rate : float = 5;
private var run : boolean = false;
private var rotateY : float;

function Start () {
	indexAction = 0;
	statusGUI.text = actions[indexAction];
	robotController = GetComponent(CharacterController);
	GetComponent.<Animation>().wrapMode = WrapMode.Loop;
	GetComponent.<Animation>()["idleTap"].wrapMode = WrapMode.Once;
}

function Update() {
	if(Input.GetButtonDown("Boost")) {
		run = true;
	}
	else if(Input.GetButtonUp("Boost")) {
		run = false;
	}
	
	if(!Input.GetButtonDown("Action")) {
		canAnimate = true;
		t = GetComponent.<Animation>()[actions[indexAction]].clip.length + Time.time;
	}
	
	if(Input.GetButtonDown("Switch")) {
		indexAction = (indexAction + 1)%actions.length;
		statusGUI.text = actions[indexAction];
	}
	
	if(robotController.isGrounded == true) {
		if(Input.GetAxis("Vertical") > 0.02 && !Input.GetKey("left ctrl")) {
			GetComponent.<Animation>()["walk"].speed = 1;
			if(run) {
				GetComponent.<Animation>().CrossFade("run");
				GetComponent.<Animation>()["run"].speed = 1;
				boostIncr = 3;
			}
			else {
				GetComponent.<Animation>().CrossFade("walk");
				boostIncr = 1;
			}
		}
		else if(Input.GetAxis("Vertical") < -0.02 && !Input.GetKey("left ctrl")) {
			GetComponent.<Animation>()["walk"].speed = -1;
			GetComponent.<Animation>().CrossFade("walk");
		}
		else {
			if(Input.GetButton("Action") && canAnimate) {
				DoAction();
			}
			else {
				IdleAnimation();
				boostIncr = 1;
			}
		}
		
		if(!Input.GetButton("Action")) {
			moveDirection = Vector3(0,0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= boostIncr * speed;
		}
		
		if(Input.GetButtonDown("Jump")) {
			GetComponent.<Animation>().CrossFade("jump");
			GetComponent.<Animation>()["jump"].speed = 2;
			moveDirection.y = jumpSpeed;
		}
	}
	
	if(Input.GetButtonDown("Action") && canAnimate) {
		DoAction();
	}
	
	transform.eulerAngles.y += Input.GetAxis("Horizontal") * 5; // Use keyboard to control character turning
	rotateY = (Input.GetAxis("Mouse X") * 300) * Time.deltaTime; // Use mouse horizontal position to control character turning
	robotController.transform.Rotate(0, rotateY, 0);
	moveDirection.y -= gravity * Time.deltaTime;
	robotController.Move(moveDirection * Time.deltaTime);
}

function IdleAnimation() {
	if(Time.time > nextLoad) {
		if(flagIdle) {
			flagIdle = false;
		}
		else {
			flagIdle = true;
		}

		nextLoad = Time.time + rate;
	}

	if(flagIdle) {
		GetComponent.<Animation>().CrossFade("idle");
	}
	else {
		GetComponent.<Animation>().CrossFade("idleTap");
		if(!GetComponent.<Animation>().IsPlaying("idleTap")) {
			flagIdle = true;
		}
	}
}

function DoAction() {
	GetComponent.<Animation>().CrossFade(actions[indexAction]);
	if(Time.time > t - 0.5 && canAnimate) {
		GetComponent.<Animation>().CrossFade(actions[indexAction]);
		canAnimate = false;
	}
}