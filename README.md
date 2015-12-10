# gemrefractor

gemrefractor is a server app for the gemengine client. It receives and validates transaction requests from the client.

## application setup

gemrefactor is an expressjs application.

<pre>
$ express gemrefractor
$ cd gemrefractor
</pre>

install body-parser and update package.json
<pre>
$ npm install body-parser --save
</pre>

<pre>
$ npm install
</pre>

## launch  
<pre>
$ DEBUG=gemrefractor:* npm start
</pre>


## test

Use [postman](http://www.getpostman.com/) to test POST requests.


