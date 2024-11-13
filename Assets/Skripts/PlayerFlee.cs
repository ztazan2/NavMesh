using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFlee : MonoBehaviour
{
    public Transform enemy; // �� ��ġ ����
    private NavMeshAgent agent;
    public float fleeDistance = 20f; // ����ĥ �ּ� �Ÿ�
    public float safeDistance = 30f; // ���� �Ÿ�
    private Vector3 navMeshLinkStart; // NavMesh Link�� ���� ����
    public float endPointThreshold = 5f; // ���� ���� �Ÿ� (���� �� ũ�� ����)

    private bool isFleeing = false; // ���� ���� �÷���
    private bool reachedEnd = false; // �� ���� ���� ���� Ȯ�� �÷���
    public static bool gameEnded = false; // ���� ���� ���� Ȯ�� �÷��� (���� �÷��׷� ����)

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // NavMeshLink ������Ʈ�� ã�� ���� ������ ����
        NavMeshLink navMeshLink = FindObjectOfType<NavMeshLink>();
        if (navMeshLink != null)
        {
            // ���� ���� ���
            navMeshLinkStart = navMeshLink.transform.position + (navMeshLink.transform.rotation * Vector3.right * navMeshLink.width * 0.5f);
        }
        else
        {
            Debug.LogWarning("NavMeshLink�� ã�� �� �����ϴ�. NavMeshLink�� �����ϴ��� Ȯ���Ͻʽÿ�.");
        }
    }

    private void Update()
    {
        // ������ �̹� ����� ���¶�� �� �̻� �������� ����
        if (gameEnded)
        {
            if (isFleeing)
            {
                Debug.Log("���� ����"); // ���� ���� �޽��� ���
                isFleeing = false; // ���� ���� ����
                agent.ResetPath(); // �̵� ����
            }
            return;
        }

        // �÷��̾�� �� ������ �Ÿ� ���
        float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);

        // ���� ����: ���� �Ÿ� �̳��� ���� �ִ� ���
        if (distanceToEnemy < safeDistance && !isFleeing)
        {
            // NavMesh Link�� ���� �������� ���� ����
            agent.SetDestination(navMeshLinkStart);
            isFleeing = true;
            Debug.Log("NavMesh Link ���� �������� ���� ��");
        }

        // NavMesh Link�� ���� ������ �����ߴ��� Ȯ��
        if (isFleeing && !reachedEnd)
        {
            float distanceToStart = Vector3.Distance(transform.position, navMeshLinkStart);

            // NavMesh Link�� ���� ������ �����ϸ� ���� ����
            if (distanceToStart < endPointThreshold)
            {
                Debug.Log("���� ����");
                reachedEnd = true;
                gameEnded = true; // ���� ���� ���� ����
                agent.ResetPath(); // �̵� ����
            }
        }
    }
}
