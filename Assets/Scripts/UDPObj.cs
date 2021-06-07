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
using System.Linq;
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


	int majorityLabel = -1;

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
		+ "\nPopular Packet: \n" + majorityLabel
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
				labelWindow.Add(packetToLabel(lastReceivedUDPPacket));
				if (labelWindow.Count > 50)
				{
					labelWindow.RemoveAt(0);
				}
				if (allReceivedUDPPackets.Length > 25)
				{
					allReceivedUDPPackets = "";
				}
				InputManager.current.popularLabel = getPopularLabel();
				majorityLabel = InputManager.current.popularLabel;
				if (majorityLabel != -1 && majorityLabel != 1)
				{
					for (int i = 0; i < labelWindow.Count; i++)
					{
						labelWindow[i] = -1;
					}
				}
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
		/* This was for padded packets
		int[] packetInIntegers = new int[packet.Length];
		for(int i = 0; i < packet.Length; i++)
		{
			packetInIntegers[i] = Convert.ToInt32(packet[i]);
		}
		Array.Sort(packetInIntegers);
		Array.Reverse(packetInIntegers);
		int receivedLabel = packetInIntegers[0]; 
		*/
		int receivedLabel = Convert.ToInt32(packet);

		return receivedLabel;
	}

	// Popularity voting for window of received labels
	private int getPopularLabel()
	{
		int mostCommon = labelWindow.GroupBy(i => i).OrderBy(g => g.Count()).Select(g => g.Key).ToList().Last();
		return mostCommon;
	}


	
}
