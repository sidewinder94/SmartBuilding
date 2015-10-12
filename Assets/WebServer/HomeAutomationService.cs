using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.WebServer
{
    class HomeAutomationService : IHomeAutomationService
    {
        private static readonly List<String> Rooms = new List<string>
        {
            "HeatPump",
            "Terrain",
            "LivingRoom",
            "WC",
            "Room1",
            "Cellar",
            "Garage",
            "Room2",
            "Room3",
            "Room4",
            "WCUpstairs",
            "UpstairsLane",
            "UpstairsBathroom"
        };

        public List<int> GetRoomList()
        {
            return Enumerable.Range(0, Rooms.Count).ToList();
        }

        public String GetRoomName(int roomId)
        {
            return roomId < Rooms.Count ? Rooms[roomId] : "Invalid ID";
        }

        public List<string> GetAvailableProperties(int roomId)
        {
            if (roomId > Rooms.Count) return new List<string>();
            return roomId <= 1 ? new List<string>() : new List<string> { "TargetTemperature" };
        }

        public bool SetRoomProperty(int roomId, string property, string value)
        {
            if (roomId > Rooms.Count) return false;

            var roomName = Rooms[roomId];

            var props = GetAvailableProperties(roomId);

            if (!props.Contains(property)) return false;

            Double dble;

            if (!Double.TryParse(value, out dble)) return false;

            GameObject.Find(roomName).GetComponent<RoomScript>().TargetTemperature = dble;

            return true;
        }

    }
}
