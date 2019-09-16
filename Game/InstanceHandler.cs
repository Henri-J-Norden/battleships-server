using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.AspNetCore.SignalR;

namespace Game {
    public static class InstanceHandler {
        public static string ProgramPath;

        static Dictionary<string, Instance> Instances = new Dictionary<string, Instance>();
        static Dictionary<string, string> ClientMap = new Dictionary<string, string>();


        public static Instance TryGetInstance(string guid) {
            Instance i;
            Instances.TryGetValue(guid, out i);
            return i;
        }

        public static void NewInstance(IClientProxy caller, string connectionId, string guid) {
            Instances[guid] = new Instance(caller, guid);
            ClientMap[connectionId] = guid;
        }

        public static void TryRemoveInstance(string connectionId) {
            string guid;
            //Console.WriteLine(client.GetHashCode());
            if (ClientMap.TryGetValue(connectionId, out guid)) {
                Instance i;
                Instances.Remove(guid, out i);
                i.Dispose();
                ClientMap.Remove(connectionId);
            }
        }

        public static string FormatGameString(string gameString) {
            return gameString.Replace("\v", "<br/>");
        }
    }
}
