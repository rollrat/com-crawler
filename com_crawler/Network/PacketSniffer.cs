// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace com_crawler.Network
{
    public class PacketSniffer
    {
        Socket socket;
        byte[] data = new byte[4096 * 128];
        bool continue_sniffing = false;
        
        public PacketSniffer(string ip)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, System.Net.Sockets.ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(ip), 0));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            byte[] inBytes = new byte[] { 1, 0, 0, 0 };
            byte[] outBytes = new byte[] { 0, 0, 0, 0 };
            socket.IOControl(IOControlCode.ReceiveAll, inBytes, outBytes);
        }

        public void Start(Action<Packet, int> on_recieve)
        {
            if (continue_sniffing)
                return;
            continue_sniffing = true;
            Task.Run(() =>
            {
                while (continue_sniffing)
                {
                    Length = socket.Receive(data);
                    on_recieve(Packet.ParsePacket(LinkLayers.Raw, data), Length);
                }
            });
        }

        public void Stop()
        {
            continue_sniffing = false;
        }

        public byte[] Data { get { return data; } }
        public int Length { get; private set; }
    }
}
