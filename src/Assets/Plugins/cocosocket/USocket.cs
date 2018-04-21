using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace cocosocket4unity
{
    public class USocket
    {
        private Socket clientSocket;
        private SocketListener listener;
        private Protocol protocol;
        private string ip;
        private int port;
        private int status;
        private bool asyc;//异步收取
        private bool serverClose = true;//服务器主动关闭
        public const int STATUS_INIT = 0;
        public const int STATUS_CONNECTING = 1;
        public const int STATUS_CONNECTED = 2;
        public const int STATUS_CLOSED = 3;
        private ByteBuf buf;//接收缓存区
        //private BlockingQueue<ByteBuf> queue ;//阻塞队列
        private long sending;//发送的数量
        private long sended;//已经发送成功的数量
        private long bytesSended;//已经发送出去的字节数量
        /**
         * 构造（但不完善，需要设置监听器和协议解析器）
         */
        public USocket()
        {
            buf = new ByteBuf(4096);
            //queue = new BlockingQueue<ByteBuf>(5000);
        }
        /**
         * 构造
         */
        public USocket(SocketListener listener, Protocol protocol)
        {
            this.listener = listener;
            this.protocol = protocol;
            buf = new ByteBuf(4096);
            //queue = new BlockingQueue<ByteBuf>(5000);
        }
        /**
         * 连接指定地址
        */
        public void Connect(string ip, int port)
        {
            this.status = STATUS_CONNECTING;
            this.ip = ip;
            this.port = port;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.NoDelay = true;
            LingerOption linger = new LingerOption(false, 0);
            clientSocket.LingerState = linger;
            //clientSocket.ExclusiveAddressUse = false;
            //clientSocket.SendTimeout = 3000;//send timeout
            //clientSocket.SendBufferSize = 1024 * 8;//16k的发送缓冲区
            //clientSocket.Blocking = false;
            //clientSocket.DontFragment = false;
            clientSocket.BeginConnect(this.ip, this.port, connected, this);
        }
        /**
         * 连接成功
        */
        private void connected(IAsyncResult asyncConnect)
        {
            if (this.clientSocket.Connected)
            {
                this.clientSocket.EndConnect(asyncConnect);
                this.status = STATUS_CONNECTED;
                receive();//开始收取
                //Thread t = new Thread(new ThreadStart(send));//启动发送线程，同步发送
                //t.IsBackground = true;
                //t.Start();
                this.listener.OnOpen(this);
            }
            else
            {
                this.status = STATUS_CLOSED;
                this.serverClose = false;
                this.listener.OnClose(this, false);
            }
        }
        /**
         * 装入一个监听器
         */
        public void setListener(SocketListener listener)
        {
            this.listener = listener;
        }
        /**
         * 装入一个协议解析器
         */
        public void setProtocol(Protocol p)
        {
            this.protocol = p;
        }
        /**
         * 协议
         */
        public Protocol getProtocol()
        {
            return this.protocol;
        }
        public int getStatus()
        {
            return this.status;
        }
        public bool isAsyc()
        {
            return asyc;
        }
        public void setAsyc(bool a)
        {
            this.asyc = a;
        }
        public string getIp()
        {
            return this.ip;
        }
        public int getPort()
        {
            return this.port;
        }
        public long getSending()
        {
            return this.sending;
        }
        public long getSended()
        {
            return this.sended;
        }
        public long getBytesSended()
        {
            return this.bytesSended;
        }
        /**
         * 关闭连接
         */
        public void Close(bool serverClose = false)
        {
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    this.status = STATUS_CLOSED;
                    this.serverClose = serverClose;
                    this.listener.OnClose(this, this.serverClose);
                }
            }
            catch (Exception e)
            {
                this.status = STATUS_CLOSED;
                this.serverClose = serverClose;
                this.listener.OnClose(this, this.serverClose);
            }
        }
        /**
         *发送
         */
        public void Send(Frame frame)
        {
            try
            {
                if (this.status == STATUS_CONNECTED)
                {
                    this.sending++;
                    ByteBuf bb = frame.GetData();
                    SocketAsyncEventArgs arg = new SocketAsyncEventArgs();
                    arg.SetBuffer(bb.GetRaw(), bb.ReaderIndex(), bb.ReadableBytes());
                    arg.UserToken = bb;
                    arg.Completed += new EventHandler<SocketAsyncEventArgs>(OnSend);
                    this.clientSocket.SendAsync(arg);
                }
            }
            catch (Exception ex)
            {
                this.Close(false);
            }
        }
        /**
         * 发送回调
         */
        private void OnSend(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                ByteBuf bb = e.UserToken as ByteBuf;
                Interlocked.Increment(ref this.sended);
                Interlocked.Add(ref this.bytesSended, bb.ReadableBytes());
            }
            else//发送失败
            {
                this.Close(false);
            }
        }
        /**
         * 接收数据
         */
        private void receive()
        {
            if (this.status == STATUS_CONNECTED)
            {
                try
                {
                    clientSocket.BeginReceive(buf.GetRaw(), 0, buf.GetRaw().Length, SocketFlags.None, new AsyncCallback(onRecieved), clientSocket);
                }
                catch (Exception e)
                {
                    this.status = STATUS_CLOSED;
                    this.listener.OnError(this, e.StackTrace + e.Message);
                    this.Close(true);
                }
            }
        }
        /**
         * 异步收取信息
         */
        private void onRecieved(IAsyncResult ar)
        {
            try
            {
                Socket so = (Socket)ar.AsyncState;
                int len = so.EndReceive(ar);
                if (len > 0)
                {
                    buf.ReaderIndex(0);
                    buf.WriterIndex(len);
                    while (true)
                    {
                        ByteBuf frame = this.protocol.TranslateFrame(buf);
                        if (frame != null)
                        {
                            this.listener.OnMessage(this, frame);
                        }
                        else
                        {
                            break;
                        }
                    }
                    this.receive();
                }
                else
                {
                    this.Close(true);
                }
            }
            catch (Exception e)
            {
                this.Close(true);
            }
        }
    }
}

