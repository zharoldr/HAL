//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;

namespace RosMessageTypes.Pedsim
{
    [Serializable]
    public class LineObstaclesMsg : Message
    {
        public const string k_RosMessageName = "pedsim_msgs/LineObstacles";
        public override string RosMessageName => k_RosMessageName;

        //  A collection of line obstacles.
        //  No need to header since these are detemined at sim initiation time.
        public HeaderMsg header;
        public LineObstacleMsg[] obstacles;

        public LineObstaclesMsg()
        {
            this.header = new HeaderMsg();
            this.obstacles = new LineObstacleMsg[0];
        }

        public LineObstaclesMsg(HeaderMsg header, LineObstacleMsg[] obstacles)
        {
            this.header = header;
            this.obstacles = obstacles;
        }

        public static LineObstaclesMsg Deserialize(MessageDeserializer deserializer) => new LineObstaclesMsg(deserializer);

        private LineObstaclesMsg(MessageDeserializer deserializer)
        {
            this.header = HeaderMsg.Deserialize(deserializer);
            deserializer.Read(out this.obstacles, LineObstacleMsg.Deserialize, deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.WriteLength(this.obstacles);
            serializer.Write(this.obstacles);
        }

        public override string ToString()
        {
            return "LineObstaclesMsg: " +
            "\nheader: " + header.ToString() +
            "\nobstacles: " + System.String.Join(", ", obstacles.ToList());
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
