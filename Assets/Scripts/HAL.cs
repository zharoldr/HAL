using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using RosMessageTypes.Pedsim;

public class HAL : MonoBehaviour {

    private ROSConnection ros;

    public string agent_topic = "/pedsim_simulator/simulated_agents";

    public string robot_topic = "/pedsim_simulator/robot_state";

    private GameObject robot;
    private List<GameObject> agents;

    public GameObject robot_model;

    public List<GameObject> agent_models;

    void Start() {
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<AgentStatesMsg>(agent_topic, agent_callback);
        ros.Subscribe<AgentStateMsg>(robot_topic, robot_callback);

        robot = Instantiate(robot_model, Vector3.zero, Quaternion.identity);

        agents = new List<GameObject>();
    }

    void agent_callback(AgentStatesMsg msg) {
        if (agents.Count == 0) {
            for (int i = 0; i < msg.agent_states.Length; i++) {
                
                GameObject new_agent = Instantiate(agent_models[Random.Range(0, agent_models.Count)], Vector3.zero, Quaternion.identity);
                
                agents.Add(new_agent);
            }
        } else {
            for (int i = 0; i < msg.agent_states.Length; i++) {
                Vector3 current_pos = agents[(int)msg.agent_states[i].id].transform.position;
                Vector3 new_pos = new Vector3((float)msg.agent_states[i].pose.position.x, 0.0f, (float)msg.agent_states[i].pose.position.y);
                
                Vector3 face_dir = (new_pos - current_pos).normalized;

                agents[(int)msg.agent_states[i].id].transform.position = new_pos;

                if (face_dir != Vector3.zero) {
                    agents[(int)msg.agent_states[i].id].transform.rotation = Quaternion.Slerp(agents[(int)msg.agent_states[i].id].transform.rotation, Quaternion.LookRotation(face_dir), 15.0f * Time.deltaTime);
                }
            }
        }
    }

    void robot_callback(AgentStateMsg msg) {

        Vector3 current_pos = robot.transform.position;
        Vector3 new_pos = new Vector3((float)msg.pose.position.x, 0.0f, (float)msg.pose.position.y);

        robot.transform.position = new_pos;
        robot.transform.localRotation = Quaternion.AngleAxis((new Quaternion((float)msg.pose.orientation.x, (float)msg.pose.orientation.y, (float)msg.pose.orientation.z, (float)msg.pose.orientation.w)).eulerAngles.z - 90.0f, Vector3.up);
    }
}
