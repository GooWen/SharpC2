﻿using System;
using System.Collections.Generic;
using System.Linq;

using SharpC2.Models;
using SharpC2.Listeners;

namespace TeamServer.Controllers
{
    public class ListenerControllerBase
    {
        public HttpListenerController HttpListenerController { get; private set; }
        public TcpListenerController TcpListenerController { get; private set; }
        public ServerController ServerController { get; private set; }
        private AgentController AgentController { get; set; }
        private CryptoController CryptoController { get; set; }

        private event EventHandler<ServerEvent> OnServerEvent;

        public ListenerControllerBase(ServerController server, AgentController agent, CryptoController crypto)
        {
            ServerController = server;
            AgentController = agent;
            CryptoController = crypto;

            HttpListenerController = new HttpListenerController(this);
            TcpListenerController = new TcpListenerController(this);

            OnServerEvent += ServerController.ServerEventHandler;
        }

        public ListenerHttp StartHttpListener(NewHttpListenerRequest request, string user)
        {
            return HttpListenerController.StartHttpListener(request, AgentController, CryptoController, user);
        }

        public ListenerTcp StartTcpListener(NewTcpListenerRequest request)
        {
            return TcpListenerController.StartTcpListener(request);
        }

        public IEnumerable<ListenerHttp> GetHttpListeners()
        {
            return HttpListenerController.GetHttpListeners();
        }

        public IEnumerable<ListenerTcp> GetTcpListeners()
        {
            return TcpListenerController.GetTcpListeners();
        }

        public ListenerBase GetListener(string listenerId)
        {
            var listeners = new List<ListenerBase>();

            foreach (var http in GetHttpListeners())
            {
                listeners.Add(http);
            }

            foreach (var tcp in GetTcpListeners())
            {
                listeners.Add(tcp);
            }

            return listeners.FirstOrDefault(l => l.ListenerId.Equals(listenerId, StringComparison.OrdinalIgnoreCase));
        }

        public bool StopListener(string listenerId, ListenerType type, string user)
        {
            var success = false;

            if (type == ListenerType.HTTP)
            {
                success = HttpListenerController.StopHttpListener(listenerId, user);
            }
            else if (type == ListenerType.TCP)
            {
                success = TcpListenerController.StopTcpListener(listenerId, user);
            }

            return success;
        }

        public IOrderedEnumerable<WebLog> GetWebLogs()
        {
            return HttpListenerController.GetWebLogs();
        }
    }
}