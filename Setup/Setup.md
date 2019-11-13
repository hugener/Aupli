# Getting started
Note that while this guide includes some instructions for Raspberry Pi 3 and 64 bit arch linux, there are currently problems with gpio and thus that platform is not yet supported.

## Setup from scratch
* [Start from 1.](#InstallFromScratch)

## Clean install image
* [Start from 3.](#CleanInstallImage)

## User image
* [Start from 4. (user and password is pi)](#UserImage)

## Aupli image
* [Start from 6 (user and password is pi)](#AupliImage)

# <a id="InstallFromScratch">1. Install Arch Linux</a>
Follow the guide on: https://archlinuxarm.org/platforms/armv8/broadcom/raspberry-pi-3

 ## 1.1 Booting to U-Boot
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

# 2. Setup pacman and update system
Login as root, password is root

```
su
```

```
pacman-key --init
pacman-key --populate archlinuxarm
pacman -Syyuu
pacman -Scc
```

# <a id="CleanInstallImage">3. Initial setup</a>
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
rm /home/alarm -r
```

Install sudo and add pi user as sudoer
```
pacman -S sudo
gpasswd -a pi wheel
visudo
```
Comment both "%wheel" lines in, by removing #

:x followed by enter to save

Set up locale
```
nano /etc/locale.gen
```
Uncomment en_US.UTF-8
```
locale-gen
localectl set-locale LANG=en_US.UTF-8
```
```
reboot
```
# <a id="UserImage">4. Hardware Configuration</a>
 ## 4.1 Configure boot and device tree
```
sudo nano /boot/config.txt
```
Add the settings:
```
dtparam=i2c_arm=on
dtparam=i2c=on
dtparam=i2c1=on
dtparam=spi=on
dtoverlay=gpio-ir,gpio_in_pin=18,gpio_in_pull=up
dtparam=audio=on
disable_splash=1
dtoverlay=sdtweak,overclock_50=100
boot_delay=0
```

 ## 4.2 Update modules
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
# 5. Install software:
 ## 5.1 Install MPD
```
sudo pacman -S mpd mpc alsa-utils
```
 ### 5.1.1 Set up folders, permissions and user
```
mkdir Music
mkdir Playlists
sudo gpasswd -a mpd pi
sudo chmod -R 750 /home/pi
sudo chmod -R 777 /home/pi/Music
sudo chmod -R 777 /home/pi/Playlists
```

### 5.1.2 Update mpd.conf
```
sudo nano /etc/mpd.conf
```
Change the lines:
```
# See: /usr/share/doc/mpd/mpdconf.example

pid_file           "/run/mpd/mpd.pid"
db_file            "/var/lib/mpd/mpd.db"
state_file         "/var/lib/mpd/mpdstate"
music_directory    "/home/pi/Music"
playlist_directory "/home/pi/Playlists"

bind_to_address    "localhost"

user               "mpd"

audio_output {
    type           "alsa"
    name           "Aupli"
    device         "hw:1,0"   # optional
    mixer_type     "null"     # optional
#   mixer_device   "default"  # optional
#   mixer_control  "PCM"      # optional
#   mixer_index    "0"        # optional
}

filesystem_charset "UTF-8"

auto_update        "yes"
```

Enable mpd service
```
sudo systemctl enable mpd
```

Set Speaker output to max
```
sudo alsamixer
```
## 5.2 Install lirc
```
sudo pacman -S lirc
```
 ### 5.2.1 Update lirc_options.conf
```
sudo nano /etc/lirc/lirc_options.conf
```
Change the lines:
```
driver        = default
device        = /dev/lirc0
listen        = 0.0.0.0:8765
```
 ### 5.2.2 Add Aupli.lircd.conf and copy contents into it
```
sudo nano /etc/lirc/lircd.conf.d/Aupli.lircd.conf
```

 ### 5.2.3 Update lircd.service
```
sudo nano /lib/systemd/system/lircd.service
```
Change the line:
```
ExecStart=/usr/sbin/lircd --nodaemon --listen
```
Enable the service
```
sudo systemctl enable lircd
```

 ## 5.3 Install upnp for MPD
```
sudo pacman -S --needed base-devel git wget yajl

git clone https://aur.archlinux.org/package-query.git 
cd package-query 
makepkg -si 
cd .. 
git clone https://aur.archlinux.org/yaourt.git 
cd yaourt 
makepkg -si 

yaourt -S upmpdcli

sudo systemctl enable upmpdcli
```
## 5.4 Install .NET Core runtime
Download the latest .NET Core Runtime for ARM64. This is refered to as armhf on the Daily Builds page.
https://dotnet.microsoft.com/download/dotnet-core/3.0

```
curl -sSL -o dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/5cbf9f66-7945-43e2-9b7c-351f900e9893/2fcd48f3d4db99283ebdb46daf9bacec/aspnetcore-runtime-3.0.0-linux-arm64.tar.gz

curl -sSL -o dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/e9d4b012-a877-443c-8344-72ef910c86dd/b5e729b532d7b3b5488c97764bd0fb8e/aspnetcore-runtime-3.0.0-linux-arm.tar.gz
```
Create a destination folder and extract the downloaded package into it.
```
sudo mkdir -p /opt/dotnet && sudo tar zxf dotnet.tar.gz -C /opt/dotnet
```
Set up a symbolic link to the dotnet executable.
```
sudo ln -s /opt/dotnet/dotnet /usr/local/bin

mkdir Aupli
```

 ## 5.5 Install Aupli
  ### 5.5.1 Publish sources from Package manager console:
```
dotnet publish -r linux-arm64 -c Release .\Aupli\Aupli.csproj
```
```
dotnet publish -r linux-arm -c Release .\Aupli\Aupli.csproj
```
Transfer published output to /home/pi/Aupli using WinSCP etc.

  ### 5.5.2 Add Aupli.service and copy contents into it
```
sudo nano /lib/systemd/system/Aupli.service

sudo systemctl enable Aupli.service
```
# <a id="AupliImage">6. Personalize setup</a>

 ## 6.1 Setup WIFI
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

 ## 6.2 Change hostname:
```
sudo hostnamectl set-hostname Aupli
```

 ## 6.3 Change upmp friendly name:
```
sudo nano /etc/upmpdcli.conf
```
```
friendlyname = Aupli
```
 ## 6.4 Change passwords: (Aupli image only)
```
su
```

Change root and pi password
```
passwd pi
passwd root
```
## 6.5 Reboot or shutdown if you need to create an image
```
reboot/shutdown now
```
# 7. Extend partition
Skip this step if you set up everything from scratch.
```
su
<enter password>
systemctl rescue
nano /boot/cmdline.txt
```
Change rw to ro in the line below:
```
root=/dev/mmcblk0p2 rw rootwait...

ctrl o
<enter>
ctrl x

nano /etc/fstab
```
Add the following lines below:

```
tmpfs   /var/log    tmpfs   nodev,nosuid    0   0
tmpfs   /var/tmp    tmpfs   nodev,nosuid    0   0

ctrl o
<enter>
ctrl x
reboot

su
<enter password>

fdisk /dev/mmcblk0
```
Fdisk commands
```
p
d
2
n
p
2
```
Make sure the beginning of old and new partition are the same.
<br>The p command above lists the start blocks
<br>If you are asked whether to remove the old signature press No.
```
<enter>
<enter>
<no>

w
```
Reboot the pick up changes and resize file system.
```
reboot

su
<enter password>

systemctl rescue
mount -o rw,remount /
resize2fs /dev/mmcblk0p2
mount -o ro,remount /
e2fsck /dev/mmcblk0p2
sync
nano /boot/cmdline.txt
```
Change ro back to rw in the line below:
```
root=/dev/mmcblk0p2 ro rootwait...

mount -o rw,remount /
nano /etc/fstab
```
Comment out the added lines below:
```
tmpfs   /var/log    tmpfs   nodev,nosuid    0   0
tmpfs   /var/tmp    tmpfs   nodev,nosuid    0   0

ctrl o
<enter>
ctrl x

reboot
```

# 8. Enjoy!
Start adding music and playlists in:
/home/pi/Music
/home/pi/Playlists

Mapping between Playlists and RFIDs happens in:
/home/pi/Aupli/playlists.json