using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

public class GrenadeScript : MonoBehaviour {

    Vector3 mousePos;
    Vector3 wantedPos;
    Ray mouseRay;
    private bool grenadeIsHold;
    Transform standardTransform;
    float depth = 0.8f;
    private GameObject grenade;

    Vector3 startingMousePos;
    Vector3 endingMousePos;
    float movementTime;

    private float length = 0;
    private bool SW = false;
    private Vector3 final;
    private Vector3 startpos;
    private Vector3 endpos;
    public float explosionRadius;
    private Rigidbody grenadeRigidBody;
    public ParticleSystem particleSystem;
    public AudioSource explosionAudio;

    public GameObject comboView;

    void Awake()
    {

    }

    void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releasedHandler;
        GetComponent<FlickGesture>().Flicked += flickedHandler;
        GetComponent<FlickGesture>().Cancelled += flickCancelledHandler;
        GetComponent<FlickGesture>().StateChanged += stateChangedHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releasedHandler;
        GetComponent<FlickGesture>().Flicked -= flickedHandler;
    }

    private void flickCancelledHandler(object sender, System.EventArgs e)
    {
       // Debug.Log("Flick cancelled");
    }
    private void stateChangedHandler(object sender, System.EventArgs e)
    {
       // Debug.Log("Flick state changed");
    }

    private void releasedHandler(object sender, System.EventArgs e)
    {
       // Debug.Log("Released");
    }

    private void pressedHandler(object sender, System.EventArgs e)
    {
        grenadeIsHold = true;
      //  Debug.Log("Pressed ");
    }

    IEnumerator DeleteParticleAfterShowing(ParticleSystem particle)
    {

        yield return new WaitForSeconds(particle.duration); //this will wait 5 seconds
        Destroy(particle);

    }



    private void flickedHandler(object sender, System.EventArgs e)
    {
      //  Debug.Log("Flicked");

        var gesture = sender as FlickGesture;

        //float distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);


        /* Debug.Log("screenstartposition x: " + gesture.PreviousScreenPosition.x);
         Debug.Log("screenstartposition y: " + gesture.PreviousScreenPosition.y);
         Debug.Log("screenendposition x: " + gesture.ScreenPosition.x);
         Debug.Log("flickvector: " + gesture.ScreenFlickVector);
         Debug.Log("screenendposition y: " + gesture.ScreenPosition.y);
         Debug.Log("flicktime: " + gesture.FlickTime);
         Debug.Log("screenflicktime: " + gesture.ScreenFlickTime);
         */
        //x is links/rechts
        //y is höhe
        //z is nach hinten

        grenadeRigidBody.useGravity = true;

        float vertAngle = 18;
        float horAngle = 90 - Mathf.Atan2(gesture.ScreenFlickVector.y, gesture.ScreenFlickVector.x) * Mathf.Rad2Deg;

        float magnitude = 0.7f/ gesture.ScreenFlickTime;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 rotated = Quaternion.Euler(-vertAngle, horAngle/2, 0) * forward;
        Vector3 force = rotated * magnitude;

        Debug.Log("force " + force + " fv " + gesture.ScreenFlickVector + " ha " + horAngle);

        grenadeRigidBody.AddForce(force, ForceMode.Impulse);
    }


    private void transformedHandler(object sender, System.EventArgs e)
    {
        //Debug.Log("object moved");
    }


    // Use this for initialization
    void Start () {
        grenadeIsHold = false;
        standardTransform = this.transform;

        grenadeRigidBody = this.GetComponent<Rigidbody>();
        explosionAudio = this.GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log("oncollision enter called");
        ParticleSystem ps = (ParticleSystem) Instantiate(particleSystem, this.transform.position, particleSystem.transform.rotation);
        ps.Play();
        StartCoroutine(DeleteParticleAfterShowing(ps));
        
        //explosionAudio.Play();
        
        AudioSource.PlayClipAtPoint(explosionAudio.clip, this.transform.position);
  

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, explosionRadius);

        int combo = 0;

        for (int i = 0; i < hitColliders.Length; i++){
            Debug.Log(hitColliders[i].name);
            ZombieMovementScript script = hitColliders[i].GetComponent<ZombieMovementScript>();
            if (script != null)
            {
                combo += 1;
                script.colleteralDamage();
            }
            
            //hitColliders[i].SendMessage("die");
        }

        if (combo > 1)
        {
            GameScript.isCombo = true;
            GameScript.points += combo;
        }
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        Destroy(GameScript.actualGrenade);

    }


    

    // Update is called once per frame
    void Update() {

        if(this.transform.position.y < -10)
        {
            Destroy(GameScript.actualGrenade);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) // Throw grenade test 
        {
            Debug.Log("Up wurde gedrückt");
            grenadeRigidBody.useGravity = true;
            grenadeRigidBody.velocity = new Vector3(0, 5, 5);
            grenadeRigidBody.AddForce(transform.forward / 2);
        }

        
    }



}




