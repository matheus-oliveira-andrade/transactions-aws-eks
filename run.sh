#!/bin/bash

# check if build and push parameter is passed
if [[ "$@" =~ "--build-push" ]]; then
  echo "Running build and push of docker images"
  ./build-push.sh    
fi
  
echo "Creating secrets"
./k8s/create-secrets.sh

echo "Creating all applications k8s resources"
kubectl apply -f k8s --recursive