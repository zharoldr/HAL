using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using RosMessageTypes.Pedsim;

public class HAL : MonoBehaviour {

    private ROSConnection ros;

    // Type: pedsim_msgs/AgentStates
    public string agent_topic = "/pedsim_simulator/simulated_agents";

    // Type: nav_msgs/Odometry
    public string robot_topic = "/pedsim_simulator/robot_position";

    public GameObject agent_prefab;

    private List<GameObject> agents;

    public List<GameObject> agent_models;

    void Start() {
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<AgentStatesMsg>(agent_topic, agent_callback);
        ros.Subscribe<RosMessageTypes.Nav.OdometryMsg>(robot_topic, robot_callback);

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

    void robot_callback(RosMessageTypes.Nav.OdometryMsg msg) {
        // Debug.Log(msg);
    }
}
