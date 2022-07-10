using UnityEngine;
using UnityEngine.Events;

public class player_controller : MonoBehaviour
{
	public float special_mult = 1.5f;
	public player_adventure _Adventure;
	bool special_speed=false;
	public int id = 0;
	[SerializeField] private float m_JumpForce = 400f;                          // sila skoku
	[Range(0, 10)] [SerializeField] private float m_CrouchSpeed = .36f;          // modyfikator szybkosci przy skakaniu 
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // wygladzanie ruchu 
	[SerializeField] private bool m_AirControl = false;                         // czy mo¿na sterowac w powietrzu
	[SerializeField] private LayerMask m_WhatIsGround;                          // definicja tego co jest gruntem
	[SerializeField] private Transform m_GroundCheck;                           // obiekt sprawdzaj¹cy czy postaæ stoi na ziemi
	[SerializeField] private Transform m_CeilingCheck;                          // obiekt sprawdzaj¹cy obecnoœæ sufitu
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // kolizja wy³¹czana przy kucaniu
	
	const float k_GroundedRadius = .2f; //Promieñ testu uziemienia
	public bool m_Grounded;            // czy postaæ stoi na ziemi
	const float k_CeilingRadius = .05f; // promieñ sprawdzaj¹cy czy postaæ mo¿e wróciæ do pyzcji wyprostowanej
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true; 
	private Vector3 m_Velocity = Vector3.zero;

	public bool is_Crouching = false;
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}
    private void Update()
    {
		if (_Adventure.power_selected == 3)
		{
			special_speed = true;
		}
		else special_speed = false;
    }
    private void FixedUpdate()
	{
		
		bool wasGrounded = m_Grounded;
		m_Grounded = false;


		//sprawdzanie, czy w promieniu od m_GroundCheck jest obiekt uznawany za grunt
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// sprawdzanie czy postaæ mo¿e wstac
		if (!crouch && m_wasCrouching)
		{
			// Jezeli kolizja od gory uniemozliwia wstanie to postac dalej kuca
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)!=null)
			{
				
				//Debug.Log(Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround));
				crouch = true;
			}
		}

		//test czy ma byc wykonany ruch w poziomie
		if (m_Grounded || m_AirControl)
		{

			//Nie wystarczy ze postac chce kucac, musi tez byc na ziemi
			if (crouch && m_Grounded)
			{
				//event w razie gdyby teraz kucnela
				if (!m_wasCrouching)
				{

					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// inna predkosc poruszania dla kucania
				move *= m_CrouchSpeed;

				// wylaczenie jednego z colliderow
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// ponowne wlaczenie collidera
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{

					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}


			//obliczenie docelowej predkosci
			float mult = 1f;
			if (special_speed == true) mult = special_mult;
			Vector3 targetVelocity = new Vector2(move * 10f * mult, m_Rigidbody2D.velocity.y);
			// nadanie predkosci docelowej z wygladzaniem
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// obrocenie postaci jesli 
			if (move > 0 && !m_FacingRight)
			{ 
				Flip();
			}
			
			else if (move < 0 && m_FacingRight)
			{
				
				Flip();
			}
			is_Crouching = crouch;
		}
		// skakanie
		if (m_Grounded && jump)
		{
			//Nadanie sily skierowanej w gore
			// Gdy postac kuca to skok jest silniejszy
			float mult = 1f;
			if (special_speed == true) mult = special_mult;
			m_Grounded = false;
			if(crouch)
            {
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce*1.25f*mult));
			}
			else
            {
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce*mult));
			}
			
		}
	}


	public void Flip()
	{
		//Obrocenie postaci
		m_FacingRight = !m_FacingRight;

		transform.Rotate(0f, 180f, 0f);
	}
	
}
