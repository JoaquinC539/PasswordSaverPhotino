#!/usr/bin/env bash
rm -rf build-dir
rm -rf PasswordSaver.flatpak
flatpak-builder --repo=repo --force-clean build-dir flatpak/com.JCOpenSoftware.PasswordSaver.json
flatpak build-bundle repo PasswordSaver.flatpak com.JCOpenSoftware.PasswordSaver
