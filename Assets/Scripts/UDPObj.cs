/*
-----------------------
UDP Object
-----------------------
Code pulled from here and modified
 [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]

Unity3D to MATLAB UDP communication 

Modified by: Sandra Fang 
2016
*/
using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPObj : MonoBehaviour
{

	//local host
	public string IP = "127.0.0.1";

	//Ports
	public int portLocal = 8000;
	public int portRemote = 8001;

	// Create necessary UdpClient objects
	UdpClient client;
	IPEndPoint remoteEndPoint;

	// Receiving Thread
	Thread receiveThread;
	// Message to be sent
	string strMessageSend = "";

	// info strings
	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";
	// clear this from time to time!

	List<int> labelWindow = new List<int>();

	// start from Unity3d
	public void Start ()
	{
		init ();
	}

	// OnGUI
	void OnGUI ()
	{
		Rect rectObj = new Rect (40, 10, 200, 400);
		GUIStyle style = new GUIStyle ();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box (rectObj, "# UDP Object Receive\n127.0.0.1:" + portLocal + "\n"
		+ "\nLast Packet: \n" + lastReceivedUDPPacket
		+ "\n\nAll Messages: \n" + allReceivedUDPPackets
			, style);

		strMessageSend = GUI.TextField (new Rect (40, 420, 140, 20), strMessageSend);
		if (GUI.Button (new Rect (190, 420, 40, 20), "send")) {
			sendData (strMessageSend + "\n");
		}  

	}

	// Initialization code
	private void init ()
	{
		// Initialize (seen in comments window)
		print ("UDP Object init()");

		// Create remote endpoint (to Matlab) 
		remoteEndPoint = new IPEndPoint (IPAddress.Parse (IP), portRemote);

		// Create local client
		client = new UdpClient (portLocal);

		// local endpoint define (where messages are received)
		// Create a new thread for reception of incoming messages
		receiveThread = new Thread (
			new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}

	// Receive data, update packets received
	private void ReceiveData ()
	{
		while (true) {

			try {
				IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
				byte[] data = client.Receive (ref anyIP);
				string text = Encoding.UTF8.GetString (data);
				print (">> " + text);
				lastReceivedUDPPacket = text;
				allReceivedUDPPackets = allReceivedUDPPackets + text;
				labelWindow.Add(packetToLabel(text));
				if (labelWindow.Count > 100)
				{
					labelWindow.RemoveAt(0);
				}
				InputManager.current.popularLabel = getPopularLabel();
				InputManager.current.testLabel = text;

			} catch (Exception err) {
				print (err.ToString ());
			}
		}
	}

	// Send data
	private void sendData (string message)
	{
		try {
			byte[] data = Encoding.UTF8.GetBytes (message);
			client.Send (data, data.Length, remoteEndPoint);
			
		} catch (Exception err) {
			print (err.ToString ());
		}
	}

	// getLatestUDPPacket, clears all previous packets
	public string getLatestUDPPacket ()
	{
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}

	//Prevent crashes - close clients and threads properly!
	void OnDisable ()
	{ 
		if (receiveThread != null)
			receiveThread.Abort (); 

		client.Close ();
	}

	// Extracts label from UDP packet
	private int packetToLabel(string packet)
	{
		int[] packetInIntegers = new int[packet.Length];
		for(int i = 0; i < packet.Length; i++)
		{
			packetInIntegers[i] = Convert.ToInt32(packet[i]);
		}
		Array.Sort(packetInIntegers);
		Array.Reverse(packetInIntegers);
		int receivedLabel = packetInIntegers[0];

		return receivedLabel;
	}

	// Popularity voting for window of received labels
	private int getPopularLabel()
	{
		Dictionary<int, int> labelVotes = new Dictionary<int, int>();
		for (int i = 0; i < labelWindow.Count; i++)
		{
			if (labelVotes.ContainsKey(labelWindow.IndexOf(i)))
			{
				labelVotes[labelWindow.IndexOf(i)]++;
			}
			else
			{
				labelVotes.Add(labelWindow.IndexOf(i), 0);
			}
		}
		bool myIndexer = false;
		KeyValuePair<int, int> currentPopular;
		foreach(KeyValuePair<int, int> label in labelVotes)
		{
			if (!myIndexer)
			{
				currentPopular = label;
				myIndexer = true;
			}
			if (label.Value > currentPopular.Value) currentPopular = label;
		}
		return currentPopular.Key;
	}

}
