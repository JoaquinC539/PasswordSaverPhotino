#!/usr/bin/env bash
set -e
rm -rf ./publish
cd ./src
cd ./frontpsaver

npm run build

rm -rf ../browser


cp -r ./dist/browser ../

cd ..

dotnet publish -c Release -r linux-x64 --self-contained true PasswordSaver.csproj
pwd
cp -r ./bin/Release/net10.0/linux-x64/publish ../

rm -rf ../flatpak/icons

mkdir -p ../flatpak/icons/scalable
mkdir -p ../flatpak/icons/16x16/apps
mkdir -p ../flatpak/icons/48x48/apps
mkdir -p ../flatpak/icons/64x64/apps
mkdir -p ../flatpak/icons/128x128/apps
mkdir -p ../flatpak/icons/256x256/apps
mkdir -p ../flatpak/icons/512x512/apps
mkdir -p ../flatpak/icons/scalable/apps

cp ./Resources/AppIcon/com.JCOpenSoftware.PasswordSave16.png ../flatpak/icons/16x16/apps/com.JCOpenSoftware.PasswordSaver.png
cp ./Resources/AppIcon/com.JCOpenSoftware.PasswordSave48.png ../flatpak/icons/48x48/apps/com.JCOpenSoftware.PasswordSaver.png
cp ./Resources/AppIcon/com.JCOpenSoftware.PasswordSave64.png ../flatpak/icons/64x64/apps/com.JCOpenSoftware.PasswordSaver.png
cp ./Resources/AppIcon/com.JCOpenSoftware.PasswordSave128.png ../flatpak/icons/128x128/apps/com.JCOpenSoftware.PasswordSaver.png
cp ./Resources/AppIcon/com.JCOpenSoftware.PasswordSave256.png ../flatpak/icons/256x256/apps/com.JCOpenSoftware.PasswordSaver.png
cp ./Resources/AppIcon/com.JCOpenSoftware.PasswordSave512.png ../flatpak/icons/512x512/apps/com.JCOpenSoftware.PasswordSaver.png
cp ./Resources/AppIcon/com.JCOpenSoftware.PasswordSave.svg ../flatpak/icons/scalable/apps/com.JCOpenSoftware.PasswordSaver.svg










