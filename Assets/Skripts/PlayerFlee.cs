using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFlee : MonoBehaviour
{
    public Transform enemy; // 적 위치 참조
    private NavMeshAgent agent;
    public float fleeDistance = 20f; // 도망칠 최소 거리
    public float safeDistance = 30f; // 안전 거리
    private Vector3 navMeshLinkStart; // NavMesh Link의 시작 지점
    public float endPointThreshold = 5f; // 도망 성공 거리 (조금 더 크게 설정)

    private bool isFleeing = false; // 도망 여부 플래그
    private bool reachedEnd = false; // 끝 지점 도달 여부 확인 플래그
    public static bool gameEnded = false; // 게임 종료 여부 확인 플래그 (정적 플래그로 설정)

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // NavMeshLink 오브젝트를 찾고 시작 지점을 설정
        NavMeshLink navMeshLink = FindObjectOfType<NavMeshLink>();
        if (navMeshLink != null)
        {
            // 시작 지점 계산
            navMeshLinkStart = navMeshLink.transform.position + (navMeshLink.transform.rotation * Vector3.right * navMeshLink.width * 0.5f);
        }
        else
        {
            Debug.LogWarning("NavMeshLink를 찾을 수 없습니다. NavMeshLink가 존재하는지 확인하십시오.");
        }
    }

    private void Update()
    {
        // 게임이 이미 종료된 상태라면 더 이상 실행하지 않음
        if (gameEnded)
        {
            if (isFleeing)
            {
                Debug.Log("도망 실패"); // 도망 실패 메시지 출력
                isFleeing = false; // 도망 상태 종료
                agent.ResetPath(); // 이동 정지
            }
            return;
        }

        // 플레이어와 적 사이의 거리 계산
        float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);

        // 도망 로직: 안전 거리 이내에 적이 있는 경우
        if (distanceToEnemy < safeDistance && !isFleeing)
        {
            // NavMesh Link의 시작 지점으로 도망 설정
            agent.SetDestination(navMeshLinkStart);
            isFleeing = true;
            Debug.Log("NavMesh Link 시작 지점으로 도망 중");
        }

        // NavMesh Link의 시작 지점에 도달했는지 확인
        if (isFleeing && !reachedEnd)
        {
            float distanceToStart = Vector3.Distance(transform.position, navMeshLinkStart);

            // NavMesh Link의 시작 지점에 도달하면 도망 성공
            if (distanceToStart < endPointThreshold)
            {
                Debug.Log("도망 성공");
                reachedEnd = true;
                gameEnded = true; // 게임 종료 상태 설정
                agent.ResetPath(); // 이동 정지
            }
        }
    }
}
