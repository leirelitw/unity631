using UnityEngine;
using System.Collections;

using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class ConnectionManager : MonoBehaviour
{
    //private TcpClient mySocket;
    //private NetworkStream theStream;
    //private StreamReader reader;

    //private Queue sockets = new Queue();
    //private Queue streams = new Queue();
    //private Queue readers = new Queue();

    private List<TcpClient> sockets = new List<TcpClient>();
    //private List<NetworkStream> streams = new List<NetworkStream>();
    //private List<StreamReader> readers = new List<StreamReader>();

    //needs to be IEnumerator for Coroutines to work
    public delegate IEnumerator Callback(Response eventArgs);

    //stores callback functions and their corresponding protocols
    private Dictionary<int, Callback> callbacks = new Dictionary<int, Callback>();

    
    void Awake()
    {
        //mySocket = null;
        //theStream = null;

    }

    // Use this for initialization
    void Start()
    {
        //setupSocket();
    }

    public bool setupSocket()
    {
        ////if already connected, close old socket
        //if (mySocket != null)
        //    mySocket.Close();


        try
        {
            //Debug.Log("Setting up new socket");
            TcpClient mySocket = new TcpClient(Constants.REMOTE_HOST, Constants.REMOTE_PORT);
            sockets.Add(mySocket);

            //NetworkStream stream = mySocket.GetStream();
            //streams.Add(stream);
            //readers.Add(new StreamReader(stream));

            Debug.Log(getCurrentMilliseconds() + ": Connected size: " + sockets.Count);
            return true;
        }
        catch (Exception e){
            Debug.Log(getCurrentMilliseconds() + ": Socket error: " + e);
        }

        return false;
    }

    public void readSockets()
    {

        //reads all the sockets
        int counter = 0;
        while (counter < sockets.Count)
        {
            try
            {
                NetworkStream stream = sockets[counter].GetStream();
                if (stream.DataAvailable)
                {
                    Debug.Log(getCurrentMilliseconds() + ": Reading line: ");
                    byte[] buffer = new byte[2048];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    stream.Flush();
                    //string reply = readers[counter].ReadLine();
                    string reply = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //removes last character since it's a newline
                    reply = reply.Substring(0, reply.Length - 1);
                    Debug.Log(getCurrentMilliseconds() + ": reply: " + reply+"|");
                    




                    //gets protocol of response and calls callback method
                    try
                    {
                        int protocol = Int32.Parse(reply.Substring(0, 3));



                        Response response = new Response();
                        response.response = reply.Substring(3); //removes first 3 characters relating to protocol

                        //calls callback method in new thread so as not to slow down update()
                        //try
                        //{
                        //    Debug.Log("Running callback THREAD");
                        //    //sends in a new thread
                        //    Callback callback = callbacks[protocol];
                        //    Thread newThread = new Thread(() => callback(response));
                        //    newThread.Start();
                        //    Debug.Log("Finished running callback");
                        //} catch(Exception ex)
                        //{
                        //if fails, then can't call callback in thread, so call it in coroutine
                        Debug.Log(getCurrentMilliseconds() + ": Running callback COROUTINE");
                        Callback callback = callbacks[protocol];
                        StartCoroutine(callback(response));
                        //callback(response);
                        Debug.Log(getCurrentMilliseconds() + ": Finished running callback");
                        //}
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(getCurrentMilliseconds() + ": Exception: " + ex.ToString());
                    }

                    closeSocket(counter);

                }
                else
                    counter++;
            } catch(Exception ex)
            {
                Debug.Log(getCurrentMilliseconds() + ": Trying to close socket: " + ex.ToString());
                closeSocket(counter);
            }
        }
        
    }

    public void closeSocket(int index)
    {
        //Debug.Log("ConnectionManager.cs closeSocket() at "+index);

        Debug.Log(getCurrentMilliseconds() + ":  index " + index+" | Len: " + sockets.Count);


        try
        {
            //don't close socket if it's already closed
            if (sockets[index] != null)
                sockets[index].Close();

            //streams[index].Flush();
            //streams[index].Close();

            sockets.RemoveAt(index);
            //streams.RemoveAt(index);
            //readers.RemoveAt(index);
        } catch(Exception ex)
        {
            Debug.Log(getCurrentMilliseconds() + ": tried closing socket " + index + " with len " + sockets.Count);
        }
    }

    //callback is the method to run during the very next response ConnectionManager receives
    public void send(string url, int callback_protocol, Callback callback)
    {
        //this.callback = callback;

        //sets callback method
        callbacks[callback_protocol] = callback;

        Debug.Log(getCurrentMilliseconds() + ": Starting sending thread");
        //sends in a new thread
        Thread newThread = new Thread(() => send2(url));
        newThread.Start();
        Debug.Log(getCurrentMilliseconds() + ": Ending sending thread");

        //StartCoroutine(send2(url));

    }

    public void send2(string url)
    {
        ////if not connected, then connect
        //if (mySocket == null)
        //    setupSocket();

        //// Detect if client disconnected
        //if (mySocket.Client.Poll(0, SelectMode.SelectRead))
        //{
        //    byte[] buff = new byte[1];
        //    if (mySocket.Client.Receive(buff, SocketFlags.Peek) == 0)
        //    {
        //        // Client disconnected
        //        setupSocket();
        //        Debug.Log("Set up socket again");
        //    }
        //}

        bool success = setupSocket();

        //if didn't open up new socket, stop
        if (!success)
            return;


        //gets newly opened network stream
        //NetworkStream stream = streams[streams.Count - 1];
        NetworkStream stream = sockets[sockets.Count - 1].GetStream();


        //String to_convert = "GET " + url + " HTTP1.1" + "\n";

        //might need this to send the request
        url += "\n";
        Debug.Log(getCurrentMilliseconds()+": To_send: " + url);

        // Convert string message to byte array.                 
        byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(url);
        // Write byte array to socketConnection stream.                 
        stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
        stream.Flush();

        //Debug.Log("Sent bytes");
        
    }

    public Boolean isConnected()
    {
        //if (mySocket != null)
        //    return true;

        //return false;

        return true;
    }

    long getCurrentMilliseconds()
    {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    // Update is called once per frame
    void Update()
    {
        readSockets();
        //StartCoroutine(readSockets());
    }
}
