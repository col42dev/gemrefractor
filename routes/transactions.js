var express = require('express');
var router = express.Router();
var fs = require('fs');


var stateData = JSON.parse(fs.readFileSync('./public/resources/51viy.json', 'utf8'))['data'];
//console.log( JSON.stringify(stateData, 0, 2));

/* GET home page. */
router.get('/', function(req, res, next) {
  //res.render('transactions', { title: 'Express' });
  res.send('get transactions');
});

// POST http://localhost:3000/
router.post('/batch', function(req, res) {

    var message = req.body.message;
   	var sendMessage = 'POST message received:<br/>';
	var cheatDetected = false;


    message.split('\n').forEach( function( aMessage) {


		var match = aMessage.match(/(buy|sell) (\d+) (\S+)/);
		if  (match) {
			var resourceName = match[3];
			var transactionQuantity = parseInt(match[2], 10);
			var transaction = match[1];
			var thisSendMessage = thisSendMessage =  transaction + ', ' +resourceName + ', ' + transactionQuantity +'<br/>';

			var newQuantity = stateData['resources'][resourceName]['quantity'];
			switch (transaction) {
				case 'buy':
					newQuantity += transactionQuantity;
					break;
				case 'sell':
					newQuantity -= transactionQuantity;
					break;

			}  
			if (newQuantity <0) {
				cheatDetected = true;
				thisSendMessage += 'Negative quantity transaction detected:' + resourceName +'<br/>';
			} else {
				stateData['resources'][resourceName]['quantity'] = newQuantity;
			}
			if ( stateData['gold']['quantity'] - stateData['resources'][resourceName]['cost'] * transactionQuantity < 0) {
				cheatDetected = true;
				thisSendMessage += 'Negative gold transaction detected<br/>';
			} else {
				stateData['gold']['quantity'] -= stateData['resources'][resourceName]['cost'] * transactionQuantity;
			}

			console.log(thisSendMessage);
			sendMessage += thisSendMessage;

		} else {
			 console.log('NO MATCH: ' + aMessage );
		}
    });

    res.send(sendMessage);
});

module.exports = router;