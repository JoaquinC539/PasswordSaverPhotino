flatpak-builder build-dir flatpak/com.JCOpenSoftware.PasswordSaver.json
flatpak-builder --user --install --force-clean build-dir flatpak/com.JCOpenSoftware.PasswordSaver.json
flatpak run com.JCOpenSoftware.PasswordSaver
flatpak uninstall com.JCOpenSoftware.PasswordSaver
flatpak build-bundle build-dir PasswordSaver.flatpak com.JCOpenSoftware.PasswordSaver
