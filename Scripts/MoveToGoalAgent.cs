using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private GameObject target;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-4f, 1f), 1.5f, Random.Range(-4f, 4f));
        target.transform.localPosition = new Vector3(Random.Range(1f, 4f), 1.5f, Random.Range(-4f, 4f));
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float moveX = vectorAction[0];
        float moveZ = vectorAction[1];

        float moveSpeed = 2.5f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.transform.localPosition);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxisRaw("Horizontal");
        actionsOut[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            floorRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorRenderer.material = loseMaterial;
            EndEpisode();
        }
    }
}
