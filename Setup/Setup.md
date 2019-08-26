# Install Arch Linux
Follow the guide on: https://archlinuxarm.org/platforms/armv8/broadcom/raspberry-pi-3

# Booting to U-Boot
In my case when booting, the Raspberry Pi 3 would always end up in U-Boot.

Originally, this project used a Raspberry Pi 2 build, which didn't have this problem.

The issue is that U-Boot had activated serial for stderr, stdin and stdout.

Fix this issue by running the following commands:
```
setenv stderr vidconsole
setenv stdin usbkbd
setenv stdout vidconsole
setenv preboot "start usb; setenv stdin nulldev" 
saveenv
```
You might want to check whether something else is set using printenv.

Optionally, bootdelay can be set to 0 for faster boot.
```
setenv bootdelay 0
saveenv
```

# Setup pacman and update system
```
pacman-key --init
pacman-key --populate archlinuxarm
pacman -Syyuu
```

# Initial setup (Users)
Login as root, password is root

```
su
```

Change root password
```
passwd root
```

Create the pi user:
```
useradd -m pi
passwd pi
```

Delete alarm user
```
userdel alarm
```

Install sudo and add pi user as sudoer
```
pacman -S sudo
gpasswd -a pi wheelgpass
visudo
```
Comment both "%wheel" lines in, by removing #

:x followed by enter to save

Change hostname:
```
hostnamectl set-hostname Aupli
```
Setup keyboard layout by adding: KEYMAP="dk"
```
nano /etc/vconsole.conf
```
Reboot
```
reboot
```

# Setup WIFI
Copy example profile
```
sudo cp /etc/netctl/examples/wireless-wpa /etc/netctl/PROFILE-NAME
```
Change ESSID and key
```
sudo nano /etc/netctl/PROFILE-NAME
```
Enable profile
```
sudo netctl enable PROFILE-NAME
```

# Configure boot and device tree
```
sudo nano /boot/config.txt
```
Add the settings:
```
dtparam=i2c_arm=on
dtparam=i2c=on
dtparam=i2c1=on
dtparam=spi=on
dtoverlay=lirc-rpi,gpio_in_pin=18
dtparam=audio=on
disable_splash=1
dtoverlay=sdtweak,overclock_50=100
boot_delay=0
```

# Update modules
```
sudo nano /etc/modules
```
Add the following lines:
```
i2c-dev
i2c-bcm2835
lirc_dev
lirc_rpi gpio_in_pin=18
```
# Install MPD
```
sudo pacman -S mpd mpc alsa-utils

sudo mkdir Music
sudo mkdir Playlists
sudo chmod -R 777 /home/pi/Music
sudo chmod -R 777 /home/pi/Playlists
```

# Update mpd.conf
```
sudo nano /etc/mpd.conf
```
Change the lines:
```
music_directory         "/home/pi/Music"
playlist_directory		"/home/pi/Playlists"
```

Set audio output to max
```
sudo alsamixer
```
# Install lirc
```
sudo pacman -S lirc
```
# Update lirc_options.conf
```
sudo nano /etc/lirc/lirc_options.conf
```
Change the lines:
```
driver    		= default
device    		= /dev/lirc0
listen          = 0.0.0.0:8765
```
# Add Aupli.lircd.conf and copy contents into it
```
sudo nano /etc/lirc/lircd.conf.d/Aupli.lircd.conf
```
# Add Aupli.service and copy contents into it
```
sudo nano /lib/systemd/system/Aupli.service

sudo systemctl enable Aupli.service
```
# Update lircd.service
```
sudo nano /lib/systemd/system/lircd.service
```
Change the line:
```
ExecStart=/usr/sbin/lircd --nodaemon --listen
```
# Install .NET Core runtime
Download the latest .NET Core Runtime for ARM64. This is refereed to as armhf on the Daily Builds page.
https://dotnet.microsoft.com/download/dotnet-core/3.0

```
curl -sSL -o dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/9605c24e-9bb2-499f-a003-4fb2bddcf09c/a8683ff89405d370961beb1909ddc295/dotnet-sdk-2.2.300-linux-arm64.tar.gz
```
Create a destination folder and extract the downloaded package into it.
```
sudo mkdir -p /opt/dotnet && sudo tar zxf dotnet.tar.gz -C /opt/dotnet
```
Set up a symbolic link to the dotnet executable.
```
sudo ln -s /opt/dotnet/dotnet /usr/local/bin

sudo mkdir Aupli
```
# Install upnp for MPD
pacman -S --needed base-devel git wget yajl

git clone https://aur.archlinux.org/package-query.git 
cd package-query 
makepkg -si 
cd .. 
git clone https://aur.archlinux.org/yaourt.git 
cd yaourt 
makepkg -si 

yaourt -S upmpdcli

sudo nano /etc/upmpdcli.conf
friendlyname = Laia-Aupli

# Publish sources from Package manager console:
dotnet publish -r linux-arm -c Release .\Aupli\Aupli.csproj

Transfer published output to /home/pi/Aupli using WinSCP etc.