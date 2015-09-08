using System;
using System.Collections.Generic;
using System.Net.Sockets;
//susing System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace covertFiles
{
    public static class Util
    {
        #region static Variables
        /*Forms static pointers*/
        public static String Version = covertFiles.Resources.version;
        
        private static FormMain _MainForm = null;
        public static FormMain MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }
        private static FormAbout _AboutForm = null;
        public static FormAbout AboutForm
        {
            get
            {
                return _AboutForm;
            }
            set
            {
                _AboutForm = value;
            }
        }

        
        private static FormHelp _HelpForm = null;
        public static FormHelp HelpForm
        {
            get
            {
                return _HelpForm;
            }
            set
            {
                _HelpForm = value;
            }
        }
        private static System.Drawing.SizeF _MessageFormPercent = new System.Drawing.SizeF((float)0.7, (float)0.5);
        public static System.Drawing.SizeF MessageFormPercent
        {
            get
            {
                return _MessageFormPercent;
            }
            set
            {
                if (_MessageFormPercent != value)
                {
                    _MessageFormPercent = value;
                    
                    Util.MainForm.Size = new System.Drawing.Size(Util.MainForm.Size.Width, Util.MainForm.Size.Height + 1);
                    Util.MainForm.Size = new System.Drawing.Size(Util.MainForm.Size.Width, Util.MainForm.Size.Height - 1);
                }
            }
        }
        private static bool _formsIgnoreResizeEvents = false;
        public static bool formsIgnoreResizeEvents
        {
            get
            {
                return _formsIgnoreResizeEvents;
            }
            set
            {
                _formsIgnoreResizeEvents = value;
                Util.HelpForm.ignoreResizeEvents = value;
                Util.MessageForm.ignoreResizeEvents = value;
                Util.StatForm.ignoreResizeEvents = value;
            }

        }
        private static FormMessageControl _MessageForm = null;
        public static FormMessageControl MessageForm
        {
            get
            {
                return _MessageForm;
            }
            set
            {
                _MessageForm = value;
            }

        }
        private static FormStatistics _StatForm = null;
        public static FormStatistics StatForm
        {
            get
            {
                return _StatForm;
            }
            set
            {
                _StatForm = value;
            }
        }

        private static FormProperties _PropertiesForm = null;
        public static FormProperties PropertiesForm
        {
            get
            {
                return _PropertiesForm;
            }
            set
            {
                _PropertiesForm = value;
            }
        }
        #endregion
        #region static Functions
        static public string LocalIPAddress()
        {
            System.Net.IPHostEntry host;
            string localIP = "";
            host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
        #endregion

        public static class covertChannel
        {
            #region Definitions

            /*Queu with all sending bits*/
            public static Queue<int> sendQueu = new Queue<int>();
            /*Queu with all receiving bits*/
            public static Queue<int> reciviedBits = new Queue<int>();
            /*Queu of the receiving message*/
            private static String reciveidQueu = "";
            /*interval time for timers in ms*/
            public static int Interval = 1000;
            public static bool isRunning;

            private static int startBit = 2;
            private static int endBit = 3;
            private static int synchronizationSilentBit = 4;

            static int silentsendBitCount = 0;


            public static bool hammingErrorCorrection = true;
            public static int silentIntervals = 0;
            public static int intervalAdjusting = 0;
            public static bool startOfFrame = true;

            private enum CovType
            {
                UseTCP,
                UseCPU,
                UseFileLock,
                UseSharedFile,
                Other
            }
            private static CovType Type;
            #endregion
            #region functions
            public delegate void invokeFunct(String msg);
            /*intitialize the util library*/
            public static void init()
            {
                isRunning = true;
                //init statistics
                Util.covertChannel.Statistics.init();

                Util.covertChannel.useShareFile.init();
                Util.covertChannel.useTCP.init();
                Util.covertChannel.useFileLock.init();
                Util.covertChannel.useSharedCpu.init();

                Type = CovType.Other;

            }
            /*finalize util*/
            public static void finalize()
            {
                isRunning = false;



                Util.covertChannel.useShareFile.finalize();
                Util.covertChannel.useTCP.finalize();
                Util.covertChannel.useFileLock.finalize();
            }
            /*
             * function to send a message
             *  input:msg the message to send
             *  
             */
            public static void sendMessage(String Msg)
            {
                sendBit(startBit);
                char[] tmp = Msg.ToCharArray();
                foreach (char c in tmp) // for all bytes in message
                {
                    sendByte(c);
                }
                sendByte((char)0);    /*send the end of message byte */
                sendBit(endBit);
            }
            /*
             * function to send a byte
             *  input:c the byte to send
             *  
             */
            private static void sendByte(char c)
            {

                int[] hammingByte = new int[13];
                int tmpBit;

                if (hammingErrorCorrection == true)
                {
                    hammingByte[3] = c % 2;
                    hammingByte[5] = (c / 2) % 2;
                    hammingByte[6] = (c / 4) % 2;
                    hammingByte[7] = (c / 8) % 2;
                    hammingByte[9] = (c / 16) % 2;
                    hammingByte[10] = (c / 32) % 2;
                    hammingByte[11] = (c / 64) % 2;
                    hammingByte[12] = (c / 128) % 2;

                    hammingByte[1] = hammingByte[3] ^ hammingByte[5] ^ hammingByte[7] ^ hammingByte[9] ^ hammingByte[11];
                    hammingByte[2] = hammingByte[3] ^ hammingByte[6] ^ hammingByte[7] ^ hammingByte[10] ^ hammingByte[11];
                    hammingByte[4] = hammingByte[5] ^ hammingByte[6] ^ hammingByte[7] ^ hammingByte[12];
                    hammingByte[8] = hammingByte[9] ^ hammingByte[10] ^ hammingByte[11] ^ hammingByte[12];

                    for (int i = 1; i < hammingByte.Length; i++)
                    {
                        sendBit(hammingByte[i]);
                    }
                }
                else
                {

                    tmpBit = c % 2;
                    sendBit(tmpBit);
                    tmpBit = (c / 2) % 2;
                    sendBit(tmpBit);
                    tmpBit = (c / 4) % 2;
                    sendBit(tmpBit);
                    tmpBit = (c / 8) % 2;
                    sendBit(tmpBit);
                    tmpBit = (c / 16) % 2;
                    sendBit(tmpBit);
                    tmpBit = (c / 32) % 2;
                    sendBit(tmpBit);
                    tmpBit = (c / 64) % 2;
                    sendBit(tmpBit);
                    tmpBit = (c / 128) % 2;
                    sendBit(tmpBit);
                }

            }
            /*
             * function to send a bit
             *  input:bit the bit to send
             *  
             */
            private static void sendBit(int bit)
            {
                if (silentIntervals != 0)
                {
                    if (silentsendBitCount >= silentIntervals)
                    {
                        silentsendBitCount = 0;
                        sendBit(synchronizationSilentBit);
                    }
                    silentsendBitCount++;
                }
                sendQueu.Enqueue(bit);

            }
            private static void receivedBit(int bit)
            {

                reciviedBits.Enqueue(bit);
                int byteSize = 8;
                if (hammingErrorCorrection == true)
                {
                    byteSize = 12;
                }
                if (reciviedBits.Count >= byteSize)
                {// received a byte
                    int tmpByte = 0;
                    if (hammingErrorCorrection == true)
                    {
                        int hammError = 0;
                        int[] hammingByte = new int[13];
                        for (int i = 1; i < hammingByte.Length; i++)
                        {
                            hammingByte[i] = reciviedBits.Dequeue();
                        }
                        if (hammingByte[1] != (hammingByte[3] ^ hammingByte[5] ^ hammingByte[7] ^ hammingByte[9] ^ hammingByte[11]))
                        {
                            hammError += 1;
                        }
                        if (hammingByte[2] != (hammingByte[3] ^ hammingByte[6] ^ hammingByte[7] ^ hammingByte[10] ^ hammingByte[11]))
                        {
                            hammError += 2;
                        }
                        if (hammingByte[4] != (hammingByte[5] ^ hammingByte[6] ^ hammingByte[7] ^ hammingByte[12]))
                        {
                            hammError += 4;
                        }
                        if (hammingByte[8] != (hammingByte[9] ^ hammingByte[10] ^ hammingByte[11] ^ hammingByte[12]))
                        {
                            hammError += 8;
                        }
                        if (hammError < hammingByte.Length)
                        {
                            if (hammingByte[hammError] == 0)
                            {
                                Util.covertChannel.Statistics.zeroBitReceivedCount--;
                            }
                            hammingByte[hammError] = 1 - hammingByte[hammError];
                            Util.covertChannel.Statistics.ErrorBitReceivedCount++;
                        }

                        tmpByte += hammingByte[3];
                        tmpByte += hammingByte[5] << 1;
                        tmpByte += hammingByte[6] << 2;
                        tmpByte += hammingByte[7] << 3;
                        tmpByte += hammingByte[9] << 4;
                        tmpByte += hammingByte[10] << 5;
                        tmpByte += hammingByte[11] << 6;
                        tmpByte += hammingByte[12] << 7;

                    }
                    else
                    {
                        tmpByte += reciviedBits.Dequeue();
                        tmpByte += reciviedBits.Dequeue() << 1;
                        tmpByte += reciviedBits.Dequeue() << 2;
                        tmpByte += reciviedBits.Dequeue() << 3;
                        tmpByte += reciviedBits.Dequeue() << 4;
                        tmpByte += reciviedBits.Dequeue() << 5;
                        tmpByte += reciviedBits.Dequeue() << 6;
                        tmpByte += reciviedBits.Dequeue() << 7;
                    }
                    reciveidQueu += ((char)tmpByte).ToString();
                    if (tmpByte == 0)
                    {// received end of message byte
                        //update graphs
                        MessageForm.Invoke(new invokeFunct(MessageForm.recieveMesage), new Object[] { reciveidQueu });
                        reciveidQueu = "";
                    }
                }
                if (bit == 0)
                {
                    Util.covertChannel.Statistics.zeroBitReceivedCount++;
                }
            }
            public static int calculateETA()
            {
                int res = 0;
                if (Type == CovType.UseCPU)
                {
                    res = useSharedCpu.calculateETA();
                }
                else if (Type == CovType.UseFileLock)
                {
                    res = useFileLock.calculateETA();
                }
                else if (Type == CovType.UseSharedFile)
                {
                    res = 0;
                }
                else if (Type == CovType.UseTCP)
                {
                    res = useTCP.calculateETA();
                }
                return res;
            }
            #endregion
            #region covert channel Over TCP timming
            public static class useTCP
            {
                #region Definitions
                /*represent the delay time
                  between two continuous package 
                  for transmition of zero bit*/
                static private int _TimeZeroBit = 0;
                static public int timeZeroBit
                {
                    get
                    {
                        return _TimeZeroBit;
                    }
                    set
                    {
                        _TimeZeroBit = value;
                    }
                }
                /* represent the delay time 
                 * between two continuous package
                 * for transmition of zero bit*/
                static private int _TimeOneBit = 1000;
                static public int timeOneBit
                {
                    get
                    {
                        return _TimeOneBit;
                    }
                    set
                    {
                        _TimeOneBit = value;
                    }
                }
                /* represent the timing error which is
                 * valid to understate zero or one bit*/
                static private int _timeError = 300;
                static public int timeError
                {
                    get
                    {
                        return _timeError;
                    }
                    set
                    {
                        _timeError = value;
                    }
                }
                static private int _MessageTimeOut = 3000;
                static public int messageTimeOut
                {
                    get
                    {
                        return _MessageTimeOut;
                    }
                    set
                    {
                        _MessageTimeOut = value;
                    }
                }
                /*the message of the sending package*/
                static public String packageMessage = "123456789";
                /*the remote hostname*/
                static private String _RemoteHostName = "nmhlios.selfip.com";//"localhost";
                static public String remoteHostName
                {
                    get
                    {
                        return _RemoteHostName;
                    }
                    set
                    {
                        _RemoteHostName = value;
                    }
                }
                /*the remote port*/
                static private int _RemotePort = 6666;
                static public int remotePort
                {
                    get
                    {
                        return _RemotePort;
                    }
                    set
                    {
                        _RemotePort = value;
                    }
                }
                /*the local port for incomming connection*/
                static private int _LocalPort = 6666;
                static public int localPort
                {
                    get
                    {
                        return _LocalPort;
                    }
                    set
                    {
                        _LocalPort = value;
                    }
                }
                static Socket tcpListener;
                static Socket tcpClient;
                /*
                 * Timers for to update input an output
                 */
                private static System.Threading.Thread ThreadWriter;
                private static System.Threading.Thread ThreadReader;
                #endregion
                static public void init()
                {
                    /*init input and output threads*/
                    ThreadReader = new System.Threading.Thread(useTCP.updateReciver);
                    ThreadWriter = new System.Threading.Thread(useTCP.updateSender);
                }
                static public void finalize()
                {
                    localStop();
                    ThreadReader.Abort();
                    ThreadWriter.Abort();
                }
                public static void remoteStop()
                {
                    try
                    {
                        tcpClient.Close();
                    }
                    catch
                    {
                    }
                }
                public static void localStop()
                {
                    try
                    {
                        tcpListener.Close();
                    }
                    catch
                    {
                    }

                }
                public static void localStart()
                {
                    ThreadReader.Start();
                }
                public static void remoteStart()
                {
                    ThreadWriter.Start();
                }
                public static void start()
                {
                    Type = CovType.UseTCP;
                }
                public static void stop()
                {
                    localStop();
                    remoteStop();

                }
                public static void sendPackage()
                {
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    tcpClient.Send(encoding.GetBytes(packageMessage));




                }
                public static void receivePackage(Socket socket)
                {
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    byte[] buffer = new byte[encoding.GetBytes(packageMessage).Length];
                    //read next package;
                    socket.Receive(buffer);
                }
                public static int calculateETA()
                {
                    int res = 0;
                    int[] msg;
                    msg = sendQueu.ToArray();
                    foreach (int bit in msg)
                    {
                        if (bit == startBit)
                        {
                            res += messageTimeOut;
                            
                        }
                        else if (bit == endBit)
                        {
                            ;
                        }
                        else if (bit == synchronizationSilentBit)
                        {
                            res+=messageTimeOut;
                        }
                        else if (bit == 1)
                        {
                            res+=timeOneBit;
                        }
                        else if (bit == 0)
                        {
                            res+=timeZeroBit;
                        }
                        else
                        {
                        }
                    }
                    return res;
                }
                /*
                 * this function is a callback function that called from WriteTimer each Interval ms
                 * comment: sends data from buffer sendQueu
                 */
                public static void updateSender()
                {

                    double timeSpace;
                    /*time last package received */
                    DateTime lastTime = DateTime.Now;
                    try
                    {
                        tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        tcpClient.NoDelay = true;

                        // tcpClient.SendBufferSize = 100;
                        // tcpClient.DontFragment = true;
                        // tcpClient.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.BsdUrgent, true);
                        // tcpClient.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay,true);


                        // tcpClient.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.PacketInformation);


                        //tcpClient.BeginConnect(remoteHostName, remotePort,null,null);
                        tcpClient.Connect(remoteHostName, remotePort);


                    }
                    catch (Exception exp)
                    {
                        MessageForm.Invoke(new invokeFunct(MessageForm.errorMessage), new Object[] { exp.Message });
                        return;
                    }
                    while (isRunning == true)
                    {
                        if (sendQueu.Count == 0)
                            continue;
                        try
                        {

                            if (sendQueu.Peek() == 1)
                            {
                                System.Threading.Thread.Sleep(timeOneBit);
                                /*calculate the time between the two continious package*/
                                timeSpace = DateTime.Now.Subtract(lastTime).TotalMilliseconds;
                                //enumerate last package received time
                                lastTime = DateTime.Now;
                                Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1, "Timespace:" + timeSpace);
                                sendPackage();
                            }
                            else if (sendQueu.Peek() == 0)
                            {
                                System.Threading.Thread.Sleep(timeZeroBit);
                                /*calculate the time between the two continious package*/
                                timeSpace = DateTime.Now.Subtract(lastTime).TotalMilliseconds;
                                //enumerate last package received time
                                lastTime = DateTime.Now;
                                Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0, "Timespace:" + timeSpace);
                                sendPackage();
                                Util.covertChannel.Statistics.zeroBitSendCount++;
                            }
                            else if (sendQueu.Peek() == startBit || sendQueu.Peek() == synchronizationSilentBit)//send dammy bit
                            {
                                System.Threading.Thread.Sleep(messageTimeOut);
                                /*calculate the time between the two continious package*/
                                timeSpace = DateTime.Now.Subtract(lastTime).TotalMilliseconds;
                                //enumerate last package received time
                                lastTime = DateTime.Now;
                                Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "Dammy bit Timespace:" + timeSpace);
                                sendPackage();
                                if (startOfFrame == true)
                                {
                                    System.Threading.Thread.Sleep((timeOneBit - timeZeroBit) / 4);
                                }
                            }
                            else if (sendQueu.Peek() == endBit)
                            {
                                Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Clear, Statistics.LogValue.Empty, "End of message");
                            }

                        }
                        catch (Exception exp)
                        {
                            MessageForm.Invoke(new invokeFunct(MessageForm.errorMessage), new Object[] { exp.Message });
                            return;
                        }
                        sendQueu.Dequeue();
                        Statistics.bitSentCount++;

                    }

                    tcpClient.Close();
                }

                /*
                 * this function is a callback function that called from ReadTimer each Interval ms
                 * comment: update received Queu data
                 */
                public static void updateReciver()
                {
                    try
                    {
                        double timeSpace;
                        /*time last package received */
                        DateTime lastTime = DateTime.Now;
                        /*initializing connection*/
                        tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        tcpListener.Bind(new IPEndPoint(IPAddress.Any, localPort));
                        tcpListener.NoDelay = true;
                        tcpListener.ReceiveBufferSize = 100;
                        tcpListener.Listen(1);
                        Socket socket = tcpListener.Accept();

                        //tcpListener.BeginAccept(null,null);
                        while (isRunning == true)
                        {
                            try
                            {


                                if (socket.Connected)
                                {
                                    receivePackage(socket);
                                    /*calculate the time between the two continious package*/
                                    timeSpace = DateTime.Now.Subtract(lastTime).TotalMilliseconds;
                                    //enumerate last package received time
                                    lastTime = DateTime.Now;
                                    if (Math.Abs(timeSpace - timeZeroBit) <= timeError)//package represent 0 bit
                                    {
                                        receivedBit(0);
                                        if (intervalAdjusting != 0)
                                        {
                                            lastTime.AddMilliseconds(intervalAdjusting * (timeSpace - timeZeroBit) / 100);
                                        }
                                        Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0, "Timespace:" + timeSpace);
                                    }
                                    else if (Math.Abs(timeSpace - timeOneBit) <= timeError)//package represent 1 bit
                                    {
                                        receivedBit(1);
                                        if (intervalAdjusting != 0)
                                        {
                                            lastTime.AddMilliseconds(intervalAdjusting * (timeSpace - timeOneBit) / 100);
                                        }



                                        Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1, "Timespace:" + timeSpace);
                                    }
                                    else if (timeSpace > messageTimeOut - timeError)//error or dammy package
                                    {
                                        Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Error: dammy package timespace:" + timeSpace);
                                    }
                                    else //error
                                    {
                                        //TODO: Error Bit
                                        receivedBit(0);
                                        Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Error: timespace:" + timeSpace);

                                    }
                                    Statistics.bitReceivedCount++;

                                }

                            }

                            catch (Exception exp)
                            {
                                MessageForm.Invoke(new invokeFunct(MessageForm.errorMessage), new Object[] { exp.Message });

                                return;
                            }
                        }

                        socket.Close();
                    }
                    catch(Exception exp)
                    {
                        System.Windows.Forms.MessageBox.Show("Error: Socket is closed!");

                    }
                }
            }
            #endregion
            #region covert channel Over Shared File

            public static class useShareFile
            {
                #region Definitions
                /*
             * Timers for to update input an output
             */
                private static System.Windows.Forms.Timer ThreadWriter;
                private static System.Windows.Forms.Timer ThreadReader;
                public static String DefaultFilePAth
                {
                    get
                    {
                        return "./";
                    }
                }
                public static String DefaultBit1File
                {
                    get
                    {
                        return "1.txt";
                    }
                }
                public static String DefaultBit0File
                {
                    get
                    {
                        return "0.txt";
                    }
                }
                public static String DefaultSendAckFile
                {
                    get
                    {
                        return "sendAck.txt";
                    }
                }
                public static String DefaultSendFile
                {
                    get
                    {
                        return "send.txt";
                    }
                }
                private static String _filesPath = DefaultFilePAth;
                public static String FilesPath
                {
                    get
                    {
                        return _filesPath;
                    }
                    set
                    {
                        _filesPath = value;
                    }
                }
                private static String _1bitFile = DefaultBit1File;
                public static String Bit1File
                {
                    get
                    {
                        return (_filesPath + "\\" + _1bitFile);
                    }
                    set
                    {
                        _1bitFile = value;
                    }
                }
                private static String _SendAckFile = DefaultSendAckFile;
                public static String SendAckFile
                {
                    get
                    {
                        return (_filesPath + "\\" + _SendAckFile);
                    }
                    set
                    {
                        _SendAckFile = value;
                    }
                }
                private static String _0bitFile = DefaultBit0File;
                public static String Bit0File
                {
                    get
                    {
                        return (_filesPath + "\\" + _0bitFile);
                    }
                    set
                    {
                        _0bitFile = value;
                    }
                }
                private static String _sendFile = DefaultSendFile;
                public static String SendFile
                {
                    get
                    {
                        return (_filesPath + "\\" + _sendFile);
                    }
                    set
                    {
                        _sendFile = value;
                    }
                }
                /*
                 * enum for transfer fsm
                 */
                private enum TransferState { idle, send, receive };
                /* current transfer fsm state*/
                private static TransferState transferFsm = TransferState.idle;
                #endregion
                static public void init()
                {
                    /*init input and output timmers*/
                    ThreadWriter = new System.Windows.Forms.Timer();
                    ThreadWriter.Interval = Interval;
                    ThreadWriter.Tick += new EventHandler(useShareFile.updateSender);


                    ThreadReader = new System.Windows.Forms.Timer();
                    ThreadReader.Interval = Interval;
                    ThreadReader.Tick += new EventHandler(useShareFile.updateReciver);
                }
                static public void finalize()
                {
                    ThreadReader.Dispose();
                    ThreadWriter.Dispose();
                }
                static public void start()
                {
                    ThreadWriter.Start();
                    ThreadReader.Start();
                    Type = CovType.UseSharedFile;
                }
                public static void stop()
                {
                    ThreadWriter.Stop();
                    ThreadReader.Stop();
                }
                /* creates a new file
             * input:file the filepath of file
             * return: false on erros
             *        true else
             * comment: if(file is existing return false
             */
                private static bool FileCreate(String file)
                {
                    try
                    {
                        System.IO.FileStream fs = System.IO.File.Open(file, System.IO.FileMode.CreateNew);
                        fs.Close();
                    }
                    catch (Exception exp)
                    {

                        Statistics.UpdateLog(exp.Message);
                        return false;
                    }
                    return true;

                }
                /*
                 * this function is a callback function that called from WriteTimer each Interval ms
                 * comment: sends data from buffer sendQueu
                 */
                public static void updateSender(Object sender, EventArgs e)
                {
                    bool status;
                    /*
                     * fsm state send 
                     */
                    if (transferFsm == TransferState.send && System.IO.File.Exists(Util.covertChannel.useShareFile.SendAckFile))
                    {
                        try
                        {

                            System.IO.File.Delete(Util.covertChannel.useShareFile.SendAckFile);
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.SendAckMutex, Statistics.LogValue.Down, "Changing SendAck mutex down");

                        }
                        catch
                        {
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Error: can't Change SendAck mutex down");
                            //???????????
                            return;
                        }
                        /*next fsm state is idle*/
                        transferFsm = TransferState.idle;
                    }
                    /*fsm is idle and there is data to send*/
                    if (transferFsm == TransferState.idle && sendQueu.Count != 0 && System.IO.File.Exists(Util.covertChannel.useShareFile.SendFile) == false && System.IO.File.Exists(Util.covertChannel.useShareFile.SendAckFile) == false)
                    {
                        //try to send

                        int bit = sendQueu.Peek();
                        if (bit == 0)
                        {
                            status = FileCreate(Util.covertChannel.useShareFile.Bit0File);
                            if (status == false)
                            {
                                transferFsm = TransferState.idle;
                                Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Collition Detect in bit0");
                                //colision detect enumerate bits
                                return;
                            }
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0, "Sending bit 0");
                        }
                        else if (bit == 1)
                        {
                            status = FileCreate(Util.covertChannel.useShareFile.Bit1File);
                            if (status == false)
                            {
                                transferFsm = TransferState.idle;
                                Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Collition Detect in bit1");
                                //colision detect enumerate bits

                                return;
                            }
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1, "Sending bit 1");
                        }
                        else //ignore bit
                        {
                            sendQueu.Dequeue();
                            return;
                        }
                        status = FileCreate(Util.covertChannel.useShareFile.SendFile);
                        if (status == false)
                        {
                            //colision detect enumerate bits
                            transferFsm = TransferState.idle;
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Collition Detect in send");
                            if (bit == 0)
                            {
                                System.IO.File.Delete(Util.covertChannel.useShareFile.Bit0File);
                            }
                            else
                            {
                                System.IO.File.Delete(Util.covertChannel.useShareFile.Bit1File);
                            }
                            return;
                        }
                        sendQueu.Dequeue();
                        Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.SendMutex, Statistics.LogValue.Up, "Changing send Mutex to up");
                        Util.covertChannel.Statistics.bitSentCount++;
                        if (bit == 0)
                        {
                            Util.covertChannel.Statistics.zeroBitSendCount++;
                        }
                        /*next state is send*/
                        transferFsm = TransferState.send;
                    }
                }
                /*
                 * this function is a callback function that called from ReadTimer each Interval ms
                 * comment: update received Queu data
                 */
                public static void updateReciver(Object sender, EventArgs e)
                {
                    int bit = 0;
                    bool status;
                    /*state is idle*/
                    if (transferFsm == TransferState.idle && System.IO.File.Exists(Util.covertChannel.useShareFile.SendFile) == true)
                    {
                        Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.SendMutex, Statistics.LogValue.Up, "Send Mutex is up");
                        if (System.IO.File.Exists(Util.covertChannel.useShareFile.Bit0File))
                        {//received bit is 0
                            bit = 0;
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0, "recived bit 0");

                        }
                        else if (System.IO.File.Exists(Util.covertChannel.useShareFile.Bit1File))
                        {//received bit is 1
                            bit = 1;
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1, "recived bit 1");

                        }
                        else
                        {
                            //TODO: error

                        }
                        try
                        {
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Clear, Statistics.LogValue.Empty, "trying to Clear transmited bit file");
                            if (bit == 0)
                            {
                                System.IO.File.Delete(Util.covertChannel.useShareFile.Bit0File);
                            }
                            else
                            {
                                System.IO.File.Delete(Util.covertChannel.useShareFile.Bit1File);
                            }
                        }
                        catch (Exception)
                        {
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Error :Cant clear the transfered bit file");
                            return;
                        }
                        try
                        {
                            System.IO.File.Delete(Util.covertChannel.useShareFile.SendFile);
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.SendMutex, Statistics.LogValue.Down, "Changing send Mutex to down");
                            Util.covertChannel.Statistics.bitReceivedCount++;
                            if (bit == 0)
                            {
                                Util.covertChannel.Statistics.zeroBitReceivedCount++;
                            }

                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.SendAckMutex, Statistics.LogValue.Up, "Changing sendAck Mutex to up");
                            status = FileCreate(Util.covertChannel.useShareFile.SendAckFile);
                            if (status == false)
                            {
                                Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Error :Cant change sendAck Mutex");
                            }

                            receivedBit(bit);
                        }
                        catch (Exception)
                        {
                            Util.covertChannel.Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Error, Statistics.LogValue.Empty, "Error :Cant change send Mutex");
                        }
                    }

                }


            }
            #endregion
            #region covert channel Over File Lock

            public static class useFileLock
            {
                #region Definitions

                /*
             * Timers for to update input an output
             */
                private static System.Windows.Forms.Timer ThreadWriter;
                private static System.Windows.Forms.Timer ThreadReader;

                public static String DefaultFilePath
                {
                    get
                    {
                        return "./";
                    }
                }
                public static String DefaultFileName
                {
                    get
                    {
                        return "test.txt";
                    }
                }
                private static String _FilePath = DefaultFilePath;
                public static String FilePath
                {
                    get
                    {
                        return _FilePath;
                    }
                    set
                    {
                        _FilePath = value;
                    }
                }
                private static String _FileName = DefaultFileName;
                public static String FileName
                {
                    get
                    {
                        return _FileName;
                    }
                    set
                    {
                        _FileName = value;
                    }
                }
                private static int ContinueOneBits = 0;
                private static int ContinueZeroBits = 0;
                private static int startBitCnt = 0;
                private static bool runningMode = false;
                #endregion
                static public void init()
                {
                    /*init input and output timmers*/
                    ThreadWriter = new System.Windows.Forms.Timer();
                    ThreadWriter.Interval = Interval;
                    ThreadWriter.Tick += new EventHandler(useFileLock.updateSender);

                    ThreadReader = new System.Windows.Forms.Timer();
                    ThreadReader.Interval = Interval;
                    ThreadReader.Tick += new EventHandler(useFileLock.updateReceiver);
                }
                static public void finalize()
                {
                    ThreadReader.Dispose();
                    ThreadWriter.Dispose();
                }
                public static void start()
                {
                    if (System.IO.File.Exists(FilePath + FileName) == false)
                    {
                        System.IO.File.Create(FilePath + FileName);
                    }
                    fileUnLock();
                    ThreadWriter.Start();
                    ThreadReader.Start();
                    Type = CovType.UseFileLock;
                }
                public static void stop()
                {
                    ThreadWriter.Stop();
                    ThreadReader.Stop();
                }
                private static void fileLock()
                {
                    FileInfo ifile = new FileInfo(FilePath + FileName);
                    ifile.IsReadOnly = true;

                }
                public static void fileUnLock()
                {
                    FileInfo ifile = new FileInfo(FilePath + FileName);
                    ifile.IsReadOnly = false;
                }
                public static bool isFileLocked()
                {
                    FileInfo ifile = new FileInfo(FilePath + FileName);
                    return ifile.IsReadOnly;
                }
                public static int calculateETA()
                {
                    int res = 0;
                    int[] msg;
                    msg = sendQueu.ToArray();
                    int waitTime = 8;
                    if (hammingErrorCorrection == true)
                    {
                        waitTime = 12;
                    }
                    foreach (int bit in msg)
                    {
                        if (bit == startBit)
                        {

                            if (res == 0)
                                res -= startBitCnt;
                            res += waitTime;
                        }
                        else if (bit == endBit)
                        {
                            res++;
                        }
                        else if (bit == synchronizationSilentBit)
                        {
                            res++;
                        }
                        else if (bit == 1)
                        {
                            res++;
                        }
                        else if (bit == 0)
                        {
                            res++;
                        }
                        else
                        {
                        }
                    }
                    res = res * Interval;
                    return res;
                }

                public static void updateReceiver(Object sender, EventArgs e)
                {
                    int waitTime = 8;
                    if (hammingErrorCorrection == true)
                    {
                        waitTime = 12;
                    }

                    if (isFileLocked() == true)//bit 1
                    {
                        ContinueZeroBits = 0;
                        if (runningMode == true)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1);
                            Util.covertChannel.receivedBit(1);
                        }

                        if (ContinueOneBits++ >= waitTime)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "Start Message");
                            runningMode = true;
                            ContinueOneBits = 0;
                        }
                        Statistics.bitReceivedCount++;
                    }
                    else//bit 0
                    {
                        ContinueOneBits = 0;
                        
                        if (runningMode == true)
                        {
                            Util.covertChannel.receivedBit(0);
                            Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0);

                            if (ContinueZeroBits++ >= waitTime)
                            {
                                Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "End Message");
                                runningMode = false;
                                ContinueZeroBits = 0;
                            }
                           // Statistics.zeroBitReceivedCount++;
                            Statistics.bitReceivedCount++;
                        }
                    }

                }
                public static void updateSender(Object sender, EventArgs e)
                {
                    int waitTime = 8;
                    if (hammingErrorCorrection == true)
                    {
                        waitTime = 12;
                    }

                    if (sendQueu.Count != 0)
                    {
                        if (sendQueu.Peek() == 1)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1);
                            fileLock();
                            sendQueu.Dequeue();
                            Statistics.bitSentCount++;

                        }
                        else if (sendQueu.Peek() == 0)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0);
                            fileUnLock();
                            Statistics.zeroBitSendCount++;
                            sendQueu.Dequeue();
                            Statistics.bitSentCount++;
                        }
                        else if (sendQueu.Peek() == startBit)
                        {
                            /*lock file for waitTime intervals to sign the srtart of the message*/
                            fileLock();
                            Statistics.bitSentCount++;
                            if (startBitCnt++ >= waitTime)
                            {
                                Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "start message");

                                sendQueu.Dequeue();

                                startBitCnt = 0;
                            }

                        }
                        else if (sendQueu.Peek() == endBit)
                        {
                            /*unlock file for waitTime intervals to sign the srtart of the message*/
                           /* fileUnLock();
                            Statistics.bitSentCount++;
                            Statistics.zeroBitSendCount++;
                            if (endBitCnt++ >= waitTime)
                            {
                                Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "end message");

                                sendQueu.Dequeue();
                                endBitCnt = 0;
                            }
                            */
                            sendQueu.Dequeue();
                        }
                       
                    }

                }
            }
            #endregion

            #region covert channel Over Shared CPU usage
            public static class useSharedCpu
            {
                public static int oneBitPer = 45;
                public static class worker
                {
                    private static System.Threading.Thread ThreadWorker;
                    private static System.Diagnostics.PerformanceCounter counterCpuUsage;
                    private static bool working = false;
                    public static void useHightCpu()
                    {
                        working = true;
                    }
                    public static void useLowCpu()
                    {
                        working = false;
                    }
                    public static bool isHightCpu()
                    {
                        //Todo: change function
                        float cputime = counterCpuUsage.NextValue();
                        if (cputime > oneBitPer)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    
                    }
                    public static void init()
                    {
                        ThreadWorker = new System.Threading.Thread(updateWork);
                        counterCpuUsage = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total");
                    }
                    public static void start()
                    {
                        ThreadWorker.Start();
                    }
                    public static void stop()
                    {
                        ThreadWorker.Abort();
                        init();
                    }
                    public static void finalize()
                    {
                        ThreadWorker.Abort();
                    }
                    private static void updateWork()
                    {
                        while (true)
                        {
                            if (working == false)
                            {
                                System.Threading.Thread.Sleep(10);
                            }
                        }
                    }
                }
                #region Definitions

                /*
             * Timers for to update input an output
             */
                private static System.Windows.Forms.Timer ThreadWriter;
                private static System.Windows.Forms.Timer ThreadReader;
                

               
                private static int ContinueOneBits = 0;
                private static int ContinueZeroBits = 0;
                private static int startBitCnt = 0;
                private static bool runningMode = false;
                #endregion
                static public void init()
                {
                    /*init input and output timmers*/
                    ThreadWriter = new System.Windows.Forms.Timer();
                    ThreadWriter.Interval = Interval;
                    ThreadWriter.Tick += new EventHandler(useSharedCpu.updateSender);

                    ThreadReader = new System.Windows.Forms.Timer();
                    ThreadReader.Interval = Interval;
                    ThreadReader.Tick += new EventHandler(useSharedCpu.updateReceiver);

                    worker.init();
                }
                static public void finalize()
                {
                    ThreadReader.Dispose();
                    ThreadWriter.Dispose();
                    worker.finalize();
                }
                public static void start()
                {
                    ThreadWriter.Start();
                    ThreadReader.Start();
                    worker.start();
                    Type = CovType.UseCPU;
                }
                public static void stop()
                {
                    ThreadWriter.Stop();
                    ThreadReader.Stop();
                    worker.stop();
                    worker.useLowCpu();
                }


                public static int calculateETA()
                {
                    int res = 0;
                    int[] msg;
                    msg = sendQueu.ToArray();
                    int waitTime = 8;
                    if (hammingErrorCorrection == true)
                    {
                        waitTime = 12;
                    }
                    foreach (int bit in msg)
                    {
                        if (bit == startBit)
                        {
                            
                            if(res==0)
                                res -= startBitCnt;
                            res += waitTime;
                        }
                        else if (bit == endBit)
                        {
                            res++;
                        }
                        else if (bit == synchronizationSilentBit)
                        {
                            res++;
                        }
                        else if (bit == 1)
                        {
                            res++;
                        }
                        else if (bit == 0)
                        {
                            res++;
                        }
                        else
                        {
                        }
                    }
                    res = res * Interval;
                    return res;
                }

                public static void updateReceiver(Object sender, EventArgs e)
                {
                    int waitTime = 8;
                    if (hammingErrorCorrection == true)
                    {
                        waitTime = 12;
                    }

                    if (worker.isHightCpu() == true)//bit 1
                    {
                        ContinueZeroBits = 0;
                        if (runningMode == true)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1);
                            Util.covertChannel.receivedBit(1);
                        }

                        if (ContinueOneBits++ >= waitTime)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "Start Message");
                            runningMode = true;
                            ContinueOneBits = 0;
                        }
                        Statistics.bitReceivedCount++;
                    }
                    else//bit 0
                    {
                        ContinueOneBits = 0;

                        if (runningMode == true)
                        {
                            Util.covertChannel.receivedBit(0);
                            Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0);

                            if (ContinueZeroBits++ >= waitTime)
                            {
                                Statistics.UpdateLog(Statistics.LogSource.Receiver, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "End Message");
                                runningMode = false;
                                ContinueZeroBits = 0;
                            }
                            // Statistics.zeroBitReceivedCount++;
                            Statistics.bitReceivedCount++;
                        }
                    }

                }
                public static void updateSender(Object sender, EventArgs e)
                {
                    int waitTime = 8;
                    if (hammingErrorCorrection == true)
                    {
                        waitTime = 12;
                    }

                    if (sendQueu.Count != 0)
                    {
                        if (sendQueu.Peek() == 1)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit1);
                            worker.useHightCpu();
                            sendQueu.Dequeue();
                            Statistics.bitSentCount++;

                        }
                        else if (sendQueu.Peek() == 0)
                        {
                            Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Bit0);
                            worker.useLowCpu();
                            Statistics.zeroBitSendCount++;
                            sendQueu.Dequeue();
                            Statistics.bitSentCount++;
                        }
                        else if (sendQueu.Peek() == startBit)
                        {
                            /*lock file for waitTime intervals to sign the start of the message*/
                            worker.useHightCpu();
                            Statistics.bitSentCount++;
                            if (startBitCnt++ >= waitTime)
                            {
                                Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "start message");

                                sendQueu.Dequeue();

                                startBitCnt = 0;
                            }

                        }
                        else if (sendQueu.Peek() == endBit)
                        {
                            /*unlock file for waitTime intervals to sign the srtart of the message*/
                            /* fileUnLock();
                             Statistics.bitSentCount++;
                             Statistics.zeroBitSendCount++;
                             if (endBitCnt++ >= waitTime)
                             {
                                 Statistics.UpdateLog(Statistics.LogSource.Sender, Statistics.LogInstanceType.Transmition, Statistics.LogValue.Empty, "end message");

                                 sendQueu.Dequeue();
                                 endBitCnt = 0;
                             }
                             */
                            sendQueu.Dequeue();
                        }

                    }

                }
            }
            #endregion
            #region statistics counter
            public static class Statistics
            {
                //Counrer of received bits
                private static long _bitReceivedCount;
                public static long bitReceivedCount
                {
                    get
                    {
                        return _bitReceivedCount;
                    }
                    set
                    {
                        _bitReceivedCount = value;
                    }
                }
                private static long _zeroBitReceivedCount;
                public static long zeroBitReceivedCount
                {
                    get
                    {
                        return _zeroBitReceivedCount;
                    }
                    set
                    {
                        _zeroBitReceivedCount = value;
                    }
                }
                private static long _ErrorBitReceivedCount;
                public static long ErrorBitReceivedCount
                {
                    get
                    {
                        return _ErrorBitReceivedCount;
                    }
                    set
                    {
                        _ErrorBitReceivedCount = value;
                    }
                }
                private static long _zeroBitSendCount;
                public static long zeroBitSendCount
                {
                    get
                    {
                        return _zeroBitSendCount;
                    }
                    set
                    {
                        _zeroBitSendCount = value;
                    }
                }
                //couter of sent bits
                private static long _bitSentCount;
                public static long bitSentCount
                {
                    get
                    {
                        return _bitSentCount;
                    }
                    set
                    {
                        _bitSentCount = value;
                    }
                }
                #region Log File
                /*list of all logs*/
                public static System.Collections.ArrayList LogFile;

                /*log source*/
                public enum LogSource { Receiver, Sender, Unknown };
                /*log type*/
                public enum LogInstanceType { Transmition, Clear, SendMutex, SendAckMutex, Error };
                /*log value*/
                public enum LogValue { Up, Down, Bit0, Bit1, Empty };

                public class LogEntry
                {
                    public LogSource Source;
                    public LogInstanceType InstanceType;
                    public LogValue Value;
                    public String Comment;
                    public DateTime Time;

                    public LogEntry(LogSource _type, LogInstanceType _instanceType, LogValue _value, String _commend)
                    {
                        Time = DateTime.Now;
                        Source = _type;
                        InstanceType = _instanceType;
                        Comment = _commend;
                        Value = _value;
                    }
                }
                public static void init()
                {
                    _bitReceivedCount = 0;
                    _bitSentCount = 0;
                    _zeroBitReceivedCount = 0;
                    _zeroBitSendCount = 0;
                    LogFile = new System.Collections.ArrayList();
                }
                public static void UpdateLog(LogSource _type, LogInstanceType _instanceType, LogValue _value, String _comment)
                {
                    LogFile.Add(new LogEntry(_type, _instanceType, _value, _comment));
                }
                public static void UpdateLog(LogSource _type, LogInstanceType _instanceType, LogValue _value)
                {
                    LogFile.Add(new LogEntry(_type, _instanceType, _value, ""));
                }
                public static void UpdateLog(String _comment)
                {
                    LogFile.Add(new LogEntry(LogSource.Unknown, LogInstanceType.Error, LogValue.Empty, _comment));
                }
                #endregion
            }
            #endregion
        }

    }
}
