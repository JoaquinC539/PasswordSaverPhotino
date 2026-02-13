
flatpak-builder build-dir flatpak/com.JCOpenSoftware.PasswordSaver.json
flatpak-builder --user --install --force-clean build-dir flatpak/com.JCOpenSoftware.PasswordSaver.json
flatpak run com.JCOpenSoftware.PasswordSaver
flatpak uninstall com.JCOpenSoftware.PasswordSaver
flatpak-builder --repo=repo --force-clean build-dir flatpak/com.JCOpenSoftware.PasswordSaver.json
flatpak build-bundle build-dir PasswordSaver.flatpak flatpak/com.JCOpenSoftware.PasswordSaver.json
