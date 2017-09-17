web:
	php -S 0.0.0.0:3000

build:
	./node_modules/webpack/bin/webpack.js

watch:
	./node_modules/webpack/bin/webpack.js --progress --colors --watch
