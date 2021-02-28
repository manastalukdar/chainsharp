#*****************
# Tasks
#*****************

build:
	dotnet build

test:
	dotnet test --collect:"XPlat Code Coverage"

install-report-generator:
	dotnet tool install -g dotnet-reportgenerator-globaltool

generate-report:
	reportgenerator "-reports:./src/tests/**/TestResults/**/coverage.cobertura.xml" "-targetdir:artifacts/coveragereport" "-reporttypes:Html"

upload-to-codecov:
# https://gist.github.com/sighingnow/deee806603ec9274fd47
ifeq ($(OS),Linux)
	bash <(curl -s https://codecov.io/bash)
endif

#*****************
# All tasks
#*****************

build-test: build test

ci: build test install-report-generator generate-report upload-to-codecov

