using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] GameObject target;
    Drive targetDriver;
    [SerializeField] float speed = 5;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target)
            targetDriver = target.GetComponent<Drive>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

        if((toTarget > 90 && relativeHeading < 20) || targetDriver.currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + targetDriver.currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead * speed);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + targetDriver.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead * speed);
    }

    void Update()
    {
        if (target != null)
            Pursue();
    }
}
