using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector3		front;
	private Vector3		side;
	private Vector3		ipos;
	private float		moveX = 0;
	private float		moveZ = 0;
	private bool		isJumping = false;
	private Transform	cam;
	private	float		viewX = 0;
	private	float		viewY = 0;
	private Vector3		up = new Vector3(0, 1, 0);
	
	public float	speed = 3;
	public float	vSpeed = 4.43f;
	public float	cSpeed = 10000;

    // Start is called before the first frame update
    void Start()
    {
		this.cam = Camera.main.transform;
		this.ipos = this.transform.position;
		Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
		this.cam.LookAt(this.transform.position, this.up);
		this.front = new Vector3(cam.forward.x, 0, cam.forward.z);
		this.side = new Vector3(cam.forward.z, 0, -cam.forward.x);
		this.moveX = this.speed * Time.deltaTime * Input.GetAxis("Horizontal");
		this.moveZ = this.speed * Time.deltaTime * Input.GetAxis("Vertical");
		this.viewX = this.cSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
		this.viewY = this.cSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");

		if (!this.isJumping && Input.GetAxis("Jump") != 0)
		{
			if (this.GetComponent<Rigidbody>())
			{
				this.isJumping = true;
				this.GetComponent<Rigidbody>().velocity = new Vector3(0, this.vSpeed, 0);
			}
		}
		this.cam.Translate(this.transform.position - this.ipos, Space.World);
		this.ipos = this.transform.position;
		this.transform.Translate(this.side * this.moveX + this.front * this.moveZ, Space.World);
		this.transform.Rotate(this.side, 250 * this.moveZ, Space.World);
		this.transform.Rotate(this.front, -250 * this.moveX, Space.World);
		this.cam.RotateAround(this.transform.position, this.up, this.viewX);
		this.cam.RotateAround(this.transform.position, this.side, -this.viewY);
    }

	void	OnCollisionEnter(Collision other)
	{
		this.isJumping = false;
		if (other.gameObject.name == "Floor")
		{
			Debug.Log("Game Over");
			GameObject.Destroy(this.gameObject);
		}
	}
}
