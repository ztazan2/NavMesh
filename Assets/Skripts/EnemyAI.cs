using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // 플레이어 위치 참조
    private NavMeshAgent agent;
    public float stoppingDistance = 1.0f; // 도착으로 간주할 거리
    private bool hasReachedDestination = false; // 도착 여부 확인 플래그
    private bool isChasing = false; // 추적 상태 플래그

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; // NavMeshAgent의 stoppingDistance 설정
    }

    private void Update()
    {
        // 게임이 종료된 상태라면 추적을 멈추고 "추적 실패" 메시지를 한 번만 출력
        if (PlayerFlee.gameEnded)
        {
            if (!hasReachedDestination)
            {
                Debug.Log("추적 실패");
                hasReachedDestination = true; // "추적 실패" 메시지를 한 번만 출력
                agent.ResetPath(); // 이동 정지
            }
            return;
        }

        // 목표에 아직 도달하지 않은 경우에만 SetDestination 호출
        if (!hasReachedDestination)
        {
            agent.SetDestination(player.position);

            // "추적 중" 메시지를 한 번만 출력
            if (!isChasing)
            {
                Debug.Log("추적 중");
                isChasing = true;
            }

            // 목표 지점과의 거리가 일정 이하일 때 도착으로 간주
            if (agent.remainingDistance <= stoppingDistance && !agent.pathPending)
            {
                Debug.Log("추적 성공");
                hasReachedDestination = true;
                isChasing = false; // 추적 중 상태 초기화

                // 게임 종료 상태 설정 (플레이어에게 도망 실패 알림)
                PlayerFlee.gameEnded = true;

                // 이동 정지
                agent.ResetPath(); // 현재 경로 초기화
            }
        }
    }
}
