.DEFAULT_GOAL := all

MAKEFILE_DIR = $(dir $(realpath $(firstword $(MAKEFILE_LIST))))

DOTNET_TEST = dotnet test --no-build --no-restore --verbosity normal


.PHONY: help
help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

.PHONY: all
all: restore build test publish

.PHONY: restore
restore: 
	dotnet restore Checkout.PaymentsGateway.sln

.PHONY: build 
build:
	dotnet build Checkout.PaymentsGateway.sln

.PHONY: test
test:  
	$(DOTNET_TEST) --results-directory "/test-results" --logger "trx;LogFilePrefix=/test-results/" Checkout.PaymentsGateway.sln
	
.PHONY: publish
publish: 
	dotnet publish ./src/Checkout.PaymentsGateway.Api/Checkout.PaymentsGateway.Api.csproj \
		--configuration Release \
		--output /app
	
.PHONY: clean
clean: ## Removes all build artifacts
	find . -iname "bin" | xargs rm -rf
	find . -iname "obj" | xargs rm -rf
	find . -iname "publish" | xargs rm -rf
	find . -iname "artifacts" | xargs rm -rf
	find . -iname "test-results" | xargs rm -rf
	find . -iname "testresult*" | xargs rm -rf