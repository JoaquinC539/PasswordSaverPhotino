#!/usr/bin/env bash
rm -rf build-dir
flatpak-builder --user --install --force-clean build-dir flatpak/com.JCOpenSoftware.PasswordSaver.json