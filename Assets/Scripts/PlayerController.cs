using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

	[SerializeField]
	private float thrusterForce = 1000f;

	[Header("Spring settings:")]
	[SerializeField]
	private JointDriveMode jointMode = JointDriveMode.Position;
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;
   
	private PlayerMotor motor;
	private ConfigurableJoint joint;
	private Animator animator;
	private Player player;
    //TODO put into jetpack class
    private float feul;
    private float timeUntilFeul;
    [SerializeField]
    private bool flying;

	void Start ()
	{
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint>();
		animator = GetComponent<Animator>();
		player = GetComponent<Player>();
        feul = 1f;
        timeUntilFeul = 0f;

		SetJointSettings(jointSpring);
	}

    public float getFeul()
    {
        return feul;
    }

	void Update ()
    {
        float _xMov = 0;
        float _zMov = 0;
        float _yRot = 0;
        float _xRot = 0;
        if (!player.paused)
        {
            _xMov = Input.GetAxis("Horizontal");
            _zMov = Input.GetAxis("Vertical");
            _yRot = Input.GetAxisRaw("Mouse X");
            _xRot = Input.GetAxisRaw("Mouse Y");
        }

        Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        Vector3 _thrusterForce = Vector3.zero;
        float _cameraRotationX = _xRot * lookSensitivity;

        animator.SetFloat("ForwardVelocity", _zMov);
		motor.Move(_velocity);motor.Rotate(_rotation);
		motor.RotateCamera(_cameraRotationX);

		if (Input.GetButton ("Jump") && !player.paused && feul > 0)
		{
            if(!flying)
                feul -= Time.deltaTime;
			_thrusterForce = Vector3.up * thrusterForce;
			SetJointSettings(0f);
            timeUntilFeul = 3f;
        }
        else
        {
            SetJointSettings(jointSpring);
        }
		motor.ApplyThruster(_thrusterForce);

        if (timeUntilFeul >= 0)
            timeUntilFeul -= Time.deltaTime;
        else
            feul = 1f;
	}

    private void SetJointSettings (float _jointSpring)
	{
		joint.yDrive = new JointDrive {
			mode = jointMode,
			positionSpring = _jointSpring,
			maximumForce = jointMaxForce
		};
	}

}
