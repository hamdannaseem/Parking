using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PedAIController : MonoBehaviour
{
    public NavMeshAgent Ped;
    public Transform FirstVertex;
    Transform CurrentVertex, Target;
    float TargetDistance;
    bool Hit = false;
    void Start()
    {
        CurrentVertex = FirstVertex;
        Target = CurrentVertex;
        Ped.SetDestination(Target.position);
    }
    void Update()
    {
        TargetDistance = Vector3.Distance(transform.position, Target.position);
        if (TargetDistance < 1 & !Hit)
        {
            PickNextTarget();
            Ped.SetDestination(Target.position);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            GetComponent<Animator>().SetBool("Hit", true);
            Ped.SetDestination(transform.position);
            Hit = true;
            GameManager.Hit=true;
        }
    }
    void PickNextTarget()
    {
        List<Transform> Edges = CurrentVertex.GetComponent<EdgeList>().Edges;
        if (Edges.Count > 0)
        {
            Transform NextVertex = Edges[Random.Range(0, Edges.Count)];
            Target = NextVertex;
            CurrentVertex = NextVertex;
        }
    }
}
