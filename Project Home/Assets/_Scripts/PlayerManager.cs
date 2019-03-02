using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace HOME
{
    public static class PlayerManager
    {
        private static List<Player> players_ = new List<Player>();
        public static int PlayerCount { get { return players_.Count; } }

        public static void AddPlayer(Player p)
        {
            if (players_.Contains(p)) { return; }
            players_.Add(p);
            p.AssignRwPlayer(ReInput.players.GetPlayer(players_.Count - 1));
            p.AssignIndex(players_.Count - 1);
        }

        public static Player GetPlayer(int index)
        {
            return players_[index];
        }

        public static bool IsPlayerRegistered(Player p)
        {
            return players_.Contains(p);
        }

        public static void RemovePlayer(Player p)
        {
            players_.Remove(p);
        }

        public static void RemovePlayer(int index)
        {
            if (index > players_.Count || index < 0) { return; }
            players_.RemoveAt(index);
        }
    }
}

