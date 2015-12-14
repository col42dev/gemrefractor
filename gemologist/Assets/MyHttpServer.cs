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
			p.outputStream.WriteLine("<html><body><h1>POST Log output</h1>");
			p.outputStream.WriteLine("<pre>");
			foreach (string log in mPostLog) {
				p.outputStream.WriteLine (log);
			}
			p.outputStream.WriteLine("</pre>");
			mPostLog.Clear();
		}
		
		public override void handlePOSTRequest(Bend.Util.HttpProcessor p, StreamReader inputData) {
			string data = inputData.ReadToEnd();


			p.outputStream.WriteLine("<html><body><h1>test server</h1>");
			p.outputStream.WriteLine("<a href=/test>return</a><p>");
			p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);
			mPostLog.Add (data);


			if (data != null && data.Length > 0) {
				Debug.Log("POST request: " + p.http_url + ", POST data: " + data);
				

				mPostLog.Add (data);
				
				p.outputStream.WriteLine ("handlePOSTRequest:");

				using (StringReader sr = new StringReader(data)) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						p.outputStream.WriteLine (line);

						string parseResult = ParseTransaction (line);
						Debug.Log (parseResult);
						mPostLog.Add (parseResult);
						p.outputStream.WriteLine (parseResult);
					}
				}
			} else {
				Debug.Log("null data received");
				p.outputStream.WriteLine ("null data received");
			}

		}

		private string ParseTransaction(string transaction)
		{
			string thisSendMessage = "gemologist parse transaction";

			JSONNode jsontransaction = JSONNode.Parse ( transaction);
			transaction = (string)jsontransaction ["transaction"];

			Debug.Log ("transaction value:" + transaction);

			Match m = Regex.Match( (string)jsontransaction["transaction"], @"(buy|sell) (\d+) (\S+)");
			if (m.Success) {
				Debug.Log ("Match Success");
				thisSendMessage += "Match Success";
		
				string resourceName = m.Groups [3].ToString ();
				int transactionQuantity = int.Parse (m.Groups [2].ToString ());
				string transactionType = m.Groups [1].ToString ();
	
				Debug.Log (resourceName + "," + transactionQuantity + "," + transactionType);

				int newQuantity = SampleState.Instance.mState["resources"][resourceName]["quantity"].AsInt;
				switch (transactionType) {
					case "buy":
						newQuantity += transactionQuantity;
						break;
					case "sell":
						newQuantity -= transactionQuantity;
						break;
				}  

				if (newQuantity <0) {
					thisSendMessage += "Negative quantity transaction detected:" + resourceName +"\n";
				} else {
					SampleState.Instance.mState["resources"][resourceName]["quantity"] = new JSONData( newQuantity);
				}

				if ( SampleState.Instance.mState["gold"]["quantity"].AsInt - SampleState.Instance.mState["resources"][resourceName]["cost"].AsInt * transactionQuantity < 0) {
					thisSendMessage += "Negative gold transaction detected\n";
				} else {
					int value = SampleState.Instance.mState["gold"]["quantity"].AsInt - SampleState.Instance.mState["resources"][resourceName]["cost"].AsInt * transactionQuantity;
					SampleState.Instance.mState["gold"]["quantity"] = new JSONData( value);
				}
			} else {
				Debug.Log ("Match Failed: " + transaction);
			}
			return thisSendMessage;
		}

	}

	SimpleHttpServer mSimpleHttpServer = null;
	Thread mThread = null;

	// Use this for initialization
	void Start () {

		Debug.Log ("MyHttpServer::Start ");
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


