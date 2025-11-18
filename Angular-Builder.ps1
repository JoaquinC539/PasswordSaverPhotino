cd frontpsaver;
npm run build;
Remove-Item ../browser -Recurse;
Remove-Item ../Platforms/Android/Assets/browser -Recurse;
Copy-Item ./dist/browser ../ -Recurse;
Copy-Item ./dist/browser ../Platforms/Android/Assets/browser -Recurse;
cd ..;