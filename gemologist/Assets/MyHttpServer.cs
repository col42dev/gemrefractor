using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading;
using System.Text.RegularExpressions;
using SimpleJSON;

public class MyHttpServer : MonoBehaviour {

	public class SimpleHttpServer : Bend.Util.HttpServer {

		List<string> mPostLog = new List<string> ();
		public SimpleHttpServer(int port)
		: base(port) {
		}
		public override void handleGETRequest(Bend.Util.HttpProcessor p) {
			/*
			Debug.Log("GET request:" + p.http_url);
			p.writeSuccess();
			p.outputStream.WriteLine("<html><body><h1>test server</h1>");
			p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
			p.outputStream.WriteLine("url : {0}", p.http_url);
			
			p.outputStream.WriteLine("<form method=post action=/form>");
			p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
			p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
			p.outputStream.WriteLine("</form>");*/

			p.writeSuccess();
			p.outputStream.WriteLine("<html><body><h1>gemologist - unity server log output</h1>");
			p.outputStream.WriteLine("<pre>");
			foreach (string log in mPostLog) {
				p.outputStream.WriteLine (log + "\n");
			}
			p.outputStream.WriteLine("</pre>");
			mPostLog.Clear();
		}
		
		public override void handlePOSTRequest(Bend.Util.HttpProcessor p, StreamReader inputData) {
			string data = inputData.ReadToEnd();


			p.outputStream.WriteLine("<html><body><h1>test server</h1>");
			p.outputStream.WriteLine("<a href=/test>return</a><p>");
			p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);


			if (data != null && data.Length > 0) {
				Debug.Log("POST request: " + p.http_url + ", POST data: " + data);
				

				mPostLog.Add (data);
				
				p.outputStream.WriteLine ("handlePOSTRequest:");

				using (StringReader sr = new StringReader(data)) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						p.outputStream.WriteLine (line);

						ParseTransactions (line);
					}
				}
			} else {
				mPostLog.Add ("null data received.");
				Debug.Log("null data received");
				p.outputStream.WriteLine ("null data received");
			}

		}

		private void ParseTransactions(string transactions)
		{
			mPostLog.Add ("gemologist parse transactions.");

			JSONNode jsontransaction = JSONNode.Parse ( transactions);
			transactions = (string)jsontransaction ["transactions"];

			Debug.Log ("transactions value:" + transactions);
			mPostLog.Add ("transactions value:" + transactions);


			Regex regex = new Regex ("<br/>");
			string[] transactionsArray = regex.Split(transactions);

			foreach (string transaction in transactionsArray) {
				mPostLog.Add ("transaction:" + transaction);

				Match m = Regex.Match (transaction, @"(buy|sell)(\d+)(\S+)");
				if (m.Success) {
					Debug.Log ("Match Success.");
					mPostLog.Add ("Match Success.");
		
					string resourceName = m.Groups [3].ToString ();
					int transactionQuantity = int.Parse (m.Groups [2].ToString ());
					string transactionType = m.Groups [1].ToString ();
	
					Debug.Log (resourceName + "," + transactionQuantity + "," + transactionType);
					mPostLog.Add (resourceName + "," + transactionQuantity + "," + transactionType);

					int newQuantity = SampleState.Instance.mState ["resources"] [resourceName] ["quantity"].AsInt;
					switch (transactionType) {
					case "buy":
						newQuantity += transactionQuantity;
						break;
					case "sell":
						newQuantity -= transactionQuantity;
						break;
					}  

					if (newQuantity < 0) {
						mPostLog.Add ("Negative quantity transaction detected:" + resourceName);
					} else {
						SampleState.Instance.mState ["resources"] [resourceName] ["quantity"] = new JSONData (newQuantity);
					}

					if (SampleState.Instance.mState ["gold"] ["quantity"].AsInt - SampleState.Instance.mState ["resources"] [resourceName] ["cost"].AsInt * transactionQuantity < 0) {
						mPostLog.Add ("Negative gold transaction detected:" + resourceName);

					} else {
						int value = SampleState.Instance.mState ["gold"] ["quantity"].AsInt - SampleState.Instance.mState ["resources"] [resourceName] ["cost"].AsInt * transactionQuantity;
						SampleState.Instance.mState ["gold"] ["quantity"] = new JSONData (value);
					}
				} else {
					Debug.Log ("Match Failed: " + transaction);
					mPostLog.Add ("Match Failed: " + transaction);
				}
			}
			return;
		}

	}

	SimpleHttpServer mSimpleHttpServer = null;
	Thread mThread = null;

	// Use this for initialization
	void Start () {

		Debug.Log ("MyHttpServer::Start on port 4000");
		mSimpleHttpServer = new SimpleHttpServer (4000);

		mThread = new Thread(new ThreadStart(mSimpleHttpServer.listen));
		mThread.Start();
	}

	void OnApplicationQuit() {
		Debug.Log("Application ending after " + Time.time + " seconds");
		mThread.Abort ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}


