using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Networking
{
    public class Server : MonoBehaviour
    {
        Queue<string> action_queue = new Queue<string>();

        bool running;

        void Awake()
        {
            running = true;
            Task.Run(() => {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ip_address = host.AddressList[0];
                IPEndPoint local_endpoint = new IPEndPoint(ip_address, 11111);

                Socket listener = new Socket(ip_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(local_endpoint);
                listener.Listen(10);

                while (running)
                {
                    Socket handler = listener.Accept();

                    string data = null;
                    byte[] bytes = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytes_received = handler.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, bytes_received);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            data = data.Substring(0, data.Length - 5);
                            break;
                        }
                    }

                    action_queue.Enqueue(data);

                    byte[] return_message = Encoding.ASCII.GetBytes(data);
                    handler.Send(return_message);
                    
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }

            });
        }

        void Update()
        {
            if (action_queue.Count > 0)
            {
                string test = action_queue.Dequeue();
                Debug.Log(test);

                if (test == "Connect")
                    CreatePlayer("Test");

                if (test == "Disconnect")
                    Destroy(GameObject.Find("Test"));
                
                // if (test == "Move")
                //     GameObject.Find("Test").GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1), ForceMode.Impulse);
                // if (test == "A")
                //     GameObject.Find("Test").GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -1), ForceMode.Impulse);
                // if (test == "S")
                //     GameObject.Find("Test").GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0), ForceMode.Impulse);
                // if (test == "D")
                //     GameObject.Find("Test").GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0), ForceMode.Impulse);
            }
        }
        void OnApplicationQuit()
        {
            running = false;
        }

        void CreatePlayer(string username)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
            player.name = username;
            player.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Blue");
            player.AddComponent<Rigidbody>();
        }
    }
}