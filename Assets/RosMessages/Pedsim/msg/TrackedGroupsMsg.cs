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
    public class TrackedGroupsMsg : Message
    {
        public const string k_RosMessageName = "pedsim_msgs/TrackedGroups";
        public override string RosMessageName => k_RosMessageName;

        //  Message with all currently tracked groups
        // 
        public HeaderMsg header;
        //  Header containing timestamp etc. of this message
        public TrackedGroupMsg[] groups;
        //  All groups that are currently being tracked

        public TrackedGroupsMsg()
        {
            this.header = new HeaderMsg();
            this.groups = new TrackedGroupMsg[0];
        }

        public TrackedGroupsMsg(HeaderMsg header, TrackedGroupMsg[] groups)
        {
            this.header = header;
            this.groups = groups;
        }

        public static TrackedGroupsMsg Deserialize(MessageDeserializer deserializer) => new TrackedGroupsMsg(deserializer);

        private TrackedGroupsMsg(MessageDeserializer deserializer)
        {
            this.header = HeaderMsg.Deserialize(deserializer);
            deserializer.Read(out this.groups, TrackedGroupMsg.Deserialize, deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.WriteLength(this.groups);
            serializer.Write(this.groups);
        }

        public override string ToString()
        {
            return "TrackedGroupsMsg: " +
            "\nheader: " + header.ToString() +
            "\ngroups: " + System.String.Join(", ", groups.ToList());
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
