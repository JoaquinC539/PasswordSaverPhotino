#!/usr/bin/env bash

set -e

cd frontpsaver

npm run build

rm -rf ../browser

cp -r ./dist/browser ../

cd ..