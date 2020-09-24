# Introduction
This is an application written with the .NETCore framework so that it could be deployed on a Linux OS like a Raspberry PI. My goal is to write a simple application that measure my internet performance running unattended on a Raspberry PI.
Here are some key technologies used for better or for worse.

    Use PM2 to handle automatic restart when the PI reboots for whatever reason.
    Collect data once per hour and upload KPI up to a MongoDB hosted on Atlas.
    I will write a seperate front-end app using React or Angular to display the data but that is not related to the project here.

Important to note is that I didn't write any of the speedtest capabilities but used a python project called speedtest-cli from https://github.com/sivel/speedtest-cli.git.
Ok, you must be asking by now, why choose .NETCore instead of Python. Well, I am a C# and Javascript developer and I just wanted to find an excuse to use .NET core.
You will have to fill in the appsettings.json file with your own details to use.

# Setting up the PI-4
    - Download and flash Raspbian Buster using Balena Etcher on a SD card to be used by the PI.
    - At this point, you should be able to boot up the PI and I just connected the PI to my TV using an HDMI cable. Do the basic Wifi and localization setup.
    - Go to the terminal and type sudo raspi-config. Enable SSH so that you can telnet/putty to it remotely.
    - Reboot the PI and now you can disconnect all all the cables and run it completely headless.
    - I use Putty to remote to the PI. First job is to update the NodeJS.sudo -s curl -sL https://deb.nodesource.com/setup_12.x | bash - apt-get install -y nodejs
    - Install .NETCore. Go to dotnet.microsoft.com. You can only use Linux/ARM32 for raspbian 
    - Don’t download but rather copy the directlink. 
    - SSH over to PI and wget the [directlink] 
    - sudo mkdir /usr/share/dotnet 
    - sudo tar zxf dotnet-sdk-3.1.107-linux-arm.tar.gz -C /usr/share/dotnet/ 
    - go to /usr/share/dotnet and run ./dotnet just to test. 
    - sudo nano ~/.profileX export PATH=$PATH:/usr/share/dotnet export DOTNET_ROOT=/usr/share/dotnet 
    - sudo reboot

