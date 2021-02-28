#*****************
# OSFLAG
# https://gist.github.com/sighingnow/deee806603ec9274fd47
#*****************

OSFLAG 				:=

ifeq ($(OS),Windows_NT)
	OSFLAG += -D WIN32
	ifeq ($(PROCESSOR_ARCHITECTURE),AMD64)
		OSFLAG += -D AMD64
	endif
	ifeq ($(PROCESSOR_ARCHITECTURE),x86)
		OSFLAG += -D IA32
	endif
else
	UNAME_S := $(shell uname -s)
	ifeq ($(UNAME_S),Linux)
		OSFLAG += -D LINUX
	endif
	ifeq ($(UNAME_S),Darwin)
		OSFLAG += -D OSX
	endif
		UNAME_P := $(shell uname -p)
	ifeq ($(UNAME_P),x86_64)
		OSFLAG += -D AMD64
	endif
		ifneq ($(filter %86,$(UNAME_P)),)
	OSFLAG += -D IA32
		endif
	ifneq ($(filter arm%,$(UNAME_P)),)
		OSFLAG += -D ARM
	endif
endif

#*****************
# Tasks
#*****************

osflag:
	@echo $(OSFLAG)

build:
	dotnet build

test:
	dotnet test --collect:"XPlat Code Coverage"

install-report-generator:
	dotnet tool install -g dotnet-reportgenerator-globaltool

generate-report:
	reportgenerator "-reports:./src/tests/**/TestResults/**/coverage.cobertura.xml" "-targetdir:artifacts/coveragereport" "-reporttypes:Html"

upload-to-codecov:
ifeq ($(UNAME_S),Linux)
	bash < curl -s https://codecov.io/bash
else
	@echo "Not run on $(OS)."
endif

#*****************
# All tasks
#*****************

build-test: build test

ci: build test install-report-generator generate-report upload-to-codecov

