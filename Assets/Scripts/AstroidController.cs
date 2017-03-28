using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidController : MonoBehaviour {

    public float maxStretch;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;
    public Transform farRight;
    public AudioSource flingSoundEffect;
    public AudioSource bumpSoundEffect;

    private SpringJoint2D springJoint2D;
    private Transform catapult;
    private Rigidbody2D rigidBody2D;
    private bool clicked;
    private bool launched;
    private float maxStretchSquare;
    private Ray mouseRay;
    private Vector2 prevSpeed;

    private CircleCollider2D circle;
    private Ray fromFrontCatapult;
    private float circleRadius;

    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        LineSetup();

        originalPosition = transform.position;

        springJoint2D = this.gameObject.GetComponent<SpringJoint2D>(); // Should be in void Awake() ?
        catapult = springJoint2D.connectedBody.transform;
        rigidBody2D = this.gameObject.GetComponent<Rigidbody2D>();
        rigidBody2D.isKinematic = true;

        clicked = false;
        launched = false;
        maxStretchSquare = maxStretch * maxStretch;
        mouseRay = new Ray(catapultLineBack.transform.position, Vector3.zero);

        fromFrontCatapult = new Ray(catapultLineFront.transform.position, Vector3.zero);

        circle = this.gameObject.GetComponent<CircleCollider2D>();
        circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {
		if (clicked && !launched)
        {
            Drag();
        }

        if (springJoint2D.enabled)
        {
            if (!rigidBody2D.isKinematic && (prevSpeed.sqrMagnitude > rigidBody2D.velocity.sqrMagnitude))
            {
                // Maintain speed for launch. (or else it will slow down even before it has left the spring)
                springJoint2D.enabled = false;
                rigidBody2D.velocity = prevSpeed;
                launched = true;

                flingSoundEffect.Play();
            }
            if (!clicked)
            {
                // Record speed after letting go of astroid.
                prevSpeed = rigidBody2D.velocity;
            }
            if (!launched)
                StringUpdate();
        } else if (!clicked)
        {
            // Launched
            catapultLineFront.enabled = false;
            catapultLineBack.enabled = false;

            if (transform.position.x > farRight.transform.position.x ||
                rigidBody2D.velocity.magnitude < 0.1f)
            {
                Wait();
                this.gameObject.SetActive(false);
            }
        }
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }

    private void LineSetup ()
    {
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Objects";
    }

    private void OnMouseDown ()
    {
        clicked = true;
    }

    private void OnMouseUp ()
    {
        rigidBody2D.isKinematic = false;
        clicked = false;
        springJoint2D.enabled = true;
    }

    private void Drag ()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDistance = mousePosition - catapultLineBack.transform.position;

        if (mouseDistance.magnitude > maxStretch)
        {
            mouseRay.direction = mouseDistance;
            mousePosition = mouseRay.GetPoint(maxStretch);
        }

        mousePosition.z = 0.0f;
        this.gameObject.transform.position = mousePosition;
    }

    private void StringUpdate()
    {
        Vector2 vectorFromFrontCatapult = this.gameObject.transform.position - catapultLineFront.transform.position;
        fromFrontCatapult.direction = vectorFromFrontCatapult;
        Vector3 holdPoint = fromFrontCatapult.GetPoint(vectorFromFrontCatapult.magnitude + circleRadius);
        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }

    public void Reset()
    {
        transform.position = originalPosition;
        rigidBody2D.isKinematic = true;
        clicked = false;
        launched = false;
        springJoint2D.enabled = true;
        catapultLineFront.enabled = true;
        catapultLineBack.enabled = true;
        if (rigidBody2D != null)
        {
            rigidBody2D.velocity = Vector3.zero;
            rigidBody2D.angularVelocity = 0f;
        }
        gameObject.SetActive(true);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        bumpSoundEffect.Play();
    }
}
