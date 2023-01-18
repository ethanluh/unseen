using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

using Networking;

using UnityEngine;

namespace Networking
{
    public class Client : MonoBehaviour
    {
        void Awake()
        {
            MessageServer("Connect");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MessageServer("Disconnect");
                
                Application.Quit();
            }
            // if (Input.GetKeyDown(KeyCode.A))
            //     MessageServer("A");
            // if (Input.GetKeyDown(KeyCode.S))
            //     MessageServer("S");
            // if (Input.GetKeyDown(KeyCode.D))
            //     MessageServer("D");
        }

        string MessageServer(string message)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip_address = host.AddressList[0];
            IPEndPoint remote_endpoint = new IPEndPoint(ip_address, 11111);

            Socket sender = new Socket(ip_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            byte[] bytes = new byte[1024];

            sender.Connect(remote_endpoint);

            byte[] send_message = Encoding.ASCII.GetBytes(message + "<EOF>");
            int bytes_sent = sender.Send(send_message);

            int bytes_received = sender.Receive(bytes);
            
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

            return Encoding.ASCII.GetString(bytes, 0, bytes_received);
        }
    }
}