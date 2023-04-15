using UnityEngine;

public class Player : MonoBehaviour
{
    public FixedJoystick joystick; // ������ �� ����������� ��������

    public float speed = 5f; // �������� �������� ���������

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // �������� ����������� �������� �� ������������ ���������
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // ������� ��������� � ����������� ������������ ���������
        rb.velocity = direction * speed;
    }
}
