# Introduction
This is an application written with the .NETCore framework so that it could be deployed on a Linux OS like a Raspberry PI. My goal is to write a simple application that measure my internet performance running unattended on a Raspberry PI.
Here are some key technologies used for better or for worse.
- Use PM2 to handle automatic restart when the PI reboots for whatever reason.
- Collect data once per hour and upload KPI up to a MongoDB hosted on Atlas. 
- I will write a seperate front-end app using React or Angular to display the data but that is not related to the project here.

Important to note is that I didn't write any of the speedtest capabilities but used a python project called speedtest-cli from https://github.com/sivel/speedtest-cli.git.
Ok, you must be asking by now, why choose .NETCore instead of Python. Well, I am a C# and Javascript developer and I just wanted to find an excuse to use .NET core.
You will have to fill in the appsettings.json file with your own details to use.

# Setting up the PI-4
    - Download and flash Raspbian Buster using Balena Etcher on a SD card to be used by the PI.
    - At this point, you should be able to boot up the PI and I just connected the PI to my TV using an HDMI cable. Do the basic Wifi and localization setup.
    - Go to the terminal and type sudo raspi-config. Enable SSH so that you can telnet/putty to it remotely.
    - It is extremely helpful to be able to access the folders on the PI from another computer.  https://pimylifeup.com/raspberry-pi-samba/
    - Reboot the PI and now you can disconnect all all the cables and run it completely headless.
    - I use Putty to remote to the PI. First job is to update the NodeJS.sudo -s curl -sL https://deb.nodesource.com/setup_12.x | bash 
    - apt-get install -y nodejs
## Downloading and installing .NET Core Runtime and SDK
    - Install .NETCore. Go to dotnet.microsoft.com. You can only use Linux/ARM32 for raspbian
    - [You can read more details here: https://edi.wang/post/2019/9/29/setup-net-core-30-runtime-and-sdk-on-raspberry-pi-4]
    - Important! Don’t download the tar file but rather copy the directlink.
    - cd Downloads. It is optional but I download the tar file to this directory.
    - Download the link. wget https://download.visualstudio.microsoft.com/download/pr/8f0dffe3-18f0-4d32-beb0-dbcb9a0d91a1/abe9a34e3f8916478f0bd80402b01b38/dotnet-sdk-3.1.402-linux-arm.tar.gz
    - sudo mkdir /usr/share/dotnet 
    - sudo tar zxf dotnet-sdk-3.1.402-linux-arm.tar.gz -C /usr/share/dotnet/ 
    - go to /usr/share/dotnet and run ./dotnet just to test. 
    - sudo nano /home/pi/.bashrc 
         export PATH=$PATH:/usr/share/dotnet 
         export DOTNET_ROOT=/usr/share/dotnet 
    - sudo reboot
    - type dotnet to make sure it is now available from any folder.

# Cloning code and final setup.
    - git clone https://github.com/hujanais/speedtest-dotnetcore.git
    - go to the speedtest-cli folder and test to see the python script will run.  python speedtest.py
    - Edit the appsettings.json file with your personnel stuff. 
    - appsettings.json
    ```
    {
      "MONGODB_URL": "mongodb+srv://hujanais:WuELnPIx57P4CWxA@dryer-cluster-vru4n.mongodb.net/dryer",
      "DB_NAME": "<The name of the database>",
      "COLLECTION_NAME": "<The name of your collection>",
      "REFRESH_RATE_MS": 1800000, // This is the duration is ms on how often the speedtest will run.
    
      "PYTHON_FULLPATH": "python", // you can just use python on a PI or else use the full path where the python.exe is located. 
      "PYTHON_CMD": "speedtest-cli\\speedtest.py --json"  // DO NOT CHANGE THIS!
    }
    ```
    - You maybe need to change the folders to have write access.  Go to root directory of the git clone and run chmod -R 777 speedtest-dotnetcore.
    - Go to the folder that contains the .sln file and build the application with dotnet build.
    - If everything works out, the executable will be in the bin/Debug/netcoreapp3.1/ folder.  
    
# Using PM2 for 24/7 operation.
    - There will be situations where the PI might reboot because of application crashes, power glitches, etc.  
    Therefore to auto restart the application upon reboot, I suggest using PM2.
