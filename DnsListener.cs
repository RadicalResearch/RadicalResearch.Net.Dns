namespace RadicalResearch.Net.Dns
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class DnsListener : IDisposable
    {
        private readonly DnsQueryResolver resolver;

        private readonly IPEndPoint localEndPoint;

        private readonly object internalLock = new object();

        private UdpClient udpClient;

        private State state;

        public DnsListener(DnsQueryResolver resolver)
            : this(new IPEndPoint(IPAddress.Any, 53), resolver)
        {
        }

        public DnsListener(IPEndPoint localEndPoint, DnsQueryResolver resolver)
        {
            this.localEndPoint = localEndPoint;
            this.resolver = resolver;
            this.state = State.Stopped;
        }

        public void Start()
        {
            lock (this.internalLock)
            {
                this.CheckDisposed();

                if (this.state == State.Started)
                {
                    return;
                }

                try
                {
                    this.udpClient = new UdpClient(this.localEndPoint);
                    this.BeginReceive();
                    this.state = State.Started;
                }
                catch
                {
                    this.state = State.Closed;
                    throw;
                }
            }
        }

        public void Stop()
        {
            lock (this.internalLock)
            {
                this.CheckDisposed();

                if (this.state == State.Started)
                {
                    this.udpClient.Close();
                }
            }
        }

        private void CheckDisposed()
        {
            if (this.state == State.Closed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        private void BeginReceive()
        {
            this.udpClient.BeginReceive(this.RequestCallback, null);
        }

        private void RequestCallback(IAsyncResult ar)
        {
            IPEndPoint remoteEp = null;
            try
            {
                var buffer = this.udpClient.EndReceive(ar, ref remoteEp);
                try
                {
                    this.ProcessQuery(buffer, remoteEp);
                }
                finally
                {
                    this.BeginReceive();
                }
            }
            catch (ObjectDisposedException)
            {
                this.state = State.Stopped;
            }
        }
        
        protected virtual void ProcessQuery(byte[] buffer, IPEndPoint remoteEndPoint)
        {
            var query = DnsQuery.Read(buffer);

            var response = this.resolver(query);

            var responseBytes = response.GetBytes();

            this.udpClient.Send(responseBytes, responseBytes.Length, remoteEndPoint);
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (this.internalLock)
            {
                try
                {
                    if (this.state == State.Closed)
                    {
                        return;
                    }
                    ((IDisposable)this.udpClient).Dispose();
                }
                finally
                {
                    this.state = State.Closed;
                }
            }
        }

        private enum State
        {
            Started,

            Stopped,

            Closed
        }
    }
}
