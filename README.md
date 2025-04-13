# Art Of Rally CoPilot
This mod integrates the audio as a real co -pilot, I started from the existing mod: [Art Of Rally Pace Notes](https://github.com/Theaninova/ArtOfRallyPaceNotes) 

Use only with WAV files from a second otherwise otherwise it does not load them and must be of the format: Not compressed PCM, 16 bit, stereo

NB: With my version of ffmpeg the wav file was not being read by Unity so I recoded with “fre:ac free audio converter” with wav setting “Signed 16 bit PCM”

In the “Make-assets” folder you will find the script to create the audios and the GIMP file for the images. The script starts with “python3 genera-audio-wav.py” and has as dependency “pip install edge-tts”

### Requirements on GNU/Linux (Debian and derivatives)
sudo apt install mono-complete msbuild nuget

wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

sudo dpkg -i packages-microsoft-prod.deb

rm packages-microsoft-prod.deb

sudo apt update

sudo apt install -y apt-transport-https

sudo apt install -y dotnet-sdk-3.1

### Compilation on GNU/Linux (Debian and derivatives)

cd ArtOfRallyCoPilot

rm -rf obj/

rm -rf bin/

dotnet restore

nuget restore ArtOfRallyCoPilot.csproj

msbuild ArtOfRallyCoPilot.csproj

