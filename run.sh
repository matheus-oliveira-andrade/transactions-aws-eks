#!/bin/bash

# check if build and push parameter is passed
if [[ "$@" =~ "--build-push" ]]; then
  echo "Running build and push of docker images"
  ./build-push.sh    
fi

echo "Installing ingres nginx"
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml

echo "Waiting for the ingress controller pod to be up, running, and ready"
kubectl wait --namespace ingress-nginx \
  --for=condition=ready pod \
  --selector=app.kubernetes.io/component=controller \
  --timeout=120s
  
echo "Creating secrets"
./k8s/create-secrets.sh

echo "Creating all applications k8s resources"
kubectl apply -f k8s --recursive