using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public struct Message
    {
        public int id;
        public string timestamp;
        public int type;
        public string data;
    }
}