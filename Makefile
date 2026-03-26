dev:
	cd src/ && dotnet watch

build:
	cd src/ && dotnet build

exec:
	cd src/ && ./bin/boxes

container:
	docker build -t boxes --file=./src/Dockerfile .
