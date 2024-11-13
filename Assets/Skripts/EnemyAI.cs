using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ ����
    private NavMeshAgent agent;
    public float stoppingDistance = 1.0f; // �������� ������ �Ÿ�
    private bool hasReachedDestination = false; // ���� ���� Ȯ�� �÷���
    private bool isChasing = false; // ���� ���� �÷���

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; // NavMeshAgent�� stoppingDistance ����
    }

    private void Update()
    {
        // ������ ����� ���¶�� ������ ���߰� "���� ����" �޽����� �� ���� ���
        if (PlayerFlee.gameEnded)
        {
            if (!hasReachedDestination)
            {
                Debug.Log("���� ����");
                hasReachedDestination = true; // "���� ����" �޽����� �� ���� ���
                agent.ResetPath(); // �̵� ����
            }
            return;
        }

        // ��ǥ�� ���� �������� ���� ��쿡�� SetDestination ȣ��
        if (!hasReachedDestination)
        {
            agent.SetDestination(player.position);

            // "���� ��" �޽����� �� ���� ���
            if (!isChasing)
            {
                Debug.Log("���� ��");
                isChasing = true;
            }

            // ��ǥ �������� �Ÿ��� ���� ������ �� �������� ����
            if (agent.remainingDistance <= stoppingDistance && !agent.pathPending)
            {
                Debug.Log("���� ����");
                hasReachedDestination = true;
                isChasing = false; // ���� �� ���� �ʱ�ȭ

                // ���� ���� ���� ���� (�÷��̾�� ���� ���� �˸�)
                PlayerFlee.gameEnded = true;

                // �̵� ����
                agent.ResetPath(); // ���� ��� �ʱ�ȭ
            }
        }
    }
}
