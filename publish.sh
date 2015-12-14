
echo Publishing
scp -r -i ../AWS/cmoore2.pem ./*.js ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/.
scp -r -i ../AWS/cmoore2.pem ./*.json ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/.
scp -r -i ../AWS/cmoore2.pem ./bin/. ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/bin/.
scp -r -i ../AWS/cmoore2.pem ./routes/. ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/routes/.
scp -r -i ../AWS/cmoore2.pem ./views/. ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/views/.
scp -r -i ../AWS/cmoore2.pem ./public/. ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/public/.

