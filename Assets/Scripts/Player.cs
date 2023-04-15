using UnityEngine;

public class Player : MonoBehaviour
{
    public FixedJoystick joystick; // Ссылка на виртуальный джойстик

    public float speed = 5f; // Скорость движения персонажа

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Получаем направление движения от виртуального джойстика
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // Двигаем персонажа в направлении виртуального джойстика
        rb.velocity = direction * speed;
    }
}
