# gemrefractor

gemrefractor is a server app for the [gemminer](https://github.com/col42dev/gemminer) client. It receives  transaction requests from the client and forwards them on to a headless Unity instance for verification. 

## setup

gemrefactor is an expressjs application.

<pre>
$ express gemrefractor
$ cd gemrefractor
</pre>

install body-parser and update package.json
<pre>
$ npm install body-parser --save
</pre>

## install 
<pre>
$ npm install
</pre>

## launch  
<pre>
$ DEBUG=gemrefractor:* npm start
</pre>


## test

Use [postman](http://www.getpostman.com/) to test POST requests.

![postman](https://raw.githubusercontent.com/col42dev/gemrefractor/master/documentation/postman.png)


