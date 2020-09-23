This is an application written with the .NETCore framework so that it could be deployed on a Linux OS like a Raspberry PI.
My goal is to write a simple application that measure my internet performance running unattended on a Raspberry PI.  
Here are some key technologies used for better or for worse.
1.  Use PM2 to handle automatic restart when the PI reboots for whatever reason.
2.  Collect data once per hour and upload KPI up to a MongoDB hosted on Atlas.
3.  I will write a seperate front-end app using React or Angular to display the data but that is not related to the project here.

Important to note is that I didn't write any of the speedtest capabilities but used a python project called speedtest-cli from 
https://github.com/sivel/speedtest-cli.git.

Ok, you must be asking by now, why choose .NETCore instead of Python.  Well, I am a C# and Javascript developer and I just wanted to
find an excuse to use .NET core.

You will have to fill in the appsettings.json file with your own details to use.