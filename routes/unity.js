var express = require('express');
var router = express.Router();
var fs = require('fs');
var http = require('http');
var spawn = require('child_process').spawn;
var request = require("request");
var prc = null;
var sendMessage = 'ready to receive events...';
var formattedOutput = '';

/* GET home page. */
router.get('/', function(req, res, next) {
	formattedOutput = '';
	sendMessage.split('\n').forEach( function( aMessage) {
 		formattedOutput += aMessage + '<br/>';
 	});
 	res.send(formattedOutput);


});




//postTransaction('sell 1 oil');
//postTransaction('fffffffffgggggkhhgggjhjjjgj');


router.post('/', function(req, res) {

   	sendMessage += 'Unity:';
   	console.log('req:' + req);
 
    req.body.event.split('\n').forEach( function( aMessage) {

		var match = aMessage.match(/^launch$/);
		if  (match) {
		   	if (prc===null) {
				var unityAppPath = (process.platform === 'darwin') ? './gemologist/gemologist.app/Contents/MacOS/gemologist' : './gemologist/gemologist.x86_64';
				console.log('launching ' + unityAppPath);
				prc = spawn(unityAppPath);


				prc.stdout.setEncoding('utf8');
				prc.stdout.on('data', function (data) {
				    var str = data.toString()
				    var lines = str.split(/(\r?\n)/g);
				    console.log(lines.join(""));
				    sendMessage += lines.join("");
				});

				prc.on('close', function (code) {
				    console.log('process exit code ' + code);
				    sendMessage += 'process exit code ' + code;
				});
			}
		}

		var match = aMessage.match(/^kill$/);
		if  (match) {
			if (prc) {
				console.log('kill unity process');
	   			prc.kill();
	   			prc = null;
	   		}
		}


		var match = aMessage.match(/^transactions:(.*)/);
		if  (match) {

			var transactions = match[1];

			console.log('refractor transactions:' + transactions);
			sendMessage += 'refractor transactions:' + transactions;

			postTransaction(transactions);

		}
		res.send(sendMessage);

	});

    
});


function postTransaction( rawtransactions) {


  var transactions = '{"transactions":'+rawtransactions+'}';

 var post_options = {
      //host: 'api.myjson.com',
      //port: 80,
      //path: '/bins',

      host: 'localhost',
      port: 4000,
      path: '/',

      method: 'POST',
      headers: {
          'Content-Type': 'application/json; charset=utf-8',
          'Content-Length': Buffer.byteLength(transactions)
      }
  };


console.log('transactions:' + transactions);

var post_req = http.request(post_options, function(res) {

	  var chunks = [];

    res.setEncoding('utf8');

    //console.log('reponse:' + JSON.stringify(res));

    res.on('data', function (chunk) {
        //console.log('Response: ' + chunk);
        //sendMessage+= chunk;
        console.log('chunk');
        chunks.push(chunk);
    });


  res.on('end', function () {
    console.log('end');
    //var body = Buffer.concat(chunks);
    //console.log('body'+body.toString());

    console.log('body'+JSON.stringify(chunks));
  });


});



 
  post_req.on('error', function (e) {
    console.log('post_req error:' + e);
  });

  post_req.write(transactions);
  post_req.end();


}


module.exports = router;