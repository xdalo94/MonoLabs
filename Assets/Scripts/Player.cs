using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

//Indice de funciones
//-Jump 1 y 2
//-DownFast
//-BackInicialPos 1 y 2

public class Player : MonoBehaviour
{
    readonly int NumberOfJumps = 2; //N�mero m�ximo de saltos

    public int JumpNo = 0; //Indica cuantos saltos haz realizado, se reinicia al tocar el suelo o la cabeza de un enemido (lo ultimo aun no se ha implementado)
    public float JumpForce;
    public float DownForce;
    public bool IsGoingDownFast;

    public Animator animator; //variable que se utiliza para cambiar el estado de la animacion
    private float ySpeed; //variable que se utiliza mientras para saber si el mono asqueroso esta saltando o cayendo


    private float InicialPosX = -2f; //Posici�n del mono por defecto en X
    public float backSpeed=1f; //Velocidad a la que vuelve el mono a su posici�n inicial en X

    //private bool IsOnGround;

    public Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        IsGoingDownFast = false;

        ySpeed = 0; //solo declaro para que no est� vac�o
    }

    // Update is called once per frame
    void Update()
    {
        Jump2();
        DownFast();
        BackInicialPos();


        //OnTheGround
        #region OnTheGround
        ySpeed = RB.velocity.y; //obtengo velocidad en y
        if (ySpeed != 0)
            animator.SetBool("OnTheGround", true); //si la velocidad en y es distinta de 0 entonces se esta moviendo en Y, por tanto saltando
        else
            animator.SetBool("OnTheGround", false); //se asume que est� en tierra o alcanz� el pinaculo mas alto en un instante
        #endregion
    }

    //Esta funci�n se encarga de informar si "Player" a tocado suelo y cambiar las variables asociadas
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            JumpNo = 0;
            IsGoingDownFast = false;
            //IsOnGround = true;
        }
    }

    //Con este salto al apretar dos veces r�pido el salto es m�s alto
    //Y al apretar dos veces lente el salto es m�s controlado
    private void Jump1()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && (JumpNo < NumberOfJumps))
        {
            JumpNo++;
            RB.AddForce(new Vector2(0.0f, JumpForce), ForceMode2D.Impulse);
            //IsOnGround = false;
        }
    }

    //Con este salto el cambio de la velocidad es inmediato, 
    //esto asegura que el primer y segundo salto sean de la misma fuerza
    private void Jump2()
    {
        //Condici�n para saltar: Apretar tecla, no haber superado el n�mero m�ximo de saltos
        if (Input.GetKeyDown(KeyCode.UpArrow) && (JumpNo < NumberOfJumps))
        {
            JumpNo++;
            RB.velocity = new Vector2(0.0f, JumpForce);
            //IsOnGround = false;
        }
    }

    private void DownFast()
    {
        
        //Condicion para bajar rapido: apretar tecla, estar en el aire, no estar bajando rapido
        if (Input.GetKeyDown(KeyCode.DownArrow) && (JumpNo > 0) && !IsGoingDownFast)
        {
            IsGoingDownFast = true;
            RB.velocity = new Vector2(0.0f, -DownForce);
        }
    }

    private void BackInicialPos()
    {
        if ((transform.position.x < (InicialPosX-0.05)) && (JumpNo == 0))
        {
            transform.position = new Vector2(transform.position.x + backSpeed * Time.deltaTime, transform.position.y);
        }
        else if ((transform.position.x > (InicialPosX+0.05)) && (JumpNo == 0))
        {
            transform.position = new Vector2(transform.position.x - backSpeed * Time.deltaTime, transform.position.y);
        }

    }

    private void BackInicialPos2()
    {
        if ((transform.position.x < InicialPosX) && (JumpNo == 0))
        {
            RB.velocity = new Vector2(backSpeed, RB.velocity.y);
        }
    }
}