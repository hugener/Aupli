#Configure PI
sudo apt-get update
sudo apt-get upgrade
sudo raspi-config
- Expand CD card
- Enable SPI
- Enable I2C
- Set device name
- Enable SSH
- Set keyboard layout, locale

# Setup WIFI
sudo nano /etc/wpa_supplicant/wpa_supplicant.conf

ctrl_interface=DIR=/var/run/wpa_supplicant GROUP=netdev
network={
    ssid="YOUR_NETWORK_NAME"
    psk="YOUR_PASSWORD"
    key_mgmt=WPA-PSK
}

# Update config:
sudo /boot/config.txt

# Uncomment some or all of these to enable the optional hardware interfaces
dtparam=i2c_arm=on
dtparam=i2c=on
dtparam=i2c1=on
dtparam=spi=on

# Uncomment this to enable the lirc-rpi module
dtoverlay=lirc-rpi,gpio_in_pin=18

# Additional overlays and parameters are documented /boot/overlays/README

# Enable audio (loads snd_bcm2835)
dtparam=audio=on

# Update modules
sudo nano /etc/modules
i2c-dev
i2c-bcm2708
lirc_dev
lirc_rpi gpio_in_pin=18

#Update rc.local
sudo nano /etc/rc.local

(sleep 100 && sudo systemctl restart lircd.service lircd.socket) &

# Install MPD
sudo apt-get install mpd mpc alsa-utils

sudo mkdir Music
sudo mkdir Playlists
sudo chmod -R 777 /home/pi/Music
sudo chmod -R 777 /home/pi/Playlists

# Update mpd.conf
sudo nano /etc/mpd.conf

music_directory         "/home/pi/Music"
playlist_directory		"/home/pi/Playlists"

amixer sset Speaker,0 100%

# Install lirc
sudo apt-get install lirc

# Update lirc_options.conf
sudo nano /etc/lirc/lirc_options.conf

driver    		= default
device    		= /dev/lirc0
listen          = 0.0.0.0:8765

# Add Aupli.lircd.conf and copy contents into it
sudo nano /etc/lirc/lircd.conf.d/Aupli.lircd.conf

sudo systemctl enable Aupli.service

# Update lircd.service
sudo nano /lib/systemd/system/lircd.service

ExecStart=/usr/sbin/lircd --nodaemon --listen

# Install .NET Core runtime
' Use the apt-get package manager to install three prerequiste packages.
sudo apt-get install curl libunwind8 gettext
' Download the latest .NET Core Runtime for ARM32. This is refereed to as armhf on the Daily Builds page.
curl -sSL -o dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/release/2.0.0/dotnet-runtime-latest-linux-arm.tar.gz 
' Create a destination folder and extract the downloaded package into it.
sudo mkdir -p /opt/dotnet && sudo tar zxf dotnet.tar.gz -C /opt/dotnet
' Set up a symbolic link to the dotnet executable.
sudo ln -s /opt/dotnet/dotnet /usr/local/bin

sudo mkdir Aupli

# Install upnp for MPD
sudo nano /etc/apt/sources.list.d/upmpdcli.list

deb http://www.lesbonscomptes.com/upmpdcli/downloads/raspbian-stretch/ stretch main
deb-src http://www.lesbonscomptes.com/upmpdcli/downloads/raspbian-stretch/ stretch main

sudo apt-get update
sudo apt-get install upmpdcli

sudo nano /etc/upmpdcli.conf
friendlyname = Laia-Aupli

# Publish sources from Package manager console:
dotnet publish -r linux-arm

Transfer published output to /home/pi/Aupli using WinSCP etc.


