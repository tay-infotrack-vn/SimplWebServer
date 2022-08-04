using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimplWebServer
{
    public abstract class CSHTTPServer
    {
        private int portNum = 90;
        private TcpListener listener;
        System.Threading.Thread Thread;

        public Hashtable respStatus;

        public string Name = "MyHTTPServer/1.0.*";

        public bool IsAlive
        {
            get
            {
                return this.Thread.IsAlive;
            }
        }

        public CSHTTPServer()
        {
            //
            respStatusInit();
        }

        public CSHTTPServer(int thePort)
        {
            portNum = thePort;
            respStatusInit();
        }

        private void respStatusInit()
        {
            respStatus = new Hashtable();

            respStatus.Add(200, "200 Ok");
            respStatus.Add(201, "201 Created");
            respStatus.Add(202, "202 Accepted");
            respStatus.Add(204, "204 No Content");

            respStatus.Add(301, "301 Moved Permanently");
            respStatus.Add(302, "302 Redirection");
            respStatus.Add(304, "304 Not Modified");

            respStatus.Add(400, "400 Bad Request");
            respStatus.Add(401, "401 Unauthorized");
            respStatus.Add(403, "403 Forbidden");
            respStatus.Add(404, "404 Not Found");

            respStatus.Add(500, "500 Internal Server Error");
            respStatus.Add(501, "501 Not Implemented");
            respStatus.Add(502, "502 Bad Gateway");
            respStatus.Add(503, "503 Service Unavailable");
        }

        public void Listen()
        {
            bool done = false;

            listener = new TcpListener(IPAddress.Any, 90);

            listener.Start();

            WriteLog("Listening On: " + portNum.ToString());

            while (!done)
            {
                WriteLog("Waiting for connection...");
                CsHTTPRequest newRequest = new CsHTTPRequest(listener.AcceptTcpClient(), this);
                Thread Thread = new Thread(new ThreadStart(newRequest.Process));
                Thread.Name = "HTTP Request";
                Thread.Start();
            }

        }

        public void WriteLog(string EventMessage)
        {
            Console.WriteLine(EventMessage);
        }

        public void Start()
        {
            // CSHTTPServer HTTPServer = new CSHTTPServer(portNum);
            this.Thread = new Thread(new ThreadStart(this.Listen));
            this.Thread.Start();
        }

        public abstract void OnResponse(ref HTTPRequestStruct rq, ref HTTPResponseStruct rp);
    }

    public class SimpServer : CSHTTPServer
    {
        public override void OnResponse(ref HTTPRequestStruct rq, ref HTTPResponseStruct rp)
        {
            String response = "<h1>Hello</h1>";
            Byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            rp.BodyData = responseBytes;
        }
    }
}
