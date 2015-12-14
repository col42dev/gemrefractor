
echo Publishing
# scp -r /Users/colinmoore/cmoore2.pem ./dist/. ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/stv/.
#scp -r -i ../../AWS/cmoore2.pem ./gemologist_Data/. ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemologist/gemologist_Data/.
scp -r -i ../../AWS/cmoore2.pem ./gemologist.x86_64 ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemologist
scp -r -i ../../AWS/cmoore2.pem ./gemologist.x86 ec2-user@ec2-54-201-237-107.us-west-2.compute.amazonaws.com:~/nginx/html/gemologist