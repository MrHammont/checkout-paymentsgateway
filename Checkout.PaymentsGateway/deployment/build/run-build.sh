#!/bin/sh -x
set -e

SCRIPT_DIR=$(dirname ${0})

mkdir -p ${SCRIPT_DIR}/artifacts/app
mkdir -p ${SCRIPT_DIR}/artifacts/test-results

echo "Creating build image..."
docker build \
    -t checkout-paymentgateway-build \
    -f ${SCRIPT_DIR}/Build.Dockerfile \
    --no-cache \
    ${SCRIPT_DIR}/../..    
echo "Build image 'checkout-paymentgateway-build' created"

OUTPUT_DIR=$(readlink -f ${SCRIPT_DIR}/artifacts)

echo "Running build..."
docker run \
    --rm \
    -v /${OUTPUT_DIR}/app:/app \
    -v /${OUTPUT_DIR}/test-results:/test-results \
    checkout-paymentgateway-build 
echo "Build Finished"

echo "Creating runtime image..."
docker build \
    -t checkout-paymentgateway-runtime \
    -f ${SCRIPT_DIR}/Runtime.Dockerfile \
    --no-cache \
    .
echo "Runtime image 'checkout-paymentgateway-runtime' created"