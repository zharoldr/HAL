using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using RosMessageTypes.Pedsim;

public class HAL : MonoBehaviour {

    private ROSConnection ros;
    
    // This is the rostopic Unity will subscribe to for the human transformations.
    public string agent_topic = "/pedsim_simulator/simulated_agents";
    
    // This is the rostopic Unity will subscribe to for the robot transformation.
    public string robot_topic = "/pedsim_simulator/robot_state";

    private GameObject robot;
    private List<GameObject> agents;

    public GameObject robot_model;

    public List<GameObject> agent_models;

    public GameObject Robot {
        get => robot;
    }

    void Awake() {
        // Create a ROSConnection instance.
        ros = ROSConnection.GetOrCreateInstance();
        
        // Subscribe to the two rostopics mentioned before and register callbacks.
        ros.Subscribe<AgentStatesMsg>(agent_topic, agent_callback);
        ros.Subscribe<AgentStateMsg>(robot_topic, robot_callback);
        
        // Create the robot.
        robot = Instantiate(robot_model, Vector3.zero, Quaternion.identity);
        
        // Initialize a new list to add the humans to.
        agents = new List<GameObject>();
    }
    
    void agent_callback(AgentStatesMsg msg) {
        if (agents.Count == 0) {
            // This must be the first message! The message contains how many humans
            // there are, so I'll add that many humans to the agents list.
            for (int i = 0; i < msg.agent_states.Length; i++) {
                // Create a new agent by picking a random human from a list of humans.
                GameObject new_agent = Instantiate(agent_models[Random.Range(0, agent_models.Count)], Vector3.zero, Quaternion.identity);
                
                // Add the new human to the agents list.
                agents.Add(new_agent);
            }
        } else {
            // Loop through all agents in message list.
            for (int i = 0; i < msg.agent_states.Length; i++) {
                // Each agent has an id. Here, I get current position of the id'th
                // agent from the agents list.
                Vector3 current_pos = agents[(int)msg.agent_states[i].id].transform.position;
                
                // I get the new position for the id'th agent from the message.
                Vector3 want_pos = new Vector3((float)msg.agent_states[i].pose.position.x, 0.0f, (float)msg.agent_states[i].pose.position.y);
                
                // THIS IS BAD! I should NEVER call lerp methods inside a callback function.
                // new_pos should be created in Update()!
                Vector3 new_pos = Vector3.Lerp(current_pos, want_pos, Time.deltaTime);
                
                // Calculate which way the human is facing.
                Vector3 face_dir = (new_pos - current_pos).normalized;
                
                // Calculate a quick and dirty velocity that I can use to change the walk animation speed.
                float dirty_vel = Vector3.Distance(agents[(int)msg.agent_states[i].id].transform.position, new_pos);
                
                /*************************************************************
                    EVERYTHING IN THIS SECTION SHOULD HAPPEN IN Update()!
                *************************************************************/
                agents[(int)msg.agent_states[i].id].transform.position = new_pos; // <-- set human position
                agents[(int)msg.agent_states[i].id].GetComponent<Animator>().speed = 30.0f * dirty_vel; // <-- set animation speed
                
                if (face_dir != Vector3.zero) {
                    // THIS IS ALSO BAD! I am calling Slerp inside a callback!
                    // Like want_pos and new_pos, I should make a want_rot and new_rot in Update().
                    agents[(int)msg.agent_states[i].id].transform.rotation = Quaternion.Slerp(agents[(int)msg.agent_states[i].id].transform.rotation, Quaternion.LookRotation(face_dir), 15.0f * Time.deltaTime);
                }
                /*************************************************************
                *************************************************************/
            }
        }
    }
    void robot_callback(AgentStateMsg msg) {
        // Get the current position of robot.
        Vector3 current_pos = robot.transform.position;
        
        // Get the position of the robot from the message.
        Vector3 new_pos = new Vector3((float)msg.pose.position.x, 0.0f, (float)msg.pose.position.y);
        
        // Set the position of the robot (This should happen in Update()).
        robot.transform.position = new_pos;
        
        // Set the rotation of the robot (This should again happen in Update()).
        robot.transform.rotation = new Quaternion((float)msg.pose.orientation.x, -(float)msg.pose.orientation.z, (float)msg.pose.orientation.y, (float)msg.pose.orientation.w) * Quaternion.Euler(0.0f, 90.0f, 0.0f);
    }
}
