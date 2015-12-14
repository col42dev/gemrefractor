
echo Publishing

scp -r -i ../../AWS/cmoore2.pem ./gemologist_Data/. ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/gemologist/gemologist_Data/.
scp -r -i ../../AWS/cmoore2.pem ./gemologist.x86_64 ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/gemologist
scp -r -i ../../AWS/cmoore2.pem ./gemologist.x86 ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemrefractor/gemologist